using SocialApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.DataTransferObject
{
    public class CommentDTO
    {
        // It is initialized with a new GUID to ensure uniqueness.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }
        public int LikesCount { get; set; } = 0;
        public int SubcommentCount { get; set; } = 0;

        [Required(ErrorMessage = "Comment must contain at least 1 character")]
        public string? Text { get; set; }

        public SimpleProfileDTO? AuthorDTO { get; set; }
        public ICollection<CommentDTO>? SubCommentDTOs { get; set; }

        public CommentDTO() { }
        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            CreatedAt = comment.CreatedAt;
            LikesCount = comment.LikesCount;
            Text = comment.Text;
            AuthorDTO = new SimpleProfileDTO(comment.AuthorProfile);
            SubcommentCount = comment.SubcommentCount;

        }

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
