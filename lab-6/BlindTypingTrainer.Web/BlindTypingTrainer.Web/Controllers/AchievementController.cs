using BlindTypingTrainer.Web.Models;
using BlindTypingTrainer.Web.Repositories;
using BlindTypingTrainer.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlindTypingTrainer.Web.Controllers
{
    [Authorize]
    public class AchievementController : Controller
    {
        private readonly IUserAchievementRepository _uaRepo;

        public AchievementController(IUserAchievementRepository uaRepo)
        {
            _uaRepo = uaRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _uaRepo.GetByUserAsync(userId);
            var vm = new AchievementListVM
            {
                Achievements = list.Select(ua => new AchievementVM
                {
                    Title = ua.Achievement.Title,
                    Description = ua.Achievement.Description,
                    IconUrl = ua.Achievement.IconUrl,
                    EarnedAt = ua.EarnedAt
                }).ToList()
            };
            return View(vm);
        }
    }
}
