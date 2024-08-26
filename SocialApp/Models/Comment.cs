using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int LikesCount { get; set; } = 0;

        [ForeignKey("Post")]
        public string? PostId { get; set; }
        public Post? Post { get; set; }


        [ForeignKey("AuthorProfileId")]
        public int? AuthorProfileId { get; set; }
        public UserProfile? AuthorProflie { get; set; }

        public IEnumerable<Comment> SubComments { get; set; } = new List<Comment>();
    }
}
