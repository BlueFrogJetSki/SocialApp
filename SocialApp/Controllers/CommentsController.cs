using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialApp.Data;
using SocialApp.Models;

namespace SocialApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CommentsController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string postId, string authorProfileId, [Bind("Text")] Comment comment)
        {

            if (ModelState.IsValid)
            {
                var author = await _context.UserProfile.FirstOrDefaultAsync(user => user.Id == authorProfileId);

                if (author != null)
                {
                    comment.PostId = postId;
                    comment.AuthorProfileId = authorProfileId;
                    comment.AuthorName = author.UserName;
    
                    _context.Comment.Add(comment);
                    _context.SaveChanges();

                }
            }
            return RedirectToAction("Details", "Posts", new { id = postId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(string parentCommentId, string authorProfileId, [Bind("Text")] Comment comment)
        {
            var author = await _context.UserProfile.FirstOrDefaultAsync(p => p.Id == authorProfileId);
            var parentComment = await _context.Comment.FirstOrDefaultAsync(c => c.Id == parentCommentId);

            if (ModelState.IsValid)
            {
                

                //add new comment
                if (author != null && parentComment != null)
                {
                    comment.PostId = parentComment.PostId;
                    comment.AuthorProfileId = authorProfileId;
                    comment.AuthorName = author.UserName;

                    _context.Comment.Add(comment);
                    _context.SaveChanges();

                }

                //add child comment to parent
                parentComment?.SubComments.Add(comment);
                _context.SaveChanges();

            }
            return RedirectToAction("Details", "Posts", new { id = parentComment?.PostId });

        }



    }
}

