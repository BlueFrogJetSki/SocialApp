using Microsoft.AspNetCore.Http.HttpResults;
using SocialApp.Models;

namespace SocialApp.DataTransferObject
{
    public class LikeDTO
    {
        public SimpleProfileDTO AuthorDTO { get; set; }

        public LikeDTO() { }
        public LikeDTO(Like like)
        {
           
            AuthorDTO = new SimpleProfileDTO(like.AuthorProfile);
            
        }

    }
}
