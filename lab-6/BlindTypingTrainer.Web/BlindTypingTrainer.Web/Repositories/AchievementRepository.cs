using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Repositories
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly AppDbContext _db;
        public AchievementRepository(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Achievement>> GetAllAsync() =>
            await _db.Achievements.ToListAsync();

        public async Task<Achievement> GetByTypeAsync(AchievementType type) =>
            await _db.Achievements.FirstOrDefaultAsync(a => a.Type == type);
    }
}
