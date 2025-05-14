using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Projet_RedSocial_app.FilterActions
{
    public class UserConnectionFilterAction : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userJson = context.HttpContext.Session.GetString("LoggedUser");
            var controller = (string)context.RouteData.Values["controller"];
         
            // Si l'utilisateur n'est pas connecté et n'est pas déjà sur la page de connexion
            if (userJson == null && controller != "Authentification")
            {
                context.Result = new RedirectToActionResult("Login", "Authentification", null);
            }
        }
    }
}
