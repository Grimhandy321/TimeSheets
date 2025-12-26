namespace TimeSheets.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public float Salary { get; set; }                 // float

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
            = new List<TeacherSubject>();
    }
}
