using System.ComponentModel.DataAnnotations;

namespace Projet_RedSocial_app.Models.DTO.Blogue
{
    public class BlogueDTO
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Veuillez saisir un titre")]
        [StringLength(50, ErrorMessage = "Le titre du blogue doit contenir 10 caractères minimum et maximum 50 caractères", MinimumLength = 10)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Veuillez saisir une description pour ce blogue")]
        [StringLength(300, ErrorMessage = "La description doit contenir au minimum 30 caractères et au maximum 300 caractères", MinimumLength = 30)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner une catégorie")]
        public string Categorie { get; set; }
    }
}
