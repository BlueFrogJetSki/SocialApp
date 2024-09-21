using Microsoft.EntityFrameworkCore;
using SocialApp.Models;

namespace SocialApp.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<bool> DeleteAsync(string AuthorProfileId, string EntityType, string EntityId);

        Task<bool> ExistsAsync(string AuthorProfileId, string EntityType, string EntityId);

        Task<Like?> GetAsync(string AuthorProfileId, string EntityType, string EntityId);

        Task<IEnumerable<Like>> GetListAsync(string entityType, string entityID);

        Task<bool> SaveChangesAsync();

        Task<bool> UpdateAsync(Like like);

        Task<bool> CreateAsync(Like like);
    }
}
