using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class CreateCommentDTO
    {
        [Required(ErrorMessage = "Comment must contain at least 1 character")]
        public string Text { get; set; }
    }
}
