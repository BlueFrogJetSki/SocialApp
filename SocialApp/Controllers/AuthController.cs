using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Interfaces.Services;
using SocialApp.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.Controllers
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterInputModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;



        public AuthController(UserManager<AppUser> userManager, ApplicationDbContext context, ITokenService tokenService, IUserStore<AppUser> userStore, IEmailSender emailSender, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;

        }

        //TODO implement register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel inputModel)
        {

            Console.WriteLine(Request.Body.ToString());
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = CreateUser();

            user.UserProfile = new UserProfile { UserName = inputModel.UserName };

            await _userStore.SetUserNameAsync(user, inputModel.UserName, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, inputModel.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, inputModel.Password);

            if (result.Succeeded) { return Ok(new { success = true, message = "successfully created account" }); }

            return StatusCode(StatusCodes.Status500InternalServerError, new { errors = result.Errors,message = "An unexpected error occurred." });


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Include userProfile here so tokenservice can use it later
            var user = await _context.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.UserName == loginModel.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var token = _tokenService.GenerateJwtToken(user);
               
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthCookie");
            return Ok();
        }
       

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


    }


}
