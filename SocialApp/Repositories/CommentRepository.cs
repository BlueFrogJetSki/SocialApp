using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces.Repositories;
using SocialApp.Models;

namespace SocialApp.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Comment? Comment = await GetAsync(id);

            if (Comment == null) { return false; }

            _context.Comment.Remove(Comment);

            return await SaveChangesAsync();

        }

        public async Task<bool> ExistsAsync(string id)
        {
            var Comment = await _context.Comment.FindAsync(id);

            return Comment != null;
        }

        public async Task<Comment?> GetAsync(string id)
        {
            return await _context.Comment.Include(c => c.AuthorProfile).Include(c => c.SubComments).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetListAsync()
        {
            return await _context.Comment.Include(p => p.AuthorProfile)
                .Include(p => p.Likes)
                .Include(p => p.SubComments).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            var NumChanges = await _context.SaveChangesAsync();
            return NumChanges > 0;
        }

        public async Task<bool> UpdateAsync(Comment Comment)
        {
            if (!await ExistsAsync(Comment.Id)) return false;

            var ExistingComment = await GetAsync(Comment.Id);

            if (ExistingComment == null) return false;

            _context.Entry(ExistingComment).CurrentValues.SetValues(Comment);

            return await SaveChangesAsync();
        }

        public async Task<bool> CreateAsync(Comment Comment)
        {
            await _context.Comment.AddAsync(Comment);
            return await SaveChangesAsync();
        }

        async Task<List<Comment>> ICommentRepository.GetCommentsForPostAsync(string postId)
        {
            var comments = await _context.Comment.Where(c => c.PostId == postId).ToListAsync();
            return comments;
        }
    }
}

