using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlindTypingTrainer.Web.Services
{
    public class TypingService
    {
        private readonly IRepository<Lesson> _lessonRepo;
        private readonly IRepository<TypingSession> _sessionRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TypingService(IRepository<Lesson> lessonRepo, IRepository<TypingSession> sessionRepo, IHttpContextAccessor httpContextAccessor)
        {
            _lessonRepo = lessonRepo;
            _sessionRepo = sessionRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TypingSession> StartSessionAsync(int lessonId)
        {
            var lesson = await _lessonRepo.GetByIdAsync(lessonId)
                      ?? throw new ArgumentException("Урок не знайдено");

            // Определяем текущего пользователя
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new InvalidOperationException("Користувач не аутентифікований");

            var session = new TypingSession
            {
                LessonId = lessonId,
                UserId = userId,
                StartTime = DateTime.Now
            };

            await _sessionRepo.AddAsync(session);
            return session;
        }

        public async Task EndSessionAsync(int sessionId, int correctChars, int errors)
        {
            var session = await _sessionRepo.GetByIdAsync(sessionId)
                          ?? throw new ArgumentException("Сесію не знайдено");

            session.EndTime = DateTime.Now;
            session.CorrectChars = correctChars;
            session.Errors = errors;

            await _sessionRepo.UpdateAsync(session);
        }
    }
}
