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
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository repository, IImageService imageService, ApplicationDbContext context)
        {
            _context = context;
            _imageService = imageService;
            _postRepository = repository;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            //Initalize DB if it is empty and there is at least one user in the db

            //if (!_context.Post.Any())
            //{
            //    Console.WriteLine("Post table empty, initalizing");
            //    var userID = _context.UserProfile.FirstOrDefaultAsync().Result?.Id;
            //    var resourcesResult = await _imageService.ListAllImageAsync();

            //    int count = 1;


            //    if (resourcesResult.Resources != null && userID != null)
            //    {
            //        Console.WriteLine(resourcesResult.Resources);
            //        foreach (var resource in resourcesResult.Resources)
            //        {
            //            Post newPost = new Post()
            //            {
            //                ImgURL = resource.Url.ToString(),
            //                Description = $"image {count}",
            //                AuthorProfileId = userID

            //            };
            //            _context.Post.Add(newPost);
            //            _context.SaveChanges();

            //            count++;
            //        }
            //    }
            //}

            var posts = await _postRepository.GetListAsync();

            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.GetAsync(id);

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
        public async Task<IActionResult> Create([Bind("Id,Img,Description,AuthorProfileId")] PostVM post)
        {

            if (ModelState.IsValid)
            {

                var result = await _imageService.UploadImageAsync(post.Img);



                if (result != null)
                {
                    var newPost = new Post()
                    {
                        Description = post.Description,
                        ImgURL = result.Url.ToString(),
                        AuthorProfileId = post.AuthorProfileId,
                    };

                    _context.Post.Add(newPost);
                    _context.SaveChanges();



                    return RedirectToAction(nameof(Index));
                }

            }

            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorProfileId);

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.GetAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorProfileId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Description")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postRepository.UpdateAsync(post);
                
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _postRepository.Exists(post.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorProfileId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.GetAsync(id);

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
            
            await _postRepository.DeleteAsync(id);
            

            
            return RedirectToAction(nameof(Index));
        }

       
    }
}
