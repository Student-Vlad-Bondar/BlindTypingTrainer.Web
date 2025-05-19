using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Перевіряємо, чи вже є дані
            if (context.Lessons.Any())
                return;

            var lessons = new List<Lesson>
            {
                new Lesson
                {
                    Title = "Природа",
                    Difficulty = Difficulty.Easy,
                    Text = string.Join("|", new[]
                    {
                        "Сонце світить",
                        "Пташки співають",
                        "Дерева шумлять",
                        "Квіти пахнуть",
                        "Трава зелена"
                    })
                },
                new Lesson
                {
                    Title = "Місто",
                    Difficulty = Difficulty.Easy,
                    Text = string.Join("|", new[]
                    {
                        "Машини їдуть",
                        "Люди поспішають",
                        "Діти граються",
                        "Трамвай дзвенить",
                        "Магазини відкриті"
                    })
                },
                new Lesson
                {
                    Title = "Школа",
                    Difficulty = Difficulty.Easy,
                    Text = string.Join("|", new[]
                    {
                        "Учні навчаються",
                        "Вчитель пояснює",
                        "Зошити відкриті",
                        "Дзвінок лунає",
                        "Урок закінчився"
                    })
                }
            };

            context.Lessons.AddRange(lessons);

            context.SaveChanges();
        }
    }
}
