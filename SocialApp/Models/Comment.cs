using SocialApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Comment:ILikeable
    {
        // It is initialized with a new GUID to ensure uniqueness.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Type { get; set; } = "Comment";

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        public int LikesCount { get; set; } = 0;

        [Required(ErrorMessage ="Comment must contain at least 1 character")]
        public string? Text { get; set; }

        [ForeignKey("Post")]
        public string? PostId { get; set; }
        public Post? Post { get; set; }


        [ForeignKey("AuthorProfile")]
        public string? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }
        public string ? AuthorName { get; set; }
        public ICollection<Like>? Likes { get; set; } = new HashSet<Like>();
        public ICollection<Comment>? SubComments { get; set; } = new List<Comment>();
    }
}
