using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Services
{
    public class VeryHardStrategy : ILessonFilterStrategy
    {
        public Difficulty StrategyDifficulty => Difficulty.VeryHard;
        public bool IsMatch(Lesson lesson) => lesson.Difficulty == Difficulty.VeryHard;
    }
}
