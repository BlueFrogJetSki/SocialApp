using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.DataTransferObject;
using SocialApp.Interfaces.Repositories;
using SocialApp.Interfaces.Services;
using SocialApp.Models;
using SocialApp.Services;


namespace SocialApp.Controllers
{
    //TODO Change id param to username
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : Controller
    {
        private readonly IUserProfileRepository _repository;
        private readonly IImageService _imageService;
        private readonly ITokenService _tokenService;

        public UserProfilesController(IUserProfileRepository repository, IImageService imageService, ITokenService tokenService)
        {
            _repository = repository;
            _imageService = imageService;
            _tokenService = tokenService;
        }



        // GET: UserProfiles/Details/5 
        [HttpGet("details/{username}")]
        [Authorize]
        public async Task<IActionResult> Details(string username)
        {

            var userProfile = await _repository.getByUsernameAsync(username);

            if (userProfile == null)
            {
                return NotFound();
            }

            var profileDetailsDTO = new ProfileDetailsDTO(userProfile);

            return Ok(profileDetailsDTO);
        }

        // POST: UserProfiles/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{username}")]
        [Authorize]
        public async Task<IActionResult> Edit(string username, [FromForm] EditProflieDTO profileDTO)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userProfile = await _repository.getByUsernameAsync(username);

            if (userProfile == null)
            {
                return NotFound();
            }


            //if Image is provided, upload the image and change IconURL, else do nothing
            // Handle image upload
            if (profileDTO.IconImg != null)
            {
               
                try
                {
                    var result = await _imageService.UploadProfileImageAsync(profileDTO.IconImg);

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
