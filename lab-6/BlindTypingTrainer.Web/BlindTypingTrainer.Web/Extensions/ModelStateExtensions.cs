using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlindTypingTrainer.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddIdentityErrors(this ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
                modelState.AddModelError(string.Empty, error.Description);
        }
    }
}
