using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Services
{
    public class MediumStrategy : ILessonFilterStrategy
    {
        public Difficulty StrategyDifficulty => Difficulty.Medium;
        public bool IsMatch(Lesson lesson) => lesson.Difficulty == Difficulty.Medium;
    }
}
