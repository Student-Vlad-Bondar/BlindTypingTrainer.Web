using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Implementations;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Handlers.ConcreteHandlers
{
    public class Speed50WpmHandler : BaseAchievementHandler
    {
        protected override AchievementType Type => AchievementType.Speed50Wpm;
        protected override bool IsSatisfied(TypingSession session) => session.Wpm >= 50;
    }
}
