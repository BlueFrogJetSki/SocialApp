using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Enum;
using SocialApp.Interfaces;
using SocialApp.Models;
using SocialApp.ViewModel;

namespace SocialApp.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly IUserProfileRepository _repository;

        public UserProfilesController(IUserProfileRepository repository)
        {
            _repository = repository;
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

            List<Interest> interests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();

            ViewData["Interests"] = interests;
            return View(userProfileVM);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Biography,Major,Interests")] UserProfile userProfile, List<string> selectedInterests)
        {
            if (id != userProfile.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                userProfile.Interests = selectedInterests
                .Select(i => Enum.TryParse<Interest>(i, out var interest) ? (Interest?)interest : null)
                .Where(i => i.HasValue)
                .Select(i => i.Value)
                .ToList();

                try
                {
                    await _repository.UpdateAsync(userProfile);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repository.UserProfileExists(userProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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

            List<Interest> interests = Enum.GetValues(typeof(Interest)).Cast<Interest>().ToList();
            ViewData["Interests"] = interests;

            var userProfileVM = new UserProfileVM(userProfile);

            return View(userProfileVM);
        }



       
    }
}
