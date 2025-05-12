using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlindTypingTrainer.Web.Services
{
    public class TypingService
    {
        private readonly IReadRepository<Lesson> _lessonRead;
        private readonly IReadRepository<TypingSession> _sessionRead;
        private readonly IWriteRepository<TypingSession> _sessionWrite;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TypingService(
            IReadRepository<Lesson> lessonRead,
            IReadRepository<TypingSession> sessionRead,
            IWriteRepository<TypingSession> sessionWrite,
            IHttpContextAccessor httpContextAccessor)
        {
            _lessonRead = lessonRead;
            _sessionRead = sessionRead;
            _sessionWrite = sessionWrite;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TypingSession> StartSessionAsync(int lessonId)
        {
            // 1) Load lesson
            var lesson = await _lessonRead.GetByIdAsync(lessonId)
                      ?? throw new ArgumentException("Урок не знайдено");

            // 2) Get current user
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new InvalidOperationException("Користувач не аутентифікований");

            // 3) Create new session
            var session = new TypingSession
            {
                LessonId = lessonId,
                UserId = userId,
                StartTime = DateTime.Now
            };

            // 4) Persist
            await _sessionWrite.AddAsync(session);
            return session;
        }

        public async Task EndSessionAsync(int sessionId, int correctChars, int errors)
        {
            // 1) Load existing session
            var session = await _sessionRead.GetByIdAsync(sessionId)
                          ?? throw new ArgumentException("Сесію не знайдено");

            // 2) Update
            session.EndTime = DateTime.Now;
            session.CorrectChars = correctChars;
            session.Errors = errors;

            // 3) Persist changes
            await _sessionWrite.UpdateAsync(session);
        }
    }
}
