using SocialApp.Data.Enum;
using SocialApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialApp.ViewModel
{
    public class UserProfileVM 
    {
        public string Id { get; set; }
       
        public string? UserName { get; set; }
        public string? Biography { get; set; }
        //public Major? Major { get; set; }
        //public IList<Interest>? Interests { get; set; }

        public IFormFile? IconImg { get; set; }
        public string? IconURL { get; set; }


        public UserProfileVM(UserProfile userProfile)
        {
            Id = userProfile.Id;
            UserName = userProfile.UserName;
            Biography = userProfile.Biography;
            IconURL = userProfile.IconURL;
            
        }

        public UserProfileVM() { }


        
    }
}
