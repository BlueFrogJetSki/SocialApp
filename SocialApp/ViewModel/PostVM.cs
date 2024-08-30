using SocialApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.ViewModel
{
    public class PostVM
    {
        [Key]
        public string Id { get; set; }

        public IFormFile Img { get; set; }

        public string Description { get; set; }

        
        public string? AuthorProfileId { get; set; }
       
    }
}
