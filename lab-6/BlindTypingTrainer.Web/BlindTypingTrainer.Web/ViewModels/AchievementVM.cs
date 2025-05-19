namespace BlindTypingTrainer.Web.ViewModels
{
    public class AchievementVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public DateTime EarnedAt { get; set; }
    }

    public class AchievementListVM
    {
        public List<AchievementVM> Achievements { get; set; }
    }
}
