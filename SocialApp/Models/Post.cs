using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Post
    {
        //create new unique id on creation
        [Key]  
        public string Id { get; set; }

        public string? ImgURL { get; set; }

        public string? Description { get; set; }

        public int LikesCount { get; set; } = 0;

        public bool Hidden { get; set; } = false;

        [ForeignKey("AuthorProfile")]
        public int? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }

        public IEnumerable<UserProfile> LikedUsers { get; set; } = new HashSet<UserProfile>();

        public IEnumerable<Comment> Comments { get; set; } = new HashSet<Comment>();

    }
}
