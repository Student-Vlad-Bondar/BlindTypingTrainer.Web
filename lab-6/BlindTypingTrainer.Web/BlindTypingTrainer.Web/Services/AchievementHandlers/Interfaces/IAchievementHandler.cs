using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Interfaces
{
    public interface IAchievementHandler
    {
        Task HandleAsync(TypingSession session, string userId);
        IAchievementHandler? SetNext(IAchievementHandler next);
    }
}
