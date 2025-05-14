using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projet_RedSocial_app.Services;

namespace Projet_RedSocial_app.Controllers
{
    public class VotesController : Controller
    {
        private readonly VotesServices _votesService;

       public VotesController(VotesServices voteService)
        {
            _votesService = voteService;
        }

       [HttpPost]
       public async Task<IActionResult> AddLike(int postId, int memberId)
       {
            await _votesService.AddLike(postId, memberId);
            return RedirectToAction("Details", "Post", new { id = postId });
       }

       [HttpPost]
       public async Task<IActionResult> AddDislike(int postId, int memberId)
       {
            await _votesService.AddDislike(postId, memberId);
            return RedirectToAction("Details", "Post", new { id = postId});
       }
    }
}
