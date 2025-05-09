using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.ViewModels;

namespace BlindTypingTrainer.Web.Controllers;

public class HomeController : Controller
{
    private readonly IRepository<Models.Lesson> _lessonRepo;
    public HomeController(IRepository<Lesson> lessonRepo)
    {
        _lessonRepo = lessonRepo;
    }

    public async Task<IActionResult> Index(Difficulty? difficulty)
    {
        // 1. Завантажуємо всі уроки
        var allLessons = await _lessonRepo.GetAllAsync();

        // 2. Фільтруємо за складністю, якщо передано параметр
        var filtered = difficulty.HasValue
            ? allLessons.Where(l => l.Difficulty == difficulty.Value)
            : allLessons;

        // 3. Передаємо у ViewBag список усіх складностей та вибрану
        ViewBag.Difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
        ViewBag.Selected = difficulty;

        // 4. Створюємо ViewModel
        var vm = new LessonListViewModel
        {
            Lessons = filtered.ToList()
        };

        return View(vm);
    }
}
