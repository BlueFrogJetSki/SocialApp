namespace SocialApp.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetAsync(string id);
        Task<IEnumerable<TEntity>> GetListAsync();
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> SaveChangesAsync();
        Task<bool> CreateAsync(TEntity entity);
    }
}
