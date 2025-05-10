namespace BlindTypingTrainer.Web.Models
{
    public class TypingSession
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CorrectChars { get; set; }
        public int Errors { get; set; }

        public double Accuracy =>
            (CorrectChars + Errors) == 0
                ? 0
                : Math.Round((double)CorrectChars / (CorrectChars + Errors) * 100, 1);

        public double Wpm =>
            (EndTime == null || EndTime <= StartTime)
                ? 0
                : Math.Round((CorrectChars / 5.0) / (EndTime.Value - StartTime).TotalMinutes, 1);

    }
}
