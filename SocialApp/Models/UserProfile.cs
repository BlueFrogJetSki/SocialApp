using SocialApp.Data.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string? UserName { get; set; }

        [StringLength(150, ErrorMessage = "Biography cannot be longer than 150 characters.")]
        public string? Biography { get; set; }

        public string? IconURL { get; set; }


        //public Major? Major { get; set; }
        //public IList<Interest>? Interests { get; set; } = new List<Interest>();

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public IEnumerable<Post> Posts { get; set; } = new HashSet<Post>();

        public ICollection<Story> Stories { get; set; } = new HashSet<Story>();

    }
}
