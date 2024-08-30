using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces;
using SocialApp.Models;

namespace SocialApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository _postRepository;
        private readonly IImageService _imageService;

        public HomeController(IPostRepository repository, IImageService imageService, ApplicationDbContext context)
        {
            _context = context;
            _imageService = imageService;
            _postRepository = repository;

        }

        public async Task<ActionResult> Index()
        {
            //Initialize DB if it is empty

            if (!_context.Post.Any())
            {
                var user = _context.UserProfile.FirstOrDefaultAsync().Result;

                var resourcesResult = await _imageService.ListAllImageAsync();
                Console.WriteLine(resourcesResult);
                int count = 1;
                if (resourcesResult.Resources != null && user != null)
                {
                    Console.WriteLine(resourcesResult.Resources);
                    foreach (var resource in resourcesResult.Resources)
                    {
                        Post newPost = new Post()
                        {

                            ImgURL = resource.Url.ToString(),
                            Description = $"image {count}",
                            AuthorProfileId = user.Id,

                        };

                        //add comments to posts
                        newPost.Comments.Add(new Comment
                        {
                            Text = $"Very cool post {user.UserName}",
                            PostId = newPost.Id,
                            AuthorProfileId = user.Id,
                            AuthorName = user.UserName,
                        });

                        _context.Post.Add(newPost);
                        _context.SaveChanges();

                        count++;
                    }
                }
            }
            var Posts = await _postRepository.GetListAsync();

            return View(Posts);

        }


    }
}
