using Microsoft.EntityFrameworkCore;
using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DBContext;

namespace Projet_RedSocial_app.Services
{
    public class VotesServices
    {
        private readonly BloguedbContext _bloguedb;

        public VotesServices(BloguedbContext bloguedb)
        {
            this._bloguedb = bloguedb;
        }

        public async Task AddLike(int postId, int memberId)
        {
            var userDislike = await this.GetPostDisLikeByUser(postId, memberId);

            var userLike = await this.GetPostLikeByUser(postId, memberId);

            // S'il y a un dislike, supprimer le avant d'ajouter un nouveau like
            if (userDislike != null)
            {
                this._bloguedb.PostDislikes.Remove(userDislike);
            }

            // S'il y a un like, supprimer le like 
            if (userLike != null)
            {
                this._bloguedb.PostLikes.Remove(userLike);
            }
            else
            {
                // Créer le nouveau like associé à l'utilsateur courant
                var newLike = new PostLike
                {
                    PostId = postId,
                    MemberId = memberId
                };

                // Enregistrer le like dans la base de données 
                await this._bloguedb.PostLikes.AddAsync(newLike);
            }

            await this._bloguedb.SaveChangesAsync();
           // Console.WriteLine($"Like saved for MemberID: {memberId} in PostID: {postId}");
        }

        public async Task AddDislike(int postId, int memberId)
        {
            var userDislike = await this.GetPostDisLikeByUser(postId, memberId);

            var userLike = await this.GetPostLikeByUser(postId, memberId);

            // S'il y a un like, supprimer le avant d'ajouter un nouveau like
            if (userLike != null)
            {
                this._bloguedb.PostLikes.Remove(userLike);
            }

            // S'il y a un dislike, supprimer-le 
            if (userDislike != null)
            {
                this._bloguedb.PostDislikes.Remove(userDislike);
            }
            else
            {
                // Créer le nouveau like associé à l'utilsateur courant
                var newDislike = new PostDislike
                {
                    PostId = postId,
                    MemberId = memberId
                };

                // Enregistrer le like dans la base de données 
                await this._bloguedb.PostDislikes.AddAsync(newDislike);
            }

            await this._bloguedb.SaveChangesAsync();
          //  Console.WriteLine($"Dislike saved for MemberID: {memberId} in PostID: {postId}");
        }

        private async Task<PostDislike?> GetPostDisLikeByUser(int postId, int memberId)
        {
            return await this._bloguedb.PostDislikes
                                 .Where(p => p.PostId == postId)
                                 .Where(p => p.MemberId == memberId)
                                 .FirstOrDefaultAsync();
        }

        private async Task<PostLike?> GetPostLikeByUser(int postId, int memberId)
        {
            return await this._bloguedb.PostLikes
                                 .Where(p => p.PostId == postId)
                                 .Where(p => p.MemberId == memberId)
                                 .FirstOrDefaultAsync();
        }
    }
}
