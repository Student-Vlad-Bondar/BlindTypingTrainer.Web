using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.Services;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindTypingTrainer.Web.Controllers
{
    [Authorize]
    public class TypingController : Controller
    {
        private readonly TypingService _svc;
        private readonly IRepository<Lesson> _lessonRepo;

        public TypingController(TypingService svc, IRepository<Lesson> lessonRepo)
        {
            _svc = svc;
            _lessonRepo = lessonRepo;
        }

        public async Task<IActionResult> Index(int lessonId)
        {
            var session = await _svc.StartSessionAsync(lessonId);
            var lesson = await _lessonRepo.GetByIdAsync(lessonId) ?? throw new ArgumentException("Урок не знайдено");
            if (lesson == null)
                return NotFound();

            var vm = new TypingViewModel
            {
                SessionId = session.Id,
                Stages = lesson.Text.Split('|').ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int sessionId, int correct, int errors)
        {
            await _svc.EndSessionAsync(sessionId, correct, errors);
            return RedirectToAction("Index", "Home");
        }
    }
}
