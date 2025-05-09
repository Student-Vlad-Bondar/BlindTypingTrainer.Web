using BlindTypingTrainer.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<TypingSession> TypingSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
