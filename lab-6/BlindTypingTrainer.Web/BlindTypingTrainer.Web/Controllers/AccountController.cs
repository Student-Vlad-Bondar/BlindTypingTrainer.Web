using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sm;
        private readonly IUserProfileService _profileService;

        public AccountController(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm, IUserProfileService profileService)
        {
            _um = um;
            _sm = sm;
            _profileService = profileService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = new ApplicationUser { UserName = vm.UserName, Email = vm.Email };
            var res = await _um.CreateAsync(user, vm.Password);
            if (!res.Succeeded)
            {
                AddErrors(res);
                return View(vm);
            }
            await _sm.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) => View(new LoginVM { ReturnUrl = returnUrl });

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var res = await _sm.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);
            if (!res.Succeeded)
            {
                ModelState.AddModelError("", "Невірні дані");
                return View(vm);
            }
            return LocalRedirect(vm.ReturnUrl ?? "/");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        [Authorize]
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

        [Authorize]
        [HttpGet]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}