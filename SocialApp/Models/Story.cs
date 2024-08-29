using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Story
    {
        //create new unique id on creation
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }
        public bool IsHidden { get; set; } = false;
        public string? ImgURL { get; set; }
        public int LikesCount { get; set; } = 0;

        [ForeignKey("AuthorProfile")]
        public int? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }

        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();

    }
}
