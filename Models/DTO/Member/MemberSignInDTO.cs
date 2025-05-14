using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Projet_RedSocial_app.Models.DTO.Member
{
    public class MemberSignInDTO
    {
        [Key]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Veuillez saisir votre nom d'utilisateur")]
        [StringLength(
            50,
            ErrorMessage = "Le nom d'utilisateur doit posséder au minimum 5 caractères et 50 caractères maximum",
            MinimumLength = 5)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Veuillez saisir une adresse email valide avec un domaine valide (ex : gmail.com) ")]
        [EmailAddress(ErrorMessage = "Email invalide. Veuillez saisir une adresse email avec un nom de domaine valide suivi du symbole '@'")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Veuillez saisir un mot de passe")]
        [StringLength(
            100, 
            ErrorMessage = "Le mot de passe doit contenir 5 caractères minimum et au maximum 100 caractères.", 
            MinimumLength = 5)]
        public string? Password { get; set; }
    }
}
