using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces.Repositories;
using SocialApp.Models;

namespace SocialApp.Repositories
{
    public class LikeRepository:ILikeRepository
    {
        private readonly ApplicationDbContext _context;
        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Like? Like = await GetAsync(id);

            if (Like == null) { return false; }

            _context.Like.Remove(Like);

            return await SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(string id)
        {
            var Like = await _context.Like.FindAsync(id);

            return Like != null;
        }

        public async Task<Like?> GetAsync(string id)
        {
            return await _context.Like.Include(l => l.AuthorProfile).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Like>> GetListAsync()
        {
            return await _context.Like.Include(p => p.AuthorProfile).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var NumChanges = await _context.SaveChangesAsync();
            return NumChanges > 0;
        }

        public async Task<bool> UpdateAsync(Like Like)
        {

            var ExistingLike = await GetAsync(Like.Id);
            if (ExistingLike == null) { return false; }
            
            _context.Entry(ExistingLike).CurrentValues.SetValues(Like);

            return await SaveChangesAsync();
        }

        public async Task<bool> CreateAsync(Like Like)
        {
            await _context.Like.AddAsync(Like);
            return await SaveChangesAsync();
        }


    }
}

