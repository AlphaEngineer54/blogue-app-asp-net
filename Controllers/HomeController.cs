using Microsoft.AspNetCore.Mvc;
using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Projet_RedSocial_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly BloguedbContext _DbContext;
     
        public HomeController(BloguedbContext dbContext)
        {
            this._DbContext = dbContext;
        }

        // GET : Home/Blogue
        [ActionName(name:"Blogue")]
        public async Task<IActionResult> Index()
        {
            var posts = await this._DbContext.Posts
                                            .Include(p => p.Member)
                                            .Include(p => p.PostLikes)
                                            .Include(p => p.PostDislikes)
                                            .Include(p => p.Comments)
                                            .OrderByDescending(p => p.PostLikes.Count)
                                            .ToListAsync();
            ViewBag.LoggedUser = GetCurrentUser();

            if (posts == null || posts.Count == 0)
            {
                ViewBag.Message = "Aucun post n'a été trouvé. Pour créer un nouveau post, aller dans mon profil et ajouter un nouveau post";
                return View();
            }

            return View(posts);
        }

        [ActionName(name:"BloguesFilter")]
        public IActionResult SearchPost()
        {
            var posts = JsonConvert.DeserializeObject<IList<Post>>(TempData["PostsSearchResult"] as string);
            ViewBag.LoggedUser = GetCurrentUser();

            if(posts != null)
            {
                return View("Blogue", posts);
            }
              
            return RedirectToAction("Blogue", "Home");
        }

        [HttpGet]
        [ActionName(name:"Contact")]
        public IActionResult GetContactView()
        {
            return View();
        }


        private Member GetCurrentUser()
        {
            var userLogged = HttpContext.Session.GetString("LoggedUser");
            var currentLoggedUser = JsonConvert.DeserializeObject<Member>(userLogged);
            return currentLoggedUser;
        }
    }
}
