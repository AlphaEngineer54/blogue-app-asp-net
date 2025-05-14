using Microsoft.EntityFrameworkCore;
using Projet_RedSocial_app.Models.DBContext;
using System.ComponentModel.DataAnnotations;
using Projet_RedSocial_app.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.Xml;
using Projet_RedSocial_app.Models.DTO.Member;

namespace Projet_RedSocial_app.Services
{
    public class AuthentificationService
    {
        private readonly BloguedbContext _dbContext;
        private readonly IPasswordHasher<Member> _passwordHasher;

         public AuthentificationService(BloguedbContext dbContext, IPasswordHasher<Member> hasher) 
         {
           this._dbContext = dbContext;
           this._passwordHasher = hasher;
         }

        public async Task<Member?> Login(MemberLoginDTO loginDTO)
        {
            var loggedUser = await this._dbContext.Members.FirstOrDefaultAsync(m => m.Email == loginDTO.Email);
            PasswordVerificationResult result = 0;

            if (loggedUser != null)
            {
                result = _passwordHasher.VerifyHashedPassword(loggedUser, loggedUser.Password, loginDTO.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    loggedUser.IsOnline = true;
                    await this._dbContext.SaveChangesAsync();
                    return await this._dbContext.Members
                                    .Include(m => m.Blogues)
                                    .Include(m => m.Comments)
                                    .Include(m => m.Posts)
                                    .FirstOrDefaultAsync(m => m.MemberId == loggedUser.MemberId);
                }
            }
    
            return null;
        }

        public async Task<Member> SignIn(MemberSignInDTO siginInDTO)
        {
            var existingMember = await this._dbContext.Members.FirstOrDefaultAsync(m => m.Email == siginInDTO.Email);

            if(existingMember != null)
            {
                // Return null if a user try to sign-in with a existing email in the DB
                return null;
            }

            var newMember = new Member()
            {
                UserName = siginInDTO.UserName,
                Email = siginInDTO.Email,
                Password = HashPassword(siginInDTO.Password),
                IsOnline = true,
                CreationDate = DateTime.Now
            };

            await this._dbContext.Members.AddAsync(newMember);
            await this._dbContext.SaveChangesAsync();

            return newMember;
        }

        public async Task<bool> Logout(int id)
        {
            var foundUser = await this._dbContext.Members.FirstOrDefaultAsync(m => m.MemberId== id);

            if(foundUser == null)
            {
                return false;
            }

            foundUser.IsOnline= false;
            await this._dbContext.SaveChangesAsync();
            return true;
        }

        public string HashPassword(string password)
        {
            var encryptePassword = this._passwordHasher.HashPassword(new Member(), password);

            return encryptePassword;
        }
    }
}
