using BlindTypingTrainer.Web.Data;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Repositories
{
    public class LessonRepository : IReadRepository<Lesson>, IWriteRepository<Lesson>
    {
        private readonly AppDbContext _db;
        public LessonRepository(AppDbContext db) => _db = db;

        // IReadRepository
        public async Task<IEnumerable<Lesson>> GetAllAsync() =>
            await _db.Lessons.ToListAsync();

        public async Task<Lesson?> GetByIdAsync(int id) =>
            await _db.Lessons.FindAsync(id);

        // IWriteRepository
        public async Task AddAsync(Lesson entity)
        {
            await _db.Lessons.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lesson entity)
        {
            _db.Lessons.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.Lessons.FindAsync(id);
            if (e != null)
            {
                _db.Lessons.Remove(e);
                await _db.SaveChangesAsync();
            }
        }
    }
}
