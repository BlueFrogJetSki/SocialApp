using SocialApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.ViewModel
{
    public class PostVM
    {
        [Key]
        public string Id { get; set; }

        public IFormFile ImgURL { get; set; }

        public string Description { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
       
    }
}
