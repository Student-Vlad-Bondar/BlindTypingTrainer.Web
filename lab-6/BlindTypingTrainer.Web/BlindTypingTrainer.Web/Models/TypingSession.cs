namespace BlindTypingTrainer.Web.Models
{
    public class TypingSession
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CorrectChars { get; set; }
        public int Errors { get; set; }

        public double Accuracy =>
            (CorrectChars + Errors) == 0 ? 0 :
            (double)CorrectChars / (CorrectChars + Errors) * 100;

        public double Wpm =>
            (EndTime <= StartTime) ? 0 :
            (CorrectChars / 5.0) / (EndTime - StartTime).TotalMinutes;
    }
}
