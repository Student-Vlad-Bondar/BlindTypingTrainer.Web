using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlindTypingTrainer.Web.Models
{
    /// <summary>
    /// Досягнення, яке здобув користувач.
    /// </summary>
    public class UserAchievement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [Required]
        public int AchievementId { get; set; }

        [ForeignKey(nameof(AchievementId))]
        public Achievement Achievement { get; set; }

        public DateTime EarnedAt { get; set; }
    }
}
