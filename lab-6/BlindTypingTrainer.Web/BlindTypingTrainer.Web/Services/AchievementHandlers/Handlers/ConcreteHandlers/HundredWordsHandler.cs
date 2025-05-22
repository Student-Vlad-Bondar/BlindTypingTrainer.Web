using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Implementations;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Handlers.ConcreteHandlers
{
    public class HundredWordsHandler : BaseAchievementHandler
    {
        protected override AchievementType Type => AchievementType.HundredWords;
        protected override bool IsSatisfied(TypingSession session) => session.CorrectChars >= 100;
    }
}
