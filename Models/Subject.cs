using TimeSheets.Models.Enums;

namespace TimeSheets.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public SubjectType Type { get; set; } 

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
            = new List<TeacherSubject>();
    }
}
