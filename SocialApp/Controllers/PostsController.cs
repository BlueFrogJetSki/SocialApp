using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces;
using SocialApp.Models;
using SocialApp.ViewModel;

namespace SocialApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public PostsController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            //Initalize DB if it is empty

            //if (_context.Post.any())
            //{
            //    var resourcesResult = await _imageService.ListAllImageAsync();
            //    Console.WriteLine(resourcesResult);
            //    int count = 1;
            //    if (resourcesResult.Resources != null)
            //    {
            //        Console.WriteLine(resourcesResult.Resources);
            //        foreach (var resource in resourcesResult.Resources)
            //        {
            //            Post newPost = new Post()
            //            {
            //                ImgURL = resource.Url.ToString(),
            //                Description = $"image {count}",
            //                AppUserId = "5f66959b-e76a-413f-a40f-73ca244d7d7c"

            //            };
            //            _context.Post.Add(newPost);
            //            _context.SaveChanges();

            //            count++;
            //        }
            //    }
            //}

            var applicationDbContext = _context.Post.Include(p => p.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImgURL,Description,AppUserId")] PostVM post)
        {

            if (ModelState.IsValid)
            {

                var result = await _imageService.UploadImageAsync(post.ImgURL);



                if (result != null)
                {
                    var newPost = new Post()
                    {
                        Description = post.Description,
                        ImgURL = result.Url.ToString(),
                        AppUserId = post.AppUserId,
                    };

                    _context.Post.Add(newPost);
                    _context.SaveChanges();



                    return RedirectToAction(nameof(Index));
                }

            }

            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", post.AppUserId);

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", post.AppUserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,ImgURL,Description,AppUserId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", post.AppUserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(string id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
