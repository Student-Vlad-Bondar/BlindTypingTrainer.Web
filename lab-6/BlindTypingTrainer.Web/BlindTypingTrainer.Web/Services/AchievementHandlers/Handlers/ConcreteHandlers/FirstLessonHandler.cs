using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Implementations;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Handlers.ConcreteHandlers
{
    public class FirstLessonHandler : BaseAchievementHandler
    {
        protected override AchievementType Type => AchievementType.FirstLesson;
        protected override bool IsSatisfied(TypingSession session) => session.LessonId == 1;
    }
}
