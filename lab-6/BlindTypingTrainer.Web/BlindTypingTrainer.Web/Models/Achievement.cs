using System.ComponentModel.DataAnnotations;

namespace BlindTypingTrainer.Web.Models
{
    /// <summary>
    /// Словник усіх можливих досягнень.
    /// </summary>
    public class Achievement
    {
        [Key]
        public int Id { get; set; }

        public AchievementType Type { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; } // шлях до іконки

        public DateTime CreatedAt { get; set; }
    }
}
