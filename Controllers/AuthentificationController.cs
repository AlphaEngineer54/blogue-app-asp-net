using Microsoft.AspNetCore.Mvc;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Services;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Projet_RedSocial_app.Models.DTO.Member;
using Newtonsoft.Json;

namespace Projet_RedSocial_app.Controllers
{
    public class AuthentificationController : Controller
    {
       private readonly AuthentificationService _authentificationService;

       public AuthentificationController(AuthentificationService authentification) 
       {
          ViewBag.Error = string.Empty;
          this._authentificationService = authentification;
       }
        
        // Generate the login view
       [HttpGet]
       public IActionResult Login()
       {
           // Redirect user to the authentification if the user is offline
           return View();
       }

        // Process the login for the current user
       [HttpPost]
       public async Task<IActionResult> Login(MemberLoginDTO memberLoginDTO)
       {
            if(!ModelState.IsValid)
            {
               return View("Login", memberLoginDTO);
            }

            var loggedUser = await this._authentificationService.Login(memberLoginDTO);

            if (loggedUser == null)
            {
                // Display An error if login failed
                ViewBag.Error = "La connexion a échouée. Vérifier vos informations de connexion";
                return View("Login");
            }

            HttpContext.Session.SetString("LoggedUser", JsonConvert.SerializeObject(loggedUser));
            // Redirect the user to home page 
            return RedirectToAction("Blogue", "Home");
        
       }

        // Generate the sign-in form
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // Prcess the sign-in 
        [HttpPost]
        public async Task<IActionResult> SignIn(MemberSignInDTO memberSignInDTO)
        {
            if (!ModelState.IsValid)
            {
                return View("SignIn", memberSignInDTO);
            }

            var newMember = await this._authentificationService.SignIn(memberSignInDTO);

            if(newMember != null)
            {
                HttpContext.Session.SetString("LoggedUser", JsonConvert.SerializeObject(newMember));
     
                // Redirect the user to the home page of the application
                return RedirectToAction("Blogue", "Home");
            }

            ViewBag.Error = "La tentative de création du compte a échouée. Veuillez réassayer";
            return View("SignIn");
        }

        // Logout the user with his ID
        [HttpGet]
        public async Task<IActionResult> Logout(int id)
        {
            var logoutUser = await this._authentificationService.Logout(id);

            if(logoutUser)
            { 
                HttpContext.Session.Clear();  // Supprimer la session de connexion lorsque l'utilisateur se déconnecte   
                return RedirectToAction("Login");
            }

            // Si une erreur lors de la déconnexion, rester dans la page principale
            return View("Blogue", "Home");
        }
    }
}