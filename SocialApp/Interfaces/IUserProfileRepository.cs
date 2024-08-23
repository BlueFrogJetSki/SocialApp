using SocialApp.Models;

namespace SocialApp.Interfaces
{
    public interface IUserProfileRepository
    {
        public Task<UserProfile?> GetAsync(int id);
        public Task<IEnumerable<UserProfile>> GetListAsync();
        public Task<bool> UpdateAsync(UserProfile userProfile);
        public Task<bool> UserProfileExists(int id);
        public Task<bool> SaveChanges();
    }
}
