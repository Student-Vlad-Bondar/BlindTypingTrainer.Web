using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using System.Security.Claims;

namespace BlindTypingTrainer.Web.Services
{
    /// <summary>
    /// Логіка перевірки та нарахування досягнень.
    /// Викликається після завершення сесії.
    /// </summary>
    public class AchievementService
    {
        private readonly IAchievementRepository _achRepo;
        private readonly IUserAchievementRepository _uaRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AchievementService(
            IAchievementRepository achRepo,
            IUserAchievementRepository uaRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _achRepo = achRepo;
            _uaRepo = uaRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Перевірити й нарахувати всі релевантні досягнення для цієї сесії.
        /// </summary>
        public async Task CheckAndAwardAsync(TypingSession session)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return;

            // 1) Перший урок
            if (session.LessonId == 1)
                await AwardOnceAsync(userId, AchievementType.FirstLesson);

            // 2) 100 правильних символів
            if (session.CorrectChars >= 100)
                await AwardOnceAsync(userId, AchievementType.HundredWords);

            // 3) 95% точність
            if (session.Accuracy >= 95)
                await AwardOnceAsync(userId, AchievementType.Accuracy95);

            // 4) WPM >= 50
            if (session.Wpm >= 50)
                await AwardOnceAsync(userId, AchievementType.Speed50Wpm);

            // 5) Марафон: 10 сесій поспіль без пропусків
            // (спрощено: перевірити, що в БД є >=10 попередніх завершених)
            var prev = await _uaRepo.GetByUserAsync(userId); // тут можна розширити
            if (prev.Count() >= 10)
                await AwardOnceAsync(userId, AchievementType.Marathon);
        }

        private async Task AwardOnceAsync(string userId, AchievementType type)
        {
            if (await _uaRepo.HasAchievementAsync(userId, type))
                return; // вже має

            var ach = await _achRepo.GetByTypeAsync(type);
            if (ach == null) return;

            await _uaRepo.AddAsync(new UserAchievement
            {
                UserId = userId,
                AchievementId = ach.Id,
                EarnedAt = DateTime.Now
            });
        }
    }
}
