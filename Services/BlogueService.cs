using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Models;
using Microsoft.EntityFrameworkCore;
using Projet_RedSocial_app.Models.DTO.Blogue;

namespace Projet_RedSocial_app.Services
{

    public class BlogueService
    {
        private readonly BloguedbContext _context;

        // Injection du BloguedbContext via le constructeur
        public BlogueService(BloguedbContext context)
        {
            _context = context;
        }


        public async Task<Blogue?> GetBlogueByTitleAsync(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                return await _context.Blogues
                        .Where(p => p.Title
                                     .Contains(query))
                        .Include(p => p.Member)
                        .Include(p => p.Posts)
                        .FirstOrDefaultAsync();
            }

            return null;
        }
        // Méthode pour récupérer un Blogue par son ID
        public async Task<Blogue?> GetBlogueByIdAsync(int blogueId)
        {
            return await _context.Blogues
                                 .Include(b => b.Posts) // Inclure les Posts associés
                                 .FirstOrDefaultAsync(b => b.BlogueId == blogueId);
        }

        // Méthode pour récupérer tous les Blogues
        public async Task<IList<Blogue>> GetAllBloguesAsync()
        {
            return await _context.Blogues
                                 .Include(b => b.Posts) // Inclure les Posts associés
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Blogue>> GetAllBlogueByUser(Member member)
        {
            return await _context.Blogues
                            .Where(b => b.Member == member)
                            .OrderBy(b => b.CreationDate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Blogue>> GetBlogueByCategorie(string categorie)
        {
            return await _context.Blogues
                    .Where(b => b.Categorie == categorie)
                    .Include(b => b.Posts)
                    .Take(10)  // Ajouter une limite de 10 pour optimiser les performances
                    .ToListAsync();
        }

        // Méthode pour créer un nouveau Blogue
        public async Task<Blogue> CreateBlogueAsync(BlogueDTO blogueDTO, Member owner)
        {
            var newBlogue = new Blogue()
            {
                Title = blogueDTO.Title,
                Description = blogueDTO.Description,
                Categorie = blogueDTO.Categorie,
                CreationDate = DateTime.Now,
                MemberId = owner.MemberId,
            };

            await _context.Blogues.AddAsync(newBlogue);
            await _context.SaveChangesAsync();

            return newBlogue;
        }

        // Méthode pour mettre à jour un Blogue existant
        public async Task<Blogue> UpdateBlogueAsync(BlogueDTO blogue)
        {
           var blogueToUpdate = await this.GetBlogueByIdAsync(blogue.Id);

            if (blogueToUpdate != null)
            {
                blogueToUpdate.Title = blogue.Title;
                blogueToUpdate.Description = blogue.Description;
                blogueToUpdate.Categorie = blogue.Categorie;
               
                _context.Blogues.Update(blogueToUpdate);
                await _context.SaveChangesAsync();

                return blogueToUpdate;  
            }

            return null;
        }

        // Méthode pour supprimer un Blogue
        public async Task<Blogue> DeleteBlogueAsync(int blogueId)
        {
            var blogue = await GetBlogueByIdAsync(blogueId);
            if (blogue != null)
            {
                _context.Blogues.Remove(blogue);
                await _context.SaveChangesAsync();
                return blogue;
            }

            return null;
        }
    }
}
