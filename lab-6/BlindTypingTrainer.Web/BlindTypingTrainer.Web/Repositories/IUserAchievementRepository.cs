using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Repositories
{
    public interface IUserAchievementRepository
    {
        Task<IEnumerable<UserAchievement>> GetByUserAsync(string userId);
        Task<bool> HasAchievementAsync(string userId, AchievementType type);
        Task AddAsync(UserAchievement ua);
    }
}
