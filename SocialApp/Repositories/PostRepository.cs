using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces.Repositories;
using SocialApp.Models;


namespace SocialApp.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Post? post = await _context.Post.FindAsync(id);

            if (post == null) { return false; }

            _context.Post.Remove(post);

            return await SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(string id)
        {
            var post = await _context.Post.FindAsync(id);

            return post != null;
        }

        public async Task<Post?> GetAsync(string id)
        {
            return await _context.Post
                .Include(p => p.AuthorProfile)
                .FirstOrDefaultAsync(p=> p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetListAsync()
        {
            return await _context.Post.Include(p => p.AuthorProfile)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var NumChanges = await _context.SaveChangesAsync();
            return NumChanges > 0;
        }

        public async Task<bool> UpdateAsync(Post post)
        {
            var ExistingPost = await GetAsync(post.Id);

            if (ExistingPost == null) return false;

            _context.Entry(ExistingPost).CurrentValues.SetValues(post);

            return await SaveChangesAsync();
        }

        public async Task<bool> CreateAsync(Post post)
        {
            await _context.Post.AddAsync(post);
            return await SaveChangesAsync();
        }

    
    }
}
