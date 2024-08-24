using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.Models
{
    public class Post
    {
        //create new unique id on creation
        [Key]  
        public string? Id { get; set; } = Guid.NewGuid().ToString();

        public string? ImgURL { get; set; }

        public string? Description { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
