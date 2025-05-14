using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Projet_RedSocial_app.Models.DTO.Member
{
    public class MemberLoginDTO
    {
     
        [Required(ErrorMessage = "Veuillez saisir votre adresse email valide")]
        [EmailAddress(ErrorMessage = "Adresse email invalide. Veuillez saisir un email avec un nom de domaine valide suivi du symbole '@'")]
        public string? Email {  get; set; }

        [Required(ErrorMessage = "Veuillez entrez votre mot de passe")]
        [StringLength(
            100, 
            ErrorMessage = "Le mot de passe est trop long!")]
        public string? Password { get; set; }   
    }
}
