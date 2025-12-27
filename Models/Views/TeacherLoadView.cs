namespace TimeSheets.Models.Views
{
    /// <summary>
    /// Represents aggregated teaching workload per teacher.
    /// Matches SQL view: View_TeacherLoad
    /// </summary>
    public class TeacherLoadView
    {
        public string TeacherName { get; set; } = null!;       // Teacher's full name
        public int LessonCount { get; set; }                   // Total number of lessons assigned
        public double TotalHours { get; set; }                // Total hours taught
        public double AvgLessonMinutes { get; set; }          // Average lesson duration in minutes
        public DateTime FirstLesson { get; set; }             // Earliest lesson start time
        public DateTime LastLesson { get; set; }              // Latest lesson end time
        public int DistinctGroupsTaught { get; set; }         // Number of distinct student groups taught
        public int DistinctClassroomsUsed { get; set; }       // Number of distinct classrooms used
    }
}
