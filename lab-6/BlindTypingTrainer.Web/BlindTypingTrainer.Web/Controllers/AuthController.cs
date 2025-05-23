using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlindTypingTrainer.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sm;

        public AuthController(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm)
        {
            _um = um;
            _sm = sm;
        }

        [HttpGet]
        [Route("/Account/Register")]
        public IActionResult Register() => View();

        [HttpPost]
        [Route("/Account/Register")]
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
        [Route("/Account/Login")]
        public IActionResult Login(string returnUrl = null) => View(new LoginVM { ReturnUrl = returnUrl });

        [HttpPost]
        [Route("/Account/Login")]
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

        [HttpPost]
        [Route("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/Account/AccessDenied")]
        public IActionResult AccessDenied() => View();

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
