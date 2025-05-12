using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Services
{
    public class HardStrategy : ILessonFilterStrategy
    {
        public Difficulty StrategyDifficulty => Difficulty.Hard;
        public bool IsMatch(Lesson lesson) => lesson.Difficulty == Difficulty.Hard;
    }
}
