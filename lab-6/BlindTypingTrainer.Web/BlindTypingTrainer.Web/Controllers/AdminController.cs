using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindTypingTrainer.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IReadRepository<Lesson> _readRepo;
        private readonly IWriteRepository<Lesson> _writeRepo;

        public AdminController(
            IReadRepository<Lesson> readRepo,
            IWriteRepository<Lesson> writeRepo)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var lessons = await _readRepo.GetAllAsync();
            return View(lessons);
        }

        public IActionResult Create() => View(new Lesson());

        [HttpPost]
        public async Task<IActionResult> Create(Lesson model)
        {
            if (!ModelState.IsValid) return View(model);
            await _writeRepo.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _readRepo.GetByIdAsync(id);
            return lesson == null ? NotFound() : View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lesson model)
        {
            if (!ModelState.IsValid) return View(model);
            await _writeRepo.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _writeRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
