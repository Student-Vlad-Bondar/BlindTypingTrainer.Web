using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;

namespace BlindTypingTrainer.Web.Services
{
    public class TypingService
    {
        private readonly IRepository<Lesson> _lessonRepo;
        private readonly IRepository<TypingSession> _sessionRepo;

        public TypingService(IRepository<Lesson> lr, IRepository<TypingSession> sr)
        {
            _lessonRepo = lr;
            _sessionRepo = sr;
        }

        public async Task<TypingSession> StartSessionAsync(int lessonId)
        {
            var lesson = await _lessonRepo.GetByIdAsync(lessonId)
                ?? throw new ArgumentException("Урок не знайдено");
            var session = new TypingSession
            {
                LessonId = lessonId,
                StartTime = DateTime.Now
            };
            await _sessionRepo.AddAsync(session);
            return session;
        }

        public async Task EndSessionAsync(int sessionId, int correctChars, int errors)
        {
            var s = await _sessionRepo.GetByIdAsync(sessionId)
                ?? throw new ArgumentException("Сесію не знайдено");
            s.EndTime = DateTime.Now;
            s.CorrectChars = correctChars;
            s.Errors = errors;
            await _sessionRepo.UpdateAsync(s);
        }
    }
}
