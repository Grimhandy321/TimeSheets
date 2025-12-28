using System.Text.Json.Serialization;

namespace TimeSheets.Models
{
    public class Teacher
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public float Salary { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<Subject>? Subjects { get; set; } = null;

        [JsonIgnore]
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
            = new List<TeacherSubject>();
    }
}
