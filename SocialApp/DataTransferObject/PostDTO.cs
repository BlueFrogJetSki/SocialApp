using SocialApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class PostDTO
    {
        // It is initialized with a new GUID to ensure uniqueness.
      
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

        // The Comments property holds a collection of comments associated with the post.
        public ICollection<CommentDTO> CommentDTOs { get; set; }

        // From ILikeable
        public int LikesCount { get; set; } = 0;
        public ICollection<Like> Likes { get; set; } = new HashSet<Like>();

        public PostDTO() { }
        public PostDTO(Post post)
        {
            Id = post.Id;
            CreatedAt = post.CreatedAt;
            ImgURL = post.ImgURL;
            Description = post.Description;
            Hidden = post.Hidden;
            AuthorDTO = new SimpleProfileDTO(post.AuthorProfile);
            CommentDTOs = serializeComments(post.Comments);
            LikesCount = post.LikesCount;
            Likes = post.Likes;

            

        }

        //TODO Create a single definition for this function to be used across DTOS
        public List<CommentDTO> serializeComments(ICollection<Comment> comments)
        {
            var result = new List<CommentDTO>();
            foreach (Comment comment in comments)
            {

                result.Add(new CommentDTO(comment));
            }

            return result;
        }
    }
}
