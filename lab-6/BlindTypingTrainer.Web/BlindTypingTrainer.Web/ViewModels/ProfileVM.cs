using BlindTypingTrainer.Web.Models;

namespace BlindTypingTrainer.Web.ViewModels
{
    public class ProfileVM
    {
        public string UserName { get; set; }
        public IEnumerable<TypingSession> Sessions { get; set; }
    }
}
