using SocialApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class ProfileDetailsDTO
    {
        //Basic info
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
        public string? UserName { get; set; }

        [StringLength(150, ErrorMessage = "Biography cannot be longer than 150 characters.")]
        public string? Biography { get; set; }

        //User Profile Picture

        public string? IconURL { get; set; }

        //Posts created by this user
        public ICollection<PostDTO>? PostDTOs { get; set; }

        //Following relationships with other userprofiles
        public ICollection<SimpleProfileDTO>? Following { get; set; }
        public ICollection<SimpleProfileDTO>? Followers { get; set; }

        public ProfileDetailsDTO() { }

        //turn all posts to postDTO (optimize with pagination)
        public ProfileDetailsDTO(UserProfile profile)
        {
            UserName = profile?.UserName;
            Biography = profile?.Biography;
            IconURL = profile?.IconURL;
            PostDTOs = serializePosts(profile.Posts);
            Following = serializeFollowing(profile.Following);
            Followers = serializeFollower(profile.Followers);

        }


        private ICollection<PostDTO> serializePosts(ICollection<Post> posts)
        {
            List<PostDTO> postDTOs = new List<PostDTO>();
            foreach (var post in posts)
            {
                postDTOs.Add(new PostDTO(post));
            };

            return postDTOs;
        }

        //returns the followee profileDTOs from a collection of follows
        private ICollection<SimpleProfileDTO> serializeFollowing(ICollection<Follow> follows)
        {
            List<SimpleProfileDTO> profileDTOs = new List<SimpleProfileDTO>();

            foreach (var f in follows)
            {
               profileDTOs.Add(new SimpleProfileDTO(f.Followee));
            };

            return profileDTOs;
        }

        //return the follower profileDTOs from a collection of follows
        private ICollection<SimpleProfileDTO> serializeFollower(ICollection<Follow> follows)
        {
            List<SimpleProfileDTO> profileDTOs = new List<SimpleProfileDTO>();

            foreach (var f in follows)
            {
                profileDTOs.Add(new SimpleProfileDTO(f.Follower));
            };

            return profileDTOs;
        }
    }
}
