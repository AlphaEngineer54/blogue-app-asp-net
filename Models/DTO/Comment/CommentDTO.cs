using System.ComponentModel.DataAnnotations;

namespace Projet_RedSocial_app.Models.DTO.Comment
{
    public class CommentDTO
    {
        [Key]
        public int Id { get; set; }
         
        [Required(ErrorMessage = "Veuilez saisir votre commentaire")]
        [StringLength(
            300, 
            ErrorMessage = "Votre commentaire doit avoir une longeur minimale de 5 caractères et au maximum 300 caractères",
            MinimumLength = 5)]
        public string Content { get; set; }
        public int PostId { get; set; }
        public int MemberId { get; set; }
    }
}
