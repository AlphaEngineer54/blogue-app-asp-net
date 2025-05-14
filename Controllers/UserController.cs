using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DTO.Member;
using Projet_RedSocial_app.Services;

namespace Projet_RedSocial_app.Controllers
{
    /// <summary>
    /// Gère l'affichage des vues pour permettre aux utilsateurs de gérer leur compte et d'effectuer des oppérations sur leur donnnéess
    /// </summary>
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        [ActionName(name:"MyBlogues")]
        public IActionResult GetUserBlogues()
        {
            string blogues = TempData["UserBlogues"] as string;

            if(blogues != null)
            {
                var deserializedBlogues = JsonConvert.DeserializeObject<IList<Blogue>>(blogues);
                return View(deserializedBlogues);
            }

            return View();
        }

        [HttpGet]
        [ActionName(name:"MyPosts")]
        public IActionResult GetUserPost()
        {
            string posts = TempData["UserPosts"] as string;
            string blogue = TempData["PostBlogue"] as string;

            ViewBag.LoggedUser = GetLoggedUser();

            if (posts != null && blogue != null)
            {
                var deserializedPosts = JsonConvert.DeserializeObject<IList<Post>>(posts);
                ViewBag.BlogueTitle = blogue;
                return View(deserializedPosts);
            }

            return View();
        }

        [HttpGet]
        [ActionName(name:"MyProfile")]
        public IActionResult GetUserInfos()
        {
            var user = GetLoggedUser(); 

           // ViewBag.LoggedUser = user;
            return View(user);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var user = GetLoggedUser();

            var memberDTO = new MemberSignInDTO()
            {
                MemberId = user.MemberId,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password
            };
            
            return View(memberDTO);
        }

        /// <summary>
        /// Modifie l'utilisateur connecté avec de nouvelles informations
        /// </summary>
        /// <param name="memberEditDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(MemberSignInDTO memberEditDTO)
        {
            var user = GetLoggedUser();

            if (ModelState.IsValid)
            {
                var updatedMember = await this._userService.UpdateMemberAsync(user.MemberId, memberEditDTO);

                if (updatedMember != null)
                {
                    HttpContext.Session.SetString("LoggedUser", JsonConvert.SerializeObject(updatedMember));
                    return View("MyProfile", updatedMember);
                }
                else
                {
                    ModelState.AddModelError("", "Impossible de modifier vos informations de compte.");
                }
               
            }

            return View(memberEditDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            var member = GetLoggedUser();

           var isDeleted = await this._userService.DeleteMemberAsync(member.MemberId);

           if(isDeleted)
           {
               HttpContext.Session.Clear();
               return RedirectToAction("Login", "Authentification");
           }

            return View("MyProfile", member);
        }

        /// <summary>
        /// Méthode d'assistance pour récupérer l'utilisateur connecté à partir de la session.
        /// </summary>
        private Member GetLoggedUser()
        {
            var user = HttpContext.Session.GetString("LoggedUser");
            return JsonConvert.DeserializeObject<Member>(user);
        }
    }
}
