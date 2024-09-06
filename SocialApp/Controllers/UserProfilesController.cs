using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.DataTransferObject;
using SocialApp.Interfaces.Repositories;
using SocialApp.Interfaces.Services;
using SocialApp.Models;
using SocialApp.ViewModel;

namespace SocialApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : Controller
    {
        private readonly IUserProfileRepository _repository;
        private readonly IImageService _imageService;

        public UserProfilesController(IUserProfileRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }



        // GET: UserProfiles/Details/5 
        [HttpGet("details/{id}")]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {

            var userProfile = await _repository.GetAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            var profileDetailsDTO = new ProfileDetailsDTO(userProfile);

            return Ok(profileDetailsDTO);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [FromForm] EditProflieDTO profileDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!await _repository.ExistsAsync(id))
            {
                return NotFound();
            }

            UserProfile? userProfile = await _repository.GetAsync(id);

            //if Image is provided, upload the image and change IconURL, else do nothing
            // Handle image upload
            if (profileDTO.IconImg != null)
            {
               
                try
                {
                    var result = await _imageService.UploadImageAsync(profileDTO.IconImg);

                    if (result != null)
                    {
                        userProfile.IconURL = result.Url.ToString();
                    }
                }
                catch (Exception ex)
                {
                   return StatusCode(500, new {error = ex, message = "failed to upload image"});
                }
                
            }

            //updates the profile

            userProfile.UserName = profileDTO.UserName;
            userProfile.Biography = profileDTO.Biography;

            if (await _repository.UpdateAsync(userProfile))
            { 
                return Ok(userProfile);
            
            };

            return StatusCode(500, new { message = "failed to update profile" }); ;
        }




    }
}
