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
        // 1. ����������� �� �����
        var allLessons = await _lessonRepo.GetAllAsync();

        // 2. Գ������� �� ���������, ���� �������� ��������
        var filtered = difficulty.HasValue
            ? allLessons.Where(l => l.Difficulty == difficulty.Value)
            : allLessons;

        // 3. �������� � ViewBag ������ ��� ����������� �� �������
        ViewBag.Difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
        ViewBag.Selected = difficulty;

        // 4. ��������� ViewModel
        var vm = new LessonListViewModel
        {
            Lessons = filtered.ToList()
        };

        return View(vm);
    }
}
