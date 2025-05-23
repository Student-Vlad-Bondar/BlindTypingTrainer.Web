using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Implementations;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Handlers.ConcreteHandlers
{
    public class Accuracy95Handler : BaseAchievementHandler
    {
        protected override AchievementType Type => AchievementType.Accuracy95;
        protected override bool IsSatisfied(TypingSession session) => session.Accuracy >= 95;
    }
}
