namespace TimeSheets.Models.Views
{
    public class TeacherLoadView
    {
        public string FullName { get; set; } = null!;
        public int LessonCount { get; set; }
        public DateTime FirstLesson { get; set; }
        public DateTime LastLesson { get; set; }
    }
}
