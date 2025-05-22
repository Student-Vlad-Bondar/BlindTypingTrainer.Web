using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Helpers;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Implementations;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Handlers.ConcreteHandlers
{
    public class MarathonHandler : BaseAchievementHandler
    {
        protected override AchievementType Type => AchievementType.Marathon;
        protected override bool IsSatisfied(TypingSession session)
        {
            var uaRepo = HttpContextHelper.RequestServices.GetService<IUserAchievementRepository>()!;
            var count = uaRepo.GetByUserAsync(session.UserId).Result.Count();
            return count >= 10;
        }
    }

}
