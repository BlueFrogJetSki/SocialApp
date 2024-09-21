using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public async Task<bool> DeleteAsync(string AuthorProfileId, string EntityType, string EntityId)
        {
            Like? Like = await _context.Like.FirstOrDefaultAsync(l => l.AuthorProfileId == AuthorProfileId && l.EntityType == EntityType && l.EntityId == EntityId);

            if (Like == null) { return false; }

            _context.Like.Remove(Like);

            return await SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(string AuthorProfileId, string EntityType, string EntityId)
        {
            var Like = await _context.Like.FirstOrDefaultAsync(l => l.AuthorProfileId == AuthorProfileId && l.EntityType == EntityType && l.EntityId == EntityId);


            return Like != null;
        }

        public async Task<Like?> GetAsync(string AuthorProfileId, string EntityType, string EntityId)
        {
            return await _context.Like.FirstOrDefaultAsync(l => l.AuthorProfileId == AuthorProfileId && l.EntityType == EntityType && l.EntityId == EntityId);

        }

        public async Task<IEnumerable<Like>> GetListAsync(string entityType, string entityID)
        {
            return await _context.Like.Where(l => l.EntityType == entityType && l.EntityId == entityID).ToListAsync();

        }

        public async Task<bool> SaveChangesAsync()
        {
            var NumChanges = await _context.SaveChangesAsync();
            return NumChanges > 0;
        }

        public async Task<bool> UpdateAsync(Like Like)
        {

            var ExistingLike = await GetAsync(Like.AuthorProfileId, Like.EntityType, Like.EntityId);
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

