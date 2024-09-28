using SocialApp.Models;

namespace SocialApp.Interfaces.Repositories
{
    public interface ICommentRepository:IBaseRepository<Comment>
    {
        public Task<List<Comment>> GetCommentsForPostAsync(string postId);

        public Task<List<Comment>> GetSubComments(string parentId);

    }
}
