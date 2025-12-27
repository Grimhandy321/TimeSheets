using System.Text.Json.Serialization;
using TimeSheets.Models.Enums;

namespace TimeSheets.Models
{
    public class Subject
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SubjectType { get; set; } = null!;
        [JsonIgnore]
        public SubjectType Type { get; set; }
        [JsonIgnore]
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
            = new List<TeacherSubject>();
    }
}
