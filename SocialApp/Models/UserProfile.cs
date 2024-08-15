using SocialApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public Major? Major { get; set; }
        public Interest[]? Interests { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId {  get; set; }
        public AppUser AppUser {  get; set; }
    }
}
