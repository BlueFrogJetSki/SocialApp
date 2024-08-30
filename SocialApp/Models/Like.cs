using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    /// <summary>
    /// Represents a "like" action by a user on a post within the application.
    /// </summary>
    public class Like
    {
        // TODO: Decide how to implement likes for different objects
        // The Id property uniquely identifies a Like instance.
        // It is initialized with a new GUID to ensure uniqueness.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // The DateTime property records when the like was created.
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        // The AuthorProfileId property links to the user profile who made the like.
        // It is optional because a like might not always be associated with a user profile.
        [ForeignKey("AuthorProfile")]
        public string? AuthorProfileId { get; set; }
        // Navigation property to the AuthorProfile.
        public UserProfile? AuthorProfile { get; set; }

        // The EntityType property indicates the type of entity being liked (e.g., Post, Comment, Story).
        [Required]
        public string? EntityType { get; set; }

        // The EntityId property stores the identifier of the entity being liked.
        [Required]
        public string? EntityId { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj is Like otherLike)
            {
                return AuthorProfileId == otherLike.AuthorProfileId && EntityId == otherLike.EntityId && EntityType == otherLike.EntityType;
            }
            return false;
        }

      
        public override int GetHashCode()
        {
            // Use a combination of AuthorProfileId and PostId for the hash code
            // We use a prime number and the XOR (^) operator to combine hash codes.
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (AuthorProfileId?.GetHashCode() ?? 0);
                hash = hash * 31 + (EntityId?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
