using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Like
    {
        //TODO: Decide how to implement likes for different objects
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [ForeignKey("AuthorProfile")]
        public int? AuthorProfileId { get; set; }
        public UserProfile? AuthorProfile { get; set; }

        [ForeignKey("LikedPost")]
        public string? PostId { get; set; }

        public Post? Post { get; set; }

        // Override Equals to compare two Like objects based on AuthorProfileId and PostId
        public override bool Equals(object? obj)
        {
            if (obj is Like otherLike)
            {
                return AuthorProfileId == otherLike.AuthorProfileId && PostId == otherLike.PostId;
            }
            return false;
        }

        // Override GetHashCode to ensure the hash code is based on AuthorProfileId and PostId
        public override int GetHashCode()
        {
            // Use a combination of AuthorProfileId and PostId for the hash code
            // We use a prime number and the XOR (^) operator to combine hash codes
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (AuthorProfileId?.GetHashCode() ?? 0);
                hash = hash * 31 + (PostId?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
