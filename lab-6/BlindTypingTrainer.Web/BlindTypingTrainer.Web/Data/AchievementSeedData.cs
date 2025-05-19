using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Data
{
    public static class AchievementSeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Achievements.Any())
                return;

            var now = DateTime.UtcNow;
            var list = new List<Achievement>
            {
                new Achievement { Type = AchievementType.FirstLesson, Title = "Перший урок",
                    Description = "Пройдіть перший урок", IconUrl="/images/achievements/first_lesson.jfif", CreatedAt=now },
                new Achievement { Type = AchievementType.HundredWords, Title = "100 слів",
                    Description = "Наберіть 100 правильних символів", IconUrl="/images/achievements/100_words.png", CreatedAt=now },
                new Achievement { Type = AchievementType.Accuracy95, Title = "Точність 95%",
                    Description = "Досягніть 95% точності", IconUrl="/images/achievements/accuracy_95.jpg", CreatedAt=now },
                new Achievement { Type = AchievementType.Speed50Wpm, Title = "50 слів/хв",
                    Description = "Досягніть 50 слів за хвилину", IconUrl="/images/achievements/speed_50.jfif", CreatedAt=now },
                new Achievement { Type = AchievementType.Marathon, Title = "Марафон",
                    Description = "Пройдіть 10 уроків поспіль", IconUrl="/images/achievements/marathon.jfif", CreatedAt=now },
            };

            context.Achievements.AddRange(list);
            context.SaveChanges();
        }
    }
}
