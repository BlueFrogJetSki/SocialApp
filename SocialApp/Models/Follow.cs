using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Follow
    {
        [Key]
        public string? Id {  get; set; }

        [ForeignKey("Follower")]
        public string? FollowerId {  get; set; }

        public UserProfile? Follower {  get; set; }

        [ForeignKey("Followee")]
        public string? FolloweeId { get; set; }

        public UserProfile? Followee { get; set; }
    }
}
