using System.Text.Json.Serialization;

namespace TimeSheets.Models
{
    public class TimetableEntry
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;

        public int StudentGroupId { get; set; }
        public StudentGroup StudentGroup { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
