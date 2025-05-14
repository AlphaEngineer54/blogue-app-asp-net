using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_RedSocial_app.Models.DTO.Post
{
    public class PostDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Veuillez saisir un titre pour le post")]
        [StringLength(100, ErrorMessage = "Le titre du post ne doit pas dépasser les 100 caractères!")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez écrire votre texte pour votre post")]
        [StringLength(1000, ErrorMessage = "Le contenue ne doit pas dépasser les 1000 caractères!")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez sélectionner un blogue")]
        public int BlogueId { get; set; }
    }
}
