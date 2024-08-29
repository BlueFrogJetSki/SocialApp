using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Models;

namespace SocialApp.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LikesController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string postId, int authorProfileId)
        {

            if (ModelState.IsValid)
            {
                var existingLike = await _context.Like
                .FirstOrDefaultAsync(l => l.AuthorProfileId == authorProfileId && l.PostId == postId);

                if (existingLike == null)
                {
                    Like newLike = new Like()
                    {

                        AuthorProfileId = authorProfileId,
                        PostId = postId
                    };
                    await _context.Like.AddAsync(newLike);
                    await _context.SaveChangesAsync();
                }



            }
            return RedirectToAction("Index", "Home");

        }
    }
}
