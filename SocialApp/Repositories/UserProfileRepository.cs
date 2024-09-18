using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces.Repositories;
using SocialApp.Models;

namespace SocialApp.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

     
        public async Task<UserProfile?> GetAsync(string id)
        {
            if (!await ExistsAsync(id)) return null;

            return await _context.UserProfile.Include(p => p.Posts)
                .Include(p => p.Followers)
                .Include(p => p.Following)
                .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<IEnumerable<UserProfile>> GetListAsync()
        {
            return await _context.UserProfile.ToListAsync();
        }



        public async Task<bool> UpdateAsync(UserProfile userProfile)
        {
            if (!await ExistsAsync(userProfile.Id)) return false;

            var ExistingProfile = await GetAsync(userProfile.Id);

            if (ExistingProfile == null) return false;

            _context.Entry(ExistingProfile).CurrentValues.SetValues(userProfile);

            return await SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(string id)
        {
            UserProfile? profile = await _context.UserProfile.FindAsync(id);

            if (profile == null) { return false; }

            return true;

        }

        public async Task<bool> SaveChangesAsync()
        {
            int ChangesMade = await _context.SaveChangesAsync();
            Console.WriteLine($"{ChangesMade} changes made in UserProfile");
            //checks whether any changes were made
            return ChangesMade >= 1;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UserProfile? UserProfile = await GetAsync(id);

            if (UserProfile == null) { return false; }

            _context.UserProfile.Remove(UserProfile);

            return await SaveChangesAsync();

        }

        public async Task<bool> CreateAsync(UserProfile UserProfile)
        {
            await _context.UserProfile.AddAsync(UserProfile);
            return await SaveChangesAsync();
        }

        public async Task<UserProfile> getByUsernameAsync(string Username)
        {
          return await _context.UserProfile.FirstOrDefaultAsync(u => u.UserName == Username);
        }
    }
}
