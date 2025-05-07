using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.ViewModels;

namespace BlindTypingTrainer.Web.Controllers;

public class HomeController : Controller
{
    private readonly IRepository<Models.Lesson> _lessonRepo;
    public HomeController(IRepository<Models.Lesson> lr) => _lessonRepo = lr;

    public async Task<IActionResult> Index()
    {
        var vm = new LessonListViewModel
        {
            Lessons = await _lessonRepo.GetAllAsync()
        };
        return View(vm);
    }
}
