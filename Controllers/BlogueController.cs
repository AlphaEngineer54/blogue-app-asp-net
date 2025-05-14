using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DTO.Blogue;
using Projet_RedSocial_app.Services;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Projet_RedSocial_app.Controllers
{
    public class BlogueController : Controller
    {
        private readonly BlogueService blogueService;
        
        public BlogueController(BlogueService blogueService)
        {
            this.blogueService = blogueService;
        }

        // GET: Blogue/Categorie/{categorie}
        public async Task<IActionResult> Categorie(string categorie)
        {
            var blogues = await this.blogueService.GetBlogueByCategorie(categorie);

            ViewBag.LoggedUser = GetCurrentUser();

            if (blogues == null || !blogues.Any())
            {
                ViewBag.Message = $"Aucun blogue trouvé pour la catégorie {categorie.ToUpper()}!";
                return View("Blogues");
            }

            ViewBag.Blogues = blogues;
            ViewBag.NumbersOfBlog = blogues.Count();
            return View("Blogues");
        }

        [HttpGet]
        [ActionName(name:"MyBlogues")]
        public async Task<IActionResult> GetAllBloguesByUser()
        {
            var loggedUser = GetCurrentUser();

            var blogues = await this.blogueService.GetAllBlogueByUser(loggedUser);
            
            TempData["UserBlogues"] = JsonConvert.SerializeObject(blogues);
            ViewBag.User = loggedUser.MemberId;
            ViewBag.LoggedUser = loggedUser;
            return RedirectToAction("MyBlogues", "User");
        }

        // GET: Blogue/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var blogue = await this.blogueService.GetBlogueByIdAsync(id);

            if (blogue == null)
            {
                return NotFound();
            }

            return PartialView("_BlogueCategorieResult", blogue.Posts);
        }

        // GET: Blogue/Create
        public IActionResult Create()
        {
            var loggedUser = GetCurrentUser();
            ViewBag.UserId = loggedUser.MemberId;
            return View("Create");
        }

        // POST: Blogue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogueDTO newBlogue)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = GetCurrentUser();
                var createdBlogue = await this.blogueService.CreateBlogueAsync(newBlogue, loggedUser);
                loggedUser.Blogues.Add(createdBlogue);
                UpdateSessionData(loggedUser);

                TempData["MessageSuccess"] = "Blogue crée avec succès!";
                return View(newBlogue);
            }

            return View(newBlogue);
        }

        // GET: Blogue/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var loggedUser = GetCurrentUser();

            var blogue = await this.blogueService.GetBlogueByIdAsync(id);
            if (blogue == null)
            {
                return NotFound();
            }

            var blogueDTO = new BlogueDTO()
            {
                Id = id,
                Title = blogue.Title,
                Description = blogue.Description,
                Categorie = blogue.Categorie
            };

            ViewBag.UserId = loggedUser.MemberId;
            return View("Update", blogueDTO);
        }

        // POST: Blogue/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogueDTO updatedBlogue)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = GetCurrentUser();

                var updatedB = await this.blogueService.UpdateBlogueAsync(updatedBlogue);

                if (updatedB != null)
                { 
                    loggedUser.Blogues.Add(updatedB);
                    UpdateSessionData(loggedUser);
                    TempData["MessageSuccess"] = "Blogue mis à jour avec succès!";
                    return View("Update", updatedBlogue);
                }
            }

            return View("Update", updatedBlogue);
        }

      

        // DELETE : Blogue/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Delete(int id)
        {
            var loggedUser = GetCurrentUser();

            var deleteB = await this.blogueService.DeleteBlogueAsync(id);

            if(deleteB != null)
            {
                loggedUser.Blogues.Remove(deleteB);
                UpdateSessionData(loggedUser);
            }

            var blogues = await this.blogueService.GetAllBlogueByUser(loggedUser);
            TempData["UserBlogues"] = JsonConvert.SerializeObject(blogues);
            return RedirectToAction("MyBlogues", "User");
        }

        private Member GetCurrentUser()
        {
            var user = HttpContext.Session.GetString("LoggedUser");
            var currentMember = JsonConvert.DeserializeObject<Member>(user);
            ViewBag.LoggedUser = currentMember;
            return currentMember;
        }

        private void UpdateSessionData(Member member)
        {
            HttpContext.Session.SetString("LoggedUser", JsonConvert.SerializeObject(member));   
        }
    }

}
