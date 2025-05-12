using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Services
{
    public interface ILessonFilterStrategy
    {
        bool IsMatch(Lesson lesson);
        Difficulty StrategyDifficulty { get; }
    }
}
