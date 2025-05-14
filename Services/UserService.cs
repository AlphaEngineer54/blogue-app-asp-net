using Projet_RedSocial_app.Models;
using Projet_RedSocial_app.Models.DBContext;
using Projet_RedSocial_app.Models.DTO.Member;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Projet_RedSocial_app.Services
{
    public class UserService
    {
        private readonly BloguedbContext _blogueContext;
        private readonly IPasswordHasher<Member> _passwordHasher;

        public UserService(BloguedbContext context, IPasswordHasher<Member> passwordHasher)
        {
            _blogueContext = context;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Permet à un utlisateur de modifier ses informations de son compte (Email, Password, Username)
        /// </summary>
        /// <param name="updatedMember"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Member> UpdateMemberAsync(int id, MemberSignInDTO updatedMember)
        {
            var member = await _blogueContext.Members
                              .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return null;
            }

            member.UserName = updatedMember.UserName ?? member.UserName;
            member.Email = updatedMember.Email ?? member.Email;
            member.Password = this._passwordHasher.HashPassword(member, updatedMember.Password) ?? member.Password;

            await _blogueContext.SaveChangesAsync();

            return member;
        }

        /// <summary>
        /// Permet à un utilisateur de supprimer son compte via son identifiant unique
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await _blogueContext.Members
                             .Include(m => m.Blogues)
                             .Include(m => m.Posts)
                             .Include(m => m.PostLikes)
                             .Include(m => m.PostDislikes)
                             .Include(m => m.Comments)
                             .Include(m => m.CommentDislikes)
                             .Include(m => m.CommentLikes)
                             .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return false;
            }

            _blogueContext.Members.Remove(member);
            await _blogueContext.SaveChangesAsync();

            return true;
        }
    }

}
