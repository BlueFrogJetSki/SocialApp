using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
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
        private readonly ILikeService _likeService;

        public PostsController(IPostRepository repository, IImageService imageService, ApplicationDbContext context, ILikeService likeService)
        {
            _context = context;
            _imageService = imageService;
            _postRepository = repository;
            _likeService = likeService;


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

        [HttpPost]
        public async Task<string> Like(string id)
        {
            var post = await _postRepository.GetAsync(id);
            if (post != null)
            {
                _likeService.LikeItem(post, post.AuthorProfileId);

                return "Post was liked";
            }

            return $"Like failed";
        }


    }
}
