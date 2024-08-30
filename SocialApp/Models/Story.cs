using SocialApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Story:ILikeable
    {
        //create new unique id on creation
        // It is initialized with a new GUID to ensure uniqueness.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }
        public bool IsHidden { get; set; } = false;
        public string? ImgURL { get; set; }
        public int LikesCount { get; set; } = 0;

        [ForeignKey("AuthorProfile")]
        public string? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }

        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();

    }
}
