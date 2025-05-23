// Файл: Services/AchievementService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Services.AchievementHandlers.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BlindTypingTrainer.Web.Services
{

    public class AchievementService
    {
        private readonly IAchievementHandler _chain;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AchievementService(
            IEnumerable<IAchievementHandler> handlers,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var ordered = handlers?.ToList()
                ?? throw new ArgumentNullException(nameof(handlers));
            if (!ordered.Any())
                throw new ArgumentException("Не знайдено жодного IAchievementHandler", nameof(handlers));

            _chain = ordered[0];
            for (int i = 1; i < ordered.Count; i++)
            {
                ordered[i - 1].SetNext(ordered[i]);
            }
        }
        public async Task CheckAndAwardAsync(TypingSession session)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return;

            await _chain.HandleAsync(session, userId);
        }
    }
}
