using SocialApp.Models;

namespace SocialApp.Interfaces
{
    public interface IPostRepository
    {
        public Task<Post?> GetAsync(string id);
        public Task<IEnumerable<Post>> GetListAsync();
        public Task<bool> UpdateAsync(Post post);
        public Task<bool> DeleteAsync(string id);
        public Task<bool> Exists(string id);
        public Task<bool> SaveChanges();

    }
}
