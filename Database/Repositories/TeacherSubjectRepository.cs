using Microsoft.Data.SqlClient;
using TimeSheets.Models;
using TimeSheets.Models.Enums;

namespace TimeSheets.Database.Repositories
{  
    public class TeacherSubjectRepository : SqlRepositoryBase
    {
        public TeacherSubjectRepository(IConfiguration config, DatabaseContext db) : base(config, db) { }

        public void Assign(int teacherId, int subjectId)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
            INSERT INTO TeacherSubjects (TeacherId, SubjectId)
            VALUES (@t, @s)
        """, c);

            cmd.Parameters.AddWithValue("@t", teacherId);
            cmd.Parameters.AddWithValue("@s", subjectId);

            cmd.ExecuteNonQuery();
        }

        public void Remove(int teacherId, int subjectId)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                DELETE FROM TeacherSubjects
                    WHERE TeacherId=@t AND SubjectId=@s
                """, c);

            cmd.Parameters.AddWithValue("@t", teacherId);
            cmd.Parameters.AddWithValue("@s", subjectId);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Subject> GetSubjectsForTeacher(int teacherId)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                SELECT s.Id, s.Name, s.Type
                    FROM Subjects s
                JOIN TeacherSubjects ts ON ts.SubjectId = s.Id
                WHERE ts.TeacherId=@t
                """, c);

            cmd.Parameters.AddWithValue("@t", teacherId);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new Subject
                {
                    Id = Int(r, "Id"),
                    Name = Str(r, "Name"),
                    SubjectType = EnumIntToString<SubjectType>(Int(r, "Type")),
                };
            }
        }
    }

}
