using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Repositories
{
    public class UserAchievementRepository : IUserAchievementRepository
    {
        private readonly AppDbContext _db;
        public UserAchievementRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<UserAchievement>> GetByUserAsync(string userId) =>
            await _db.UserAchievements
                     .Include(ua => ua.Achievement)
                     .Where(ua => ua.UserId == userId)
                     .OrderByDescending(ua => ua.EarnedAt)
                     .ToListAsync();

        public async Task<bool> HasAchievementAsync(string userId, AchievementType type) =>
            await _db.UserAchievements
                     .Include(ua => ua.Achievement)
                     .AnyAsync(ua => ua.UserId == userId && ua.Achievement.Type == type);

        public async Task AddAsync(UserAchievement ua)
        {
            _db.UserAchievements.Add(ua);
            await _db.SaveChangesAsync();
        }
    }
}
