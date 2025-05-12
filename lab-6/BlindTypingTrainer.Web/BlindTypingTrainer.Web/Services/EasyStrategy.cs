using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Services
{
    public class EasyStrategy : ILessonFilterStrategy
    {
        public Difficulty StrategyDifficulty => Difficulty.Easy;
        public bool IsMatch(Lesson lesson) => lesson.Difficulty == Difficulty.Easy;
    }
}
