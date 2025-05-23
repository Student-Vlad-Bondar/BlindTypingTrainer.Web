using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Helpers;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Interfaces;

namespace BlindTypingTrainer.Web.Services.AchievementHandlers.Implementations
{
    public abstract class BaseAchievementHandler : IAchievementHandler
    {
        private IAchievementHandler? _next;

        public async Task HandleAsync(TypingSession session, string userId)
        {
            await TryAwardAsync(session, userId);
            if (_next != null)
                await _next.HandleAsync(session, userId);
        }

        public IAchievementHandler? SetNext(IAchievementHandler next)
        {
            _next = next;
            return next;
        }

        protected async Task TryAwardAsync(TypingSession session, string userId)
        {
            var uaRepo = HttpContextHelper.RequestServices.GetService<IUserAchievementRepository>()!;
            var achRepo = HttpContextHelper.RequestServices.GetService<IAchievementRepository>()!;

            if (await uaRepo.HasAchievementAsync(userId, Type))
                return;

            if (IsSatisfied(session))
            {
                var ach = await achRepo.GetByTypeAsync(Type);
                if (ach != null)
                {
                    await uaRepo.AddAsync(new UserAchievement
                    {
                        UserId = userId,
                        AchievementId = ach.Id,
                        EarnedAt = DateTime.UtcNow
                    });
                }
            }
        }

        protected abstract AchievementType Type { get; }
        protected abstract bool IsSatisfied(TypingSession session);
    }
}
