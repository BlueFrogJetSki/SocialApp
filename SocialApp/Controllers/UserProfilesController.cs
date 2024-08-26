using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Enum;
using SocialApp.Interfaces;
using SocialApp.Models;
using SocialApp.ViewModel;

namespace SocialApp.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly IUserProfileRepository _repository;
        private readonly IImageService _imageService;

        public UserProfilesController(IUserProfileRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            var list = await _repository.GetListAsync();
            return View(list);
        }

        // GET: UserProfiles/Details/5 
        public async Task<IActionResult> Details(int id)
        {
            if (!await _repository.UserProfileExists(id))
            {
                return NotFound();
            }

            var userProfile = await _repository.GetAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileVM = new UserProfileVM(userProfile);

            return View(userProfileVM);
        }



        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var userProfile = await _repository.GetAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            var userProfileVM = new UserProfileVM(userProfile);

            //List<Interest> interests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();

            //ViewData["Interests"] = interests;
            return View(userProfileVM);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Biography,IconImg")] UserProfileVM userProfileVM)
        {
            Console.WriteLine("hitting /userprofiles/edit/id");
            if (id != userProfileVM.Id)
            {
                return NotFound();
            }

            UserProfile? userProfile = await _repository.GetAsync(userProfileVM.Id);

            if (userProfile == null)
            {
                return NotFound();
            }

          
            if (ModelState.IsValid)
            {
                Console.WriteLine("Model is valid");

                //if Image is provided, upload the image and change IconURL, else do nothing
                // Handle image upload
                if (userProfileVM.IconImg != null)
                {
                    Console.WriteLine("Uploading Img");
                    try
                    {
                        var result = await _imageService.UploadImageAsync(userProfileVM.IconImg);

                        if (result != null)
                        {
                            userProfile.IconURL = result.Url.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Image upload failed: {ex.Message}");
                        ModelState.AddModelError("", "Image upload failed. Please try again.");
                        return View(userProfileVM);
                    }
                    Console.WriteLine("Finished Img upload");
                }   

                //updates the profile

                userProfile.UserName = userProfileVM.UserName;
                userProfile.Biography = userProfileVM.Biography;

                await _repository.UpdateAsync(userProfile);


                return RedirectToAction(nameof(Index));
            }

            //For debugging model state

            //foreach (var state in ModelState)
            //{
            //    var key = state.Key;
            //    var errors = state.Value.Errors;

            //    if (errors.Count > 0)
            //    {
            //        foreach (var error in errors)
            //        {
            //            // Log or display the error
            //            Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
            //        }
            //    }
            //}

            //List<Interest> interests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();
            //ViewData["Interests"] = interests;

           

            return View(userProfileVM);
        }




    }
}
