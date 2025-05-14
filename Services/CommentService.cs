using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Models;
using Microsoft.EntityFrameworkCore;

namespace Projet_RedSocial_app.Services
{
    public class CommentService
    {
        private readonly BloguedbContext _blogueDbContext;

        public CommentService(BloguedbContext blogueDbContext)
        {
            _blogueDbContext = blogueDbContext;
        }

        public async Task<Comment> Add(Comment newComment)
        {
            var newCommentResult = await this._blogueDbContext.Comments.AddAsync(newComment);    
            
            if(newCommentResult != null)
            {
                await this._blogueDbContext.SaveChangesAsync();
                return newCommentResult.Entity;
            }

            return null;
        }

        public async Task<Comment> Update(Comment updateComment)
        {
            var foundComment = await this._blogueDbContext.Comments
                                         .FirstOrDefaultAsync(c => c.CommentId == updateComment.CommentId);

            if(foundComment != null)
            {
                foundComment.Content = updateComment.Content;
                await this._blogueDbContext.SaveChangesAsync();
                return foundComment;
            }
        
            return null;
        }

        public async Task<Comment> Delete(int id)
        {
            var commentToDelete = await this._blogueDbContext.Comments
                                          .FirstOrDefaultAsync(c => c.CommentId == id); 

            if(commentToDelete != null)
            {
                var deleteComment = _blogueDbContext.Remove(commentToDelete);
                await _blogueDbContext.SaveChangesAsync();
                return deleteComment.Entity;   
            }

            return null;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentByPost(int postId)
        {
            var comments = await this._blogueDbContext.Comments
                                    .Where(c => c.PostId == postId)
                                    .ToListAsync();
            if(comments != null)
            {
                return comments;
            }

            return null;
        }
    }
}
