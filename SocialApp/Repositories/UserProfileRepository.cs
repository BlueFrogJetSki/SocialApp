using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces;
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
            if (!await UserProfileExists(id)) return null;

            return await _context.UserProfile.FindAsync(id);

        }

        public async Task<IEnumerable<UserProfile>> GetListAsync()
        {
            return await _context.UserProfile.ToListAsync();
        }



        public async Task<bool> UpdateAsync(UserProfile userProfile)
        {
            if (!await UserProfileExists(userProfile.Id)) return false;

            var ExistingProfile = await GetAsync(userProfile.Id);

            if (ExistingProfile == null) return false;

            _context.Entry(ExistingProfile).CurrentValues.SetValues(userProfile);

            return await SaveChanges();

        }

        public async Task<bool> UserProfileExists(string id)
        {
            UserProfile? profile = await _context.UserProfile.FindAsync(id);

            if (profile == null) { return false; }

            return true;

        }

        public async Task<bool> SaveChanges()
        {
            int ChangesMade = await _context.SaveChangesAsync();
            Console.WriteLine($"{ChangesMade} changes made in UserProfile");
            //checks whether any changes were made
            return ChangesMade >= 1;
        }


    }
}
