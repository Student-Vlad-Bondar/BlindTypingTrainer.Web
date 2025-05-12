using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Repositories
{
    public class TypingSessionRepository : IReadRepository<TypingSession>, IWriteRepository<TypingSession>
    {
        private readonly AppDbContext _db;
        public TypingSessionRepository(AppDbContext db) => _db = db;

        // IReadRepository
        public async Task<IEnumerable<TypingSession>> GetAllAsync() =>
            await _db.TypingSessions.Include(s => s.Lesson).ToListAsync();

        public async Task<TypingSession?> GetByIdAsync(int id) =>
            await _db.TypingSessions
                     .Include(s => s.Lesson)
                     .FirstOrDefaultAsync(s => s.Id == id);

        // IWriteRepository
        public async Task AddAsync(TypingSession entity)
        {
            await _db.TypingSessions.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TypingSession entity)
        {
            _db.TypingSessions.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var session = await _db.TypingSessions.FindAsync(id);
            if (session != null)
            {
                _db.TypingSessions.Remove(session);
                await _db.SaveChangesAsync();
            }
        }
    }
}
