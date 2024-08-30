using SocialApp.Models;

namespace SocialApp.Interfaces
{
    public interface IUserProfileRepository
    {
        public Task<UserProfile?> GetAsync(string id);
        public Task<IEnumerable<UserProfile>> GetListAsync();
        public Task<bool> UpdateAsync(UserProfile userProfile);
        public Task<bool> UserProfileExists(string id);
        public Task<bool> SaveChanges();
    }
}
