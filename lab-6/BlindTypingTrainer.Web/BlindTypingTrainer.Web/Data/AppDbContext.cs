using BlindTypingTrainer.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlindTypingTrainer.Web.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<TypingSession> TypingSessions { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // AspNetRoles
            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.Id).HasMaxLength(50);
                entity.Property(r => r.Name).HasMaxLength(50);
                entity.Property(r => r.NormalizedName).HasMaxLength(50);
            });

            // AspNetUsers (ApplicationUser)
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.Id).HasMaxLength(50);
                entity.Property(u => u.UserName).HasMaxLength(50);
                entity.Property(u => u.NormalizedUserName).HasMaxLength(50);
                entity.Property(u => u.Email).HasMaxLength(50);
                entity.Property(u => u.NormalizedEmail).HasMaxLength(50);
            });

            // AspNetUserLogins: ключі LoginProvider + ProviderKey
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(l => l.LoginProvider).HasMaxLength(50);
                entity.Property(l => l.ProviderKey).HasMaxLength(50);
            });

            // AspNetUserTokens: ключі LoginProvider + Name
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(t => t.LoginProvider).HasMaxLength(50);
                entity.Property(t => t.Name).HasMaxLength(50);
            });

            // Налаштування для нових сутностей:
            builder.Entity<Achievement>()
                .Property(a => a.IconUrl)
                .HasMaxLength(256);

            builder.Entity<UserAchievement>()
                .HasIndex(ua => new { ua.UserId, ua.AchievementId })
                .IsUnique(); // унікальне досягнення на користувача
        }
    }
}
