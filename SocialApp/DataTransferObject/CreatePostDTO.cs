using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class CreatePostDTO
    {
        [Required]
        public string? Description { get; set; }
        
    }
}
