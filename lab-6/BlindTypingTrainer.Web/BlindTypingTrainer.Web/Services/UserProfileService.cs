using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BlindTypingTrainer.Web.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool Success, IEnumerable<string> Errors, bool NeedSignIn)> UpdateUserProfileAsync(ApplicationUser user, EditProfileVM vm)
        {
            var errors = new List<string>();
            bool needSignIn = false;

            if (vm.UserName != user.UserName)
            {
                user.UserName = vm.UserName;
                needSignIn = true;
            }

            if (vm.Email != user.Email)
            {
                user.Email = vm.Email;
                user.EmailConfirmed = false;
                needSignIn = true;
            }

            var updateRes = await _userManager.UpdateAsync(user);
            if (!updateRes.Succeeded)
                errors.AddRange(updateRes.Errors.Select(e => e.Description));

            if (!string.IsNullOrEmpty(vm.CurrentPassword) || !string.IsNullOrEmpty(vm.NewPassword))
            {
                if (string.IsNullOrEmpty(vm.CurrentPassword) || string.IsNullOrEmpty(vm.NewPassword))
                {
                    errors.Add("Щоб змінити пароль, заповніть обидва поля.");
                }
                else
                {
                    var pwRes = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
                    if (!pwRes.Succeeded)
                        errors.AddRange(pwRes.Errors.Select(e => e.Description));
                    else
                        needSignIn = true;
                }
            }

            return (errors.Count == 0, errors, needSignIn);
        }
    }
}
