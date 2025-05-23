using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlindTypingTrainer.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sm;
        private readonly IUserProfileService _profileService;

        public ProfileController(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm, IUserProfileService profileService)
        {
            _um = um;
            _sm = sm;
            _profileService = profileService;
        }

        [HttpGet]
        [Route("/Account/Profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _um.Users
              .Include(u => u.Sessions)
                .ThenInclude(s => s.Lesson)
              .SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var completed = user.Sessions
              .Where(s => s.EndTime.HasValue)
              .OrderByDescending(s => s.EndTime);

            var vm = new ProfileVM
            {
                UserName = user.UserName,
                Sessions = completed
            };

            return View(vm);
        }

        [HttpGet]
        [Route("/Account/EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _um.GetUserAsync(User);
            if (user == null) return NotFound();

            var vm = new EditProfileVM
            {
                UserName = user.UserName,
                Email = user.Email
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Account/EditProfile")]
        public async Task<IActionResult> EditProfile(EditProfileVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _um.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var (success, errors, needSignIn) = await _profileService.UpdateUserProfileAsync(user, vm);

            if (!success)
            {
                foreach (var error in errors)
                    ModelState.AddModelError("", error);
                return View(vm);
            }

            if (needSignIn)
                await _sm.RefreshSignInAsync(user);

            TempData["StatusMessage"] = "Дані профілю успішно оновлено";
            return RedirectToAction(nameof(EditProfile));
        }
    }
}
