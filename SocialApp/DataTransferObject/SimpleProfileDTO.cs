using SocialApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.DataTransferObject
{
    public class SimpleProfileDTO
    {

        //Basic info
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string? UserName { get; set; }

        [StringLength(150, ErrorMessage = "Biography cannot be longer than 150 characters.")]
        public string? Biography { get; set; }

        //User Profile Picture

        public string? IconURL { get; set; }

        public SimpleProfileDTO() { }
        public SimpleProfileDTO(UserProfile profile)
        {
            UserName = profile?.UserName;
            IconURL = profile?.IconURL;
        }

       

    }
}

