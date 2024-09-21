using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    [Index(nameof(UserName), IsUnique =true)]
    public class UserProfile
    {
        // It is initialized with a new GUID to ensure uniqueness.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        //Basic info

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string? UserName { get; set; }

        [StringLength(150, ErrorMessage = "Biography cannot be longer than 150 characters.")]
        public string? Biography { get; set; }

        //User Profile Picture

        public string? IconURL { get; set; }

        //The AppUser this profile belongs to

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        //Posts created by this user
        public ICollection<Post>? Posts { get; set; } = new HashSet<Post>();

        //Following relationships with other userprofiles
        public ICollection<Follow>? Following { get; set; } = new HashSet<Follow>();
        public ICollection<Follow>? Followers { get; set; } = new HashSet<Follow>();

        //Likes made by this user
        public ICollection<Like>? Likes { get; set; } = new HashSet<Like>();

        public ICollection<Story>? Stories { get; set; } = new HashSet<Story>();

    }
}
