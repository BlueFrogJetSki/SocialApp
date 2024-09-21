using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    [PrimaryKey(nameof(FollowerId), nameof(FolloweeId))]
    public class Follow
    { 

        [ForeignKey("Follower")]
        public string? FollowerId {  get; set; }

        public UserProfile? Follower {  get; set; }

        [ForeignKey("Followee")]
        public string? FolloweeId { get; set; }

        public UserProfile? Followee { get; set; }
    }
}
