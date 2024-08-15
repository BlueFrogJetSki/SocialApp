using SocialApp.Data.Enum;
using SocialApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.ViewModel
{
    public class CreateUserProfileVM
    {
       
        public string Name { get; set; }
        public string Biography { get; set; }
        public Major? Major { get; set; }
        public Interest[]? Interests { get; set; }

        public string AppUserId {  get; set; }

        
    }
}
