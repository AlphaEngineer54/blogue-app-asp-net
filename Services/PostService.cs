using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Models;
using Microsoft.EntityFrameworkCore;


namespace Projet_RedSocial_app.Services
{
    public class PostService
    {
        private readonly BloguedbContext _dbContext;

        public PostService(BloguedbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // Recherche des posts par titre
        public async Task<IList<Post>> GetPostsByTitleAsync(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var formatedQuery = query.Trim().ToLower();
                return await _dbContext.Posts
                        .Where(p => p.Title
                                     .Trim()
                                     .ToLower()
                                     .Contains(formatedQuery))
                        .Include(p => p.Member)
                        .Include(p => p.PostLikes)
                        .Include(p => p.PostDislikes)
                        .Include(p => p.Comments)
                        .ToListAsync();
            }

            return null;
        }

        // Recherche un post par ID
        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            return await _dbContext.Posts
                        .Include(p => p.Member)
                        .Include(p => p.Blogue)
                        .Include(p => p.PostLikes)
                        .Include(p => p.PostDislikes)
                        .Include(p => p.Comments)
                           .ThenInclude(c => c.Member)
                        .FirstOrDefaultAsync(p => p.PostId == postId);
        }

        public async Task<IEnumerable<Post>> GetPostByUser(Member user)
        {
            return await this._dbContext.Posts
                             .Include(p => p.Member)
                             .Include (p => p.Blogue)
                             .Include(p => p.PostLikes)
                             .Include(p => p.PostDislikes)
                             .Where(p => p.MemberId == user.MemberId)
                             .OrderBy(p => p.PublishingDate)
                             .ToListAsync();
        }

        // Ajout d'un nouveau post
        public async Task<Post> AddPostAsync(Post newPost)
        {
            if (newPost != null) 
            {
                var post = _dbContext.Posts.Add(newPost).Entity;
                await _dbContext.SaveChangesAsync();
                return post;
            }

            return null;
        }

        // Suppression d'un post par ID
        public async Task<Post> DeletePostAsync(int postId)
        {
            var postToDelete = await _dbContext.Posts
                                      .Include (p => p.PostLikes)
                                      .Include(p => p.PostDislikes)
                                      .Include(p => p.Comments)
                                      .FirstOrDefaultAsync(p => p.PostId == postId);

            if (postToDelete != null)
            {
                _dbContext.Posts.Remove(postToDelete);
                await _dbContext.SaveChangesAsync();

                return postToDelete;
            }

            return null;
        }


        // Mise à jour d'un post
        public async Task<Post> UpdatePostAsync(Post updatedPost)
        {
            if (updatedPost != null) 
            {
                var updatedPostResult = _dbContext.Posts.Update(updatedPost).Entity;
                await _dbContext.SaveChangesAsync();
                return updatedPostResult;
            }

            return null;
        }
    }
}
