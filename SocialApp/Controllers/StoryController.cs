using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.DataTransferObject;
using SocialApp.Models;

namespace SocialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get a specific story with id
        [HttpGet("{id}")]
        public async Task<ActionResult<Story>> Get(int id)
        {
            var story = await _context.Story.FindAsync(id);

            if (story == null)
            {
                return NotFound(); // Returns a 404 Not Found if the story is not found
            }

            return Ok(story); // Returns a 200 OK with the story serialized to JSON
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult<Story>> Create(StoryDTO dto)
        {
            var cookieValue = HttpContext.Request.Cookies[".AspNetCore.Identity.Application"];
            
            // Validate the input data
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new Story entity from the DTO
            var story = new Story
            {
                DateTime = dto.DateTime,
                IsHidden = dto.IsHidden,
                ImgURL = dto.ImgURL,
                LikesCount = dto.LikesCount,
                AuthorProfileId = dto.AuthorProfileId
            };

            // Add the new story to the database
            _context.Story.Add(story);
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the created story
            // Create{ "location" : storypath }in header
            return CreatedAtAction(nameof(Get), new { id = story.Id }, story);
        }


        [HttpPost]
        [HttpPost("delete/{id}")]
        [Authorize]
        public async Task<ActionResult<Story>> Delete([FromRoute] int id)
        {
            var targetStory = await _context.Story.FirstOrDefaultAsync(s => s.Id == id);

            if (targetStory == null)
            {
                return NotFound();
            }

            var result = _context.Story.Remove(targetStory);
            _context.SaveChanges();

            //return 204 status code
            return NoContent();
        }



    }
}
