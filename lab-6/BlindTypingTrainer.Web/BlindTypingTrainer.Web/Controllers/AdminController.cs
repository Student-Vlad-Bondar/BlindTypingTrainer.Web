using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlindTypingTrainer.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository<Lesson> _lessonRepo;
        public AdminController(IRepository<Lesson> lr) => _lessonRepo = lr;

        public async Task<IActionResult> Index()
        {
            var lessons = await _lessonRepo.GetAllAsync();
            return View(lessons);
        }

        public IActionResult Create() => View(new Lesson());

        [HttpPost]
        public async Task<IActionResult> Create(Lesson model)
        {
            if (!ModelState.IsValid) return View(model);
            await _lessonRepo.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonRepo.GetByIdAsync(id);
            return lesson == null ? NotFound() : View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lesson model)
        {
            if (!ModelState.IsValid) return View(model);
            await _lessonRepo.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _lessonRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
