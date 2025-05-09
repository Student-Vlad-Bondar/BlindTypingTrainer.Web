namespace BlindTypingTrainer.Web.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
