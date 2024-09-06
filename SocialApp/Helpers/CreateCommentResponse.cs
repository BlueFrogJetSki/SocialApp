using SocialApp.Models;

namespace SocialApp.Helpers
{
    public class CreateCommentResponse
    {
        public bool Success { get; set; }
        public Comment Comment { get; set; }
    }
}
