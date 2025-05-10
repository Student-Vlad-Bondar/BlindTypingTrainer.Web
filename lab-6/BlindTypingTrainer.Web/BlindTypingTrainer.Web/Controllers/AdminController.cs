using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindTypingTrainer.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<Lesson> _lessonRepo;
        public AdminController(IRepository<Lesson> lr) => _lessonRepo = lr;

        public async Task<IActionResult> Index()
        {
            var lessons = await _lessonRepo.GetAllAsync();
            return View(lessons);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            // Просто отдаём пустую модель — в форме по-умолчанию выберется первый элемент enum
            return View(new Lesson());
        }

        // POST: Admin/Create
        [HttpPost]
        public async Task<IActionResult> Create(Lesson model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _lessonRepo.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonRepo.GetByIdAsync(id);
            if (lesson == null)
                return NotFound();

            return View(lesson);
        }

        // POST: Admin/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Lesson model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _lessonRepo.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _lessonRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
