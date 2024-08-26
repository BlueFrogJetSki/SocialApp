﻿using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces;
using SocialApp.Models;
using SocialApp.ViewModel;

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
            Post? post = await GetAsync(id);

            if (post == null) { return false; }

            _context.Post.Remove(post);

            return await SaveChanges();

        }

        public async Task<bool> Exists(string id)
        {
            var post = await _context.Post.FindAsync(id);

            return post != null;
        }

        public async Task<Post?> GetAsync(string id)
        {
            return await _context.Post
                .Include(p => p.AuthorProfile)
                .Include(p => p.LikedUsers)
                .Include(p => p.Comments).FirstOrDefaultAsync(p=> p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetListAsync()
        {
            return await _context.Post.Include(p => p.AuthorProfile)
                .Include(p => p.LikedUsers)
                .Include(p => p.Comments).ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            var NumChanges = await _context.SaveChangesAsync();
            return NumChanges > 0;
        }

        public async Task<bool> UpdateAsync(Post post)
        {
            if (!await Exists(post.Id)) return false;

            var ExistingPost = await GetAsync(post.Id);

            if (ExistingPost == null) return false;

            _context.Entry(ExistingPost).CurrentValues.SetValues(post);

            return await SaveChanges();
        }
    }
}
