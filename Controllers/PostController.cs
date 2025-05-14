using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DTO;
using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Services;
using Projet_RedSocial_app.Models.DTO.Post;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Projet_RedSocial_app.Controllers
{
    /// <summary>
    /// Gère la logique métier pour les opérations sur les liens
    /// </summary>
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly BlogueService _blogueService;
        
        // Injection du service PostService
        public PostController(PostService postService, BlogueService blogueService)
        {
            this._postService = postService;
            this._blogueService = blogueService;    
        }

        // Action pour la recherche de posts
        [ActionName(name: "Search")]
        public async Task<IActionResult> GetPostByName(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                // Utilisation du service pour rechercher des posts
                var posts = await _postService.GetPostsByTitleAsync(query);

                if (posts != null && posts.Count != 0)
                {
                    TempData["PostsSearchResult"] = JsonConvert.SerializeObject(posts);
                    return RedirectToAction("BloguesFilter", "Home");
                }
            }

            return RedirectToAction("Blogue", "Home");
        }

        // Action pour récupérer un post par ID
        [ActionName(name: "Details")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);

            if (post == null)
            {
                ViewBag.Error = $"Le post avec l'ID {id} n'a pas pu être trouvé. Peut-être qu'il a été supprimé?";
                return PartialView("_PostDetail");
            }

            ViewBag.Error = TempData["Error"];
            return PartialView("_PostDetail", post);
        }

        // GET : Post/MyPosts
        [ActionName(name: "MyPosts")]
        public async Task<IActionResult> GetAll()
        {
            var user = GetCurrentUser();

            var userPosts = await this._postService.GetPostByUser(user);

            if(userPosts != null && userPosts.Any())
            {
                TempData["PostBlogue"] = userPosts.Select(p => p.Blogue.Title).First();
                TempData["UserPosts"] = JsonConvert.SerializeObject(userPosts);
            }
          
            return RedirectToAction("MyPosts", "User");
        }

        [HttpGet]
        [ActionName("CreatePost")]
        public async Task<IActionResult> Create()
        {
            var user = GetCurrentUser();

            if (user != null)
            {
                var userBlogue = await this._blogueService.GetAllBlogueByUser(user);
                ViewBag.UserBlogue = userBlogue;
                return View();
            }

            return RedirectToAction("Login", "Authentification");  
        }

        // POST : Post/Create/
        [HttpPost]
        [ActionName("CreatePost")]
        public async Task<IActionResult> Create(PostDTO newPostDTO)
        {
            var user = GetCurrentUser();

            if (!ModelState.IsValid)
            {
                var userBlogue = await this._blogueService.GetAllBlogueByUser(user);
                ViewBag.UserBlogue = userBlogue;
                return View(newPostDTO);
            }

            var newPost = new Post()
            {
                Title = newPostDTO.Title,
                Content = newPostDTO.Content,
                PublishingDate = DateTime.Now,
                MemberId = user.MemberId,
                BlogueId = newPostDTO.BlogueId
            };

            // Utilisation du service pour ajouter un post
            var createdPost = await _postService.AddPostAsync(newPost);

            if (createdPost != null)
            {
                createdPost.PostUrl = $"{Request.Scheme}://{Request.Host}{Url.Action("Details", "Post", new { id = createdPost.PostId })}";
                await this._postService.UpdatePostAsync(createdPost);
                TempData["PostMessage"] = "Le post a été ajouté avec succès!";
                return View(newPostDTO);
            }


            TempData["PostMessage"] = "Une erreur est survenu lors de la création du post! Réassayer plus tard";
            return View(newPostDTO);
        }

        // DELETE : Post/Delete/
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var loggedUser = GetCurrentUser();
           
            var postToDelete = await _postService.DeletePostAsync(id);

            if (postToDelete != null)
            {
                loggedUser.Posts.Remove(postToDelete);
                UpdateSessionData(loggedUser);
            }

            var posts = await this._postService.GetPostByUser(loggedUser);
            TempData["UserPosts"] = JsonConvert.SerializeObject(posts);
            return RedirectToAction("MyPosts", "User");
        }

        // PUT : Blogue/UpdatePost/{postId}
        [HttpPost]
        public async Task<IActionResult> Edit(PostDTO post)
        {
            var loggedUser = GetCurrentUser();

            if (!ModelState.IsValid)
            {
                var bloguePost = await this._blogueService.GetAllBlogueByUser(loggedUser);
                ViewBag.Blogue = bloguePost;
                return View(post);
            }

            var updatePost = await _postService.GetPostByIdAsync(post.Id);   

            if(updatePost != null)
            {
                updatePost.Title = post.Title;
                updatePost.Content = post.Content;
                updatePost.BlogueId = post.BlogueId;

                // Utilisation du service pour mettre à jour le post
                await _postService.UpdatePostAsync(updatePost);
                TempData["UpdatePostMessage"] = "Le post a été mis à jour avec succès!";
                return View(post);
            }

            TempData["UpdatePostMessage"] = "Une erreur s'est produite pour ajouter le post!";
            return View(post);
        }

        // GET : Post/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = GetCurrentUser();

            var postToEdit = await this._postService.GetPostByIdAsync(id);

            if(postToEdit != null)
            {
                var bloguePost = await this._blogueService.GetAllBlogueByUser(user);

                if(bloguePost != null)
                {
                    var postDTO = new PostDTO()
                    {
                        Id = postToEdit.PostId,
                        Title = postToEdit.Title,
                        Content = postToEdit.Content,
                        BlogueId = postToEdit.Blogue.BlogueId
                    };

                    ViewBag.Blogue = bloguePost;
                    return View(postDTO);
                }
            }

            return NotFound();
        }

        public Member GetCurrentUser()
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
