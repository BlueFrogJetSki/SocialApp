using SocialApp.Interfaces; // Importing the interface for likeable functionality
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    /// <summary>
    /// Represents a post within the application that users can interact with.
    /// Implements the ILikeable interface to support liking functionality.
    /// </summary>
    public class Post : ILikeable
    {
        // The Id property uniquely identifies a Post instance.
        // It is initialized with a new GUID to ensure uniqueness.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();


        public string Type { get; set; } = "Post";

        //Meta Info
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        public string? ImgURL { get; set; }

        public string? Description { get; set; }

        // The Hidden property indicates whether the post is visible to users.
        public bool Hidden { get; set; } = false;

        //Author Info
        [ForeignKey("AuthorProfile")]
        public string? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }

        // The Comments property holds a collection of comments associated with the post.
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        // From ILikeable
        public int LikesCount { get; set; } = 0;
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();
    }
}
