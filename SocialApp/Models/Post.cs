using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Post
    {
        //create new unique id on creation
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? ImgURL { get; set; }

        public string? Description { get; set; }

        public int LikesCount { get; set; } = 0;

        public bool Hidden { get; set; } = false;

        [ForeignKey("AuthorProfile")]
        public int? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }

        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

    }
}
