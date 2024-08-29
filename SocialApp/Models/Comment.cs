using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int LikesCount { get; set; } = 0;

        [Required(ErrorMessage ="Comment must contain at least 1 character")]
        public string? Text { get; set; }

        [ForeignKey("Post")]
        public string? PostId { get; set; }
        public Post? Post { get; set; }


        [ForeignKey("AuthorProfile")]
        public int? AuthorProfileId { get; set; }
        public UserProfile? AuthorProflie { get; set; }
        public string ? AuthorName { get; set; }

        public ICollection<Comment> SubComments { get; set; } = new List<Comment>();
    }
}
