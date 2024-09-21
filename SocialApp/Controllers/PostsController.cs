using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using SocialApp.Data;
using SocialApp.DataTransferObject;
using SocialApp.Interfaces.Repositories;
using SocialApp.Interfaces.Services;
using SocialApp.Models;

namespace SocialApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IPostRepository _postRepository;
        private readonly ILikeService _likeService;
        private readonly ITokenService _tokenService;


        public PostsController(IPostRepository repository, IImageService imageService, ApplicationDbContext context, ILikeService likeService, ITokenService tokenService)
        {
            _context = context;
            _imageService = imageService;
            _postRepository = repository;
            _likeService = likeService;
            _tokenService = tokenService;
           
        }

        [HttpGet("")]
        public async Task<ActionResult> Get()
        {

            var posts = await _postRepository.GetListAsync();
            var result = new List<PostDTO>();
            foreach (var p in posts)
            {
                var pDTO = new PostDTO(p);
                result.Add(pDTO);
            }

            return Ok(new { Posts = result }); // Return posts as JSON

        }

        // GET: Posts/Details/5
        [HttpGet("details/{id}")]
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

            var postDTO = new PostDTO(post);

            return Ok(new { Post = postDTO });
        }



        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreatePostDTO createPostDTO, IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Empty file uploaded.");

            if (!ModelState.IsValid || imageFile == null) { return BadRequest(ModelState); }


            //get token
            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }


            string? profileId = _tokenService.GetProfileIdFromToken(token);

            //invalid token
            if (profileId == null) { return Unauthorized(); }

            try
            {

                var result = await _imageService.UploadPostImageAsync(imageFile);

                var newPost = new Post()
                {
                    Description = createPostDTO.Description,
                    ImgURL = result.Url.ToString(),
                    AuthorProfileId = profileId,
                    CreatedAt = DateTime.Now.ToUniversalTime().ToUniversalTime(),

                };

                await _postRepository.CreateAsync(newPost);

                return CreatedAtAction(nameof(Details), new { id = newPost.Id });

            }
            catch (Exception ex)
            {
                //failed to upload image
                return BadRequest(new { error = ex.Message });
            }



        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromBody] CreatePostDTO createPostDTO)
        {
          

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var existingPost = await _postRepository.GetAsync(id);
            if (existingPost == null) { return NotFound(); }
            Console.WriteLine("checking cookie");
            //Checks if existingPost belongs to the user making the request
            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { Console.WriteLine("not cookie found"); return Unauthorized(); }
            Console.Write("Cookie found");
            Console.WriteLine(token);

            string? profileId = _tokenService.GetProfileIdFromToken(token);


            //invalid token
            if (profileId == null) { return Unauthorized(); }


            if (profileId != existingPost.AuthorProfileId)
            {
                return Unauthorized(new { message = "User is not authorized to edit this post" });
            }

            //update description
            existingPost.Description = createPostDTO.Description;
            existingPost.UpdatedAt = DateTime.Now.ToUniversalTime().ToUniversalTime();

            var updateResult = await _postRepository.UpdateAsync(existingPost);


            if (updateResult)
            {
                var postDTO = new PostDTO(existingPost);
                return Ok(new { success = true, post = postDTO });

            }

            //Failed to save update to the database
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
        }


        // POST: Posts/Delete/5
        [HttpPost("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var existingPost = await _context.Post.FindAsync(id);

            if (existingPost == null) { return NotFound(); }


            //Checks if existingPost belongs to the user making the request
            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }

            string? profileId = _tokenService.GetProfileIdFromToken(token);

            //invalid token
            if (profileId == null) { return Unauthorized(); }


            if (profileId != existingPost.AuthorProfileId)
            {
                return Unauthorized("User is not authorized to edit this post");
            }


            if (profileId != existingPost.AuthorProfileId)
            {
                return Unauthorized("User is not authorized to edit this post");
            }


            var deleteResult = await _postRepository.DeleteAsync(id);

            if (deleteResult)
            {
                return Ok(new { success = true });

            }
            //Failed to save update to the database
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });


        }

        [HttpPost("like/{id}")]
        [Authorize]
        public async Task<IActionResult> Like(string id)
        {
            var post = await _postRepository.GetAsync(id);

            if (post == null) { return NotFound(); }

            //get profileId from token
            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }

            string? profileId = _tokenService.GetProfileIdFromToken(token);

            //invalid token
            if (profileId == null) { return Unauthorized(); }

            _likeService.LikeItem(post, profileId);

            return Ok(new { success = true });

        }



    }
}
