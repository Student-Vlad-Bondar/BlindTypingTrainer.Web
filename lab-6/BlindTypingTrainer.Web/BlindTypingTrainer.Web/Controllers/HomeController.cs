using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.ViewModels;
using BlindTypingTrainer.Web.Services;

namespace BlindTypingTrainer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReadRepository<Lesson> _lessonRepo;
        private readonly IEnumerable<ILessonFilterStrategy> _filterStrategies;

        public HomeController(
            IReadRepository<Lesson> lessonRepo,
            IEnumerable<ILessonFilterStrategy> filterStrategies)
        {
            _lessonRepo = lessonRepo;
            _filterStrategies = filterStrategies;
        }

        public async Task<IActionResult> Index(Difficulty? difficulty)
        {
            var allLessons = await _lessonRepo.GetAllAsync();

            // apply Strategy if difficulty selected
            var filtered = difficulty.HasValue
                ? allLessons.Where(l =>
                {
                    var strat = _filterStrategies
                        .First(s => s.StrategyDifficulty == difficulty.Value);
                    return strat.IsMatch(l);
                })
                : allLessons;

            ViewBag.Difficulties = Enum.GetValues(typeof(Difficulty)) as Difficulty[];
            ViewBag.Selected = difficulty;

            var vm = new LessonListViewModel
            {
                Lessons = filtered.ToList()
            };

            return View(vm);
        }
    }
}
