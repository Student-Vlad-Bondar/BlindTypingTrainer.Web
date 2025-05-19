using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Repositories
{
    public interface IAchievementRepository
    {
        Task<IEnumerable<Achievement>> GetAllAsync();
        Task<Achievement> GetByTypeAsync(AchievementType type);
    }
}
