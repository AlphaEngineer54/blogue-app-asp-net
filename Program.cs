using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Projet_RedSocial_app.FilterActions;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Services;

var builder = WebApplication.CreateBuilder(args);

// Environnemnt variables
var envConnectionString = Environment.GetEnvironmentVariable("BLOGUE_DB_CONNECTION_STRING");
builder.Configuration["ConnectionStrings:BlogueDB_Connection"] = envConnectionString;


// Add DBContext
builder.Services.AddDbContext<BloguedbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlogueDB_Connection"));
});


// Configure the controller
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new UserConnectionFilterAction());
})
      .AddNewtonsoftJson(options =>
      {
          options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
      });


// Add Dependacy to the container
builder.Services.AddScoped<AuthentificationService>();
builder.Services.AddScoped<VotesServices>();
builder.Services.AddScoped<BlogueService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CommentService>();
builder.Services.AddScoped<PostService>();

builder.Services.AddSingleton<IPasswordHasher<Member>, PasswordHasher<Member>>();
builder.Services.AddSingleton<UserConnectionFilterAction>();


builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // FOR DOCKER ONLY
   // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // FOR AZURE
});


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", config =>
    {
        config.PermitLimit = 15; // number of requests allowed
        config.Window = TimeSpan.FromMinutes(1); // time window duration
        config.QueueLimit = 2; // max requests to queue
    });
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Configure les options de la session (facultatif)
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Durée de vie de la session (30 minutes par défaut)
    options.Cookie.HttpOnly = true; // Rend le cookie inaccessible via JavaScript
    options.Cookie.IsEssential = true; // Rendre le cookie essentiel pour le bon fonctionnement de l'application
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
   // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpContextAccessor();  
builder.Services.AddRazorPages();   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseRateLimiter();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Blogue}/{id?}");

app.Run();
