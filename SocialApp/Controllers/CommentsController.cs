using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private readonly ILikeService _likeService;
        private readonly ITokenService _tokenService;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPostRepository _postRepository;
        public CommentsController(IUserProfileRepository userProfileRepository, ITokenService tokenService, ILikeService likeService, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _userProfileRepository = userProfileRepository;
            _likeService = likeService;
            _tokenService = tokenService;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        [HttpGet("{postId}")]
        //Return CommentDTOs of the comments belonging to postId
        public async Task<IActionResult> Get(string postId)

        {
            var comments = await _commentRepository.GetCommentsForPostAsync(postId);
            var commentDTOs = commentDTOSerializer(comments);
            return Ok(commentDTOs);

        }

        [HttpGet("subcomments/{parentId}")]
        //Return CommentDTOs of the subcomments belonging to parentID
        public async Task<IActionResult> GetSubcomments(string parentId)

        {
            var subcomments = await _commentRepository.GetSubComments(parentId);
            var commentDTOs = commentDTOSerializer(subcomments);
            return Ok(commentDTOs);

        }

        [HttpPost("create/{postId}")]
        [Authorize]
        //create comment in db
        // increment commentcount in post with post id
        public async Task<IActionResult> Create(string postId, [FromBody] CreateCommentDTO commentDTO)
        {

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }

            string? profileId = _tokenService.GetProfileIdFromToken(token);


            UserProfile? author = await _userProfileRepository.GetAsync(profileId);

            if (author == null) { return Unauthorized("profile not found"); }


            Post? post = await _postRepository.GetAsync(postId);
            if (post == null) { return NotFound("post not found"); }


            Comment comment = new Comment(postId, author.Id, commentDTO.Text);

            Console.WriteLine(author.UserName);
            //abstract to repository
            var createdComment = await _commentRepository.CreateAsync(comment);

            if (!createdComment) { return StatusCode(500); }

            //update comment count on post
            await updatePostCommentCount(post, 1);

            return Ok(new CommentDTO(comment));

        }

        [HttpPost("reply/{parentCommentId}")]
        [Authorize]
        public async Task<IActionResult> Reply(string parentCommentId, [FromBody] CreateCommentDTO commentDTO)
        {


            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }
            string? profileId = _tokenService.GetProfileIdFromToken(token);

            UserProfile? author = await _userProfileRepository.GetAsync(profileId);
            if (author == null) { return Unauthorized("profile not found"); }


            var parentComment = await _commentRepository.GetAsync(parentCommentId);

            //add new comment
            if (parentComment == null) { return NotFound(); }

            Post? post = await _postRepository.GetAsync(parentComment.PostId);
            if (post == null) { return NotFound("post not found"); }

            var comment = new Comment(parentComment.PostId, author.Id, commentDTO.Text);

            comment.parentCommentId = parentCommentId;

            var result = await _commentRepository.CreateAsync(comment);

            if (!result) { return StatusCode(500); }

            await updateSubcommentCount(parentComment, 1);

            await updatePostCommentCount(post, 1);


            return Ok(new CommentDTO(comment));
        }

        [HttpPost("update/{commentId}")]
        [Authorize]
        //update comment's text
        public async Task<IActionResult> update(string commentId, [FromBody] CreateCommentDTO commentDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }
            string? profileId = _tokenService.GetProfileIdFromToken(token);

            var comment = await _commentRepository.GetAsync(commentId);
            if (comment == null) { return NotFound(); }


            UserProfile? author = await _userProfileRepository.GetAsync(profileId);
            if (author == null) { return Unauthorized("profile not found"); }
            if (comment.AuthorProfileId != profileId) { return Unauthorized(); }

            comment.Text = commentDTO.Text;
            await _commentRepository.UpdateAsync(comment);

            return Ok(new CommentDTO(comment));
        }

        [HttpPost("delete/{commentId}")]
        [Authorize]
        //update comment's text
        public async Task<IActionResult> delete(string commentId, [FromBody] CreateCommentDTO commentDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }
            string? profileId = _tokenService.GetProfileIdFromToken(token);

            var comment = await _commentRepository.GetAsync(commentId);
            if (comment == null) { return NotFound(); }


            UserProfile? author = await _userProfileRepository.GetAsync(profileId);
            if (author == null) { return Unauthorized("profile not found"); }
            if (comment.AuthorProfileId != profileId) { return Unauthorized(); }

            //update post comment count
            var post = await _postRepository.GetAsync(comment.PostId);
            await updatePostCommentCount(post, -1);

            //update parent's subcomment count if parent exist
            if (comment.parentCommentId != null)
            {
                var parentComment = await _commentRepository.GetAsync(comment.parentCommentId);
                await updateSubcommentCount(parentComment, -1);
            }

            var result = await _commentRepository.DeleteAsync(commentId);

            if (result)
            {
                return Ok(new { message="delete successful"});
            }

            return StatusCode(500);
            
        }

        //[HttpPost("like/{id}")]
        //[Authorize]
        //public async Task<IActionResult> Like(string id)
        //{
        //    var comment = await _context.Comment.FirstOrDefaultAsync(p => p.Id == id);

        //    if (comment == null) { return NotFound(); }

        //    //get profileId from token
        //    if (!Request.Cookies.TryGetValue("Authorization", out var token)) { return Unauthorized(); }

        //    string? profileId = _tokenService.GetProfileIdFromToken(token);

        //    //invalid token
        //    if (profileId == null) { return Unauthorized(); }

        //    var success = _likeService.LikeItem(comment, profileId);


        //    return Ok(new { success = success });

        //}


        private ICollection<CommentDTO> commentDTOSerializer(ICollection<Comment> comments)
        {
            var commentDTOs = new List<CommentDTO>();
            foreach (var comment in comments)
            {
                commentDTOs.Add(new CommentDTO(comment));
            }

            return commentDTOs;
        }

        // i < 0 < i
        private async Task<bool> updatePostCommentCount(Post post, int i)
        {
            post.CommentCount += i;
            return await _postRepository.UpdateAsync(post);
        }

        private async Task<bool> updateSubcommentCount(Comment comment, int i)
        {
            comment.SubcommentCount += i;
            return await _commentRepository.UpdateAsync(comment);
        }




    }
}

