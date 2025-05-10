using Microsoft.AspNetCore.Identity;

namespace BlindTypingTrainer.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Навігаційна властивість для історії проходжень
        public ICollection<TypingSession> Sessions { get; set; } = new List<TypingSession>();
    }
}
