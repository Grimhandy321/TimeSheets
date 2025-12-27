namespace TimeSheets.Models.Views
{
    /// <summary>
    /// Represents classroom usage statistics.
    /// Matches SQL view: View_ClassroomUsage
    /// </summary>
    public class ClassroomUsageView
    {
        public string ClassroomName { get; set; } = null!;    // Classroom name
        public int UsageCount { get; set; }                   // Total number of lessons scheduled in the classroom
        public double TotalHoursUsed { get; set; }           // Total hours classroom is used
        public int GroupsCount { get; set; }                 // Number of distinct student groups using the classroom
        public int DistinctTeachers { get; set; }            // Number of distinct teachers using the classroom
    }
}
