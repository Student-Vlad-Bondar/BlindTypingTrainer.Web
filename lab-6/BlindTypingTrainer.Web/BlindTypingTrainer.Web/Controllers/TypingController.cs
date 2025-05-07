using BlindTypingTrainer.Web.Services;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlindTypingTrainer.Web.Controllers
{
    public class TypingController : Controller
    {
        private readonly TypingService _svc;
        public TypingController(TypingService svc) => _svc = svc;

        public async Task<IActionResult> Index(int lessonId)
        {
            var session = await _svc.StartSessionAsync(lessonId);
            var lesson = (await _svc.StartSessionAsync(lessonId)).Lesson; // тут треба виправити — див. майбутній рефакторинг
            var vm = new TypingViewModel
            {
                SessionId = session.Id,
                Text = lesson.Text
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
