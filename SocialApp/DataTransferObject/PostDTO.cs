using SocialApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class PostDTO
    {
        public string Id { get; set; }

        //Meta Info
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        public string? ImgURL { get; set; }

        public string? Description { get; set; }

        // The Hidden property indicates whether the post is visible to users.
        public bool Hidden { get; set; } = false;

        //Author Info
        public SimpleProfileDTO? AuthorDTO { get; set; }

        public int CommentCount { get; set; } = 0;

        // From ILikeable
        public int LikesCount { get; set; } = 0;

        public PostDTO() { }
        public PostDTO(Post post)
        {
            Id = post.Id;
            CreatedAt = post.CreatedAt;
            ImgURL = post.ImgURL;
            Description = post.Description;
            Hidden = post.Hidden;
            AuthorDTO = new SimpleProfileDTO(post.AuthorProfile);
            LikesCount = post.LikesCount;
            CommentCount = post.CommentCount;
        }

      
    }
}
