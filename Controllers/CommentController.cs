using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DTO.Comment;
using Projet_RedSocial_app.Services;

namespace Projet_RedSocial_app.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentDTO newCommentDTO)
        {
           if(!ModelState.IsValid)
           {
              TempData["Error"] = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .FirstOrDefault();

              return RedirectToAction("Details", "Post", new { id = newCommentDTO.PostId });
           }

            var newComment = new Comment()
            {
                Content = newCommentDTO.Content,
                PublishingDate = DateTime.Now,
                MemberId = newCommentDTO.MemberId,
                PostId = newCommentDTO.PostId,  
            };

            await this._commentService.Add(newComment);
            return RedirectToAction("Details", "Post", new { id = newCommentDTO.PostId });
        }


        [HttpPost]
        public async Task<IActionResult> Update(CommentDTO updateCommentDTO)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .FirstOrDefault();

                return RedirectToAction("Details", "Post", new { id = updateCommentDTO.PostId });
            }

            var updatedComment = new Comment()
            {
                CommentId = updateCommentDTO.Id,
                Content = updateCommentDTO.Content
            };

            var commentResult = await this._commentService.Update(updatedComment);
            return RedirectToAction("Details", "Post", new { id = commentResult.PostId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteComment = await this._commentService.Delete(id);

            if (deleteComment != null)
            {
                return RedirectToAction("Details", "Post", new { id = deleteComment.PostId });
            }

            TempData["Error"] = $"Une erreur est survenu pour supprimer le commentaire #{deleteComment.CommentId}";
            return RedirectToAction("Details", "Post", new { id = deleteComment.PostId });
        }
    }
}
