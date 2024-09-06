using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Helpers;
using SocialApp.Interfaces.Services;
using SocialApp.Models;
using SocialApp.Services;

namespace SocialApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILikeService _likeService;
        private readonly ITokenService _tokenService;
        public CommentsController(ApplicationDbContext context, ITokenService tokenService, ILikeService likeService)
        {
            _context = context;
            _likeService = likeService;
            _tokenService = tokenService;

        }

        [HttpPost("create/{postId}")]
        [Authorize]
        public async Task<IActionResult> Create(string postId, [FromBody] Comment comment)
        {


            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string? profileId = _tokenService.GetProfileIdFromToken(authHeader);

            UserProfile? author = await _context.UserProfile.FirstOrDefaultAsync(user => user.Id == profileId);

            if (author == null) { return NotFound("profile not found"); }

            //abstract to repository
            comment.PostId = postId;
            comment.AuthorProfileId = author.Id;
            comment.AuthorName = author.UserName;

            _context.Comment.Add(comment);
            _context.SaveChanges();



            return Ok( comment );

        }

        [HttpPost("reply/{parentCommentId}")]
        [Authorize]
        public async Task<IActionResult> Reply(string parentCommentId, [FromBody] Comment comment)
        {

            var parentComment = await _context.Comment.FirstOrDefaultAsync(c => c.Id == parentCommentId);

            if (!ModelState.IsValid) { return BadRequest(ModelState); }


            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            string? profileId = _tokenService.GetProfileIdFromToken(authHeader);

            UserProfile? author = await _context.UserProfile.FirstOrDefaultAsync(user => user.Id == profileId);



            //add new comment
            if (author == null || parentComment == null) { return NotFound(); }

            comment.PostId = parentComment.PostId;
            comment.AuthorProfileId = author.Id;
            comment.AuthorName = author.UserName;

            _context.Comment.Add(comment);
            _context.SaveChanges();



            //add child comment to parent
            parentComment?.SubComments.Add(comment);
            _context.SaveChanges();


            return Ok(comment);
        }

        [HttpPost("like/{id}")]
        [Authorize]
        public async Task<IActionResult> Like(string id)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(p=>p.Id == id);

            if (comment == null) { return NotFound(); }

            //get profileId from token
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();

            string? profileId = _tokenService.GetProfileIdFromToken(authHeader);

            //invalid token
            if (profileId == null) { return Unauthorized(); }

            Like? like = _likeService.LikeItem(comment, profileId);


            return Ok(like);

        }

       


    }
}

