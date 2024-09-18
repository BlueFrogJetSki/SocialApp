using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.DataTransferObject;
using SocialApp.Interfaces.Repositories;
using SocialApp.Interfaces.Services;
using SocialApp.Models;

namespace SocialApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILikeService _likeService;
        private readonly ITokenService _tokenService;
        private readonly ICommentRepository _commentRepository;
        public CommentsController(ApplicationDbContext context, ITokenService tokenService, ILikeService likeService, ICommentRepository commentRepository)
        {
            _context = context;
            _likeService = likeService;
            _tokenService = tokenService;
            _commentRepository = commentRepository;

        }

        [HttpGet("{postId}")]
        //Return CommentDTOs of the comments belonging to postId
        public async Task<IActionResult> Get(string postId)
            
        {
            var comments = await _commentRepository.GetCommentsForPostAsync(postId);
            var commentDTOs = commentDTOSerializer(comments);



            return Ok(commentDTOs);

        }

        [HttpPost("create/{postId}")]
        [Authorize]
        public async Task<IActionResult> Create(string postId, [FromBody] Comment comment)
        {

            if (!ModelState.IsValid) { return BadRequest(ModelState); }



            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }

            string? profileId = _tokenService.GetProfileIdFromToken(token);

            UserProfile? author = await _context.UserProfile.FirstOrDefaultAsync(user => user.Id == profileId);

            if (author == null) { return NotFound("profile not found"); }

            comment.PostId = postId;
            comment.AuthorProfileId = author.Id;
            comment.AuthorName = author.UserName;

            //abstract to repository
            var createdComment = await _commentRepository.CreateAsync(comment);

            if(!createdComment) { return StatusCode(500); }

            var resultComment = await _commentRepository.GetAsync(comment.Id);

            return Ok(new CommentDTO(resultComment));

        }

        [HttpPost("reply/{parentCommentId}")]
        [Authorize]
        public async Task<IActionResult> Reply(string parentCommentId, [FromBody] Comment comment)
        {

            var parentComment = await _commentRepository.GetAsync(parentCommentId);

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }
            string? profileId = _tokenService.GetProfileIdFromToken(token);

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




            return Ok(new CommentDTO(comment));
        }

        [HttpPost("like/{id}")]
        [Authorize]
        public async Task<IActionResult> Like(string id)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(p => p.Id == id);

            if (comment == null) { return NotFound(); }

            //get profileId from token
            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }

            string? profileId = _tokenService.GetProfileIdFromToken(token);

            //invalid token
            if (profileId == null) { return Unauthorized(); }

            Like? like = _likeService.LikeItem(comment, profileId);


            return Ok(like);

        }


        private ICollection<CommentDTO> commentDTOSerializer(ICollection<Comment> comments)
        {
            var commentDTOs = new List<CommentDTO>();
            foreach (var comment in comments)
            {
                commentDTOs.Add(new CommentDTO(comment));
            }

            return commentDTOs;
        }




    }
}

