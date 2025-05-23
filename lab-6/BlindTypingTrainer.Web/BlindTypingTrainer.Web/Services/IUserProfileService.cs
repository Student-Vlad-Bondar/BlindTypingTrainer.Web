using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.ViewModels;

namespace BlindTypingTrainer.Web.Services
{
    public interface IUserProfileService
    {
        Task<(bool Success, IEnumerable<string> Errors, bool NeedSignIn)> UpdateUserProfileAsync(ApplicationUser user, EditProfileVM vm);
    }
}
