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

        public void ClearForTeacher(SqlConnection c,SqlTransaction tx,int teacherId)
        {
            using var cmd = new SqlCommand("""
                DELETE FROM TeacherSubjects
                    WHERE TeacherId = @t
                """, c, tx);

            cmd.Parameters.AddWithValue("@t", teacherId);
            cmd.ExecuteNonQuery();
        }

        public void Assign(SqlConnection c,SqlTransaction tx,int teacherId,int subjectId)
        {
            using var cmd = new SqlCommand("""
                INSERT INTO TeacherSubjects (TeacherId, SubjectId)
                    VALUES (@t, @s)
             """, c, tx);

            cmd.Parameters.AddWithValue("@t", teacherId);
            cmd.Parameters.AddWithValue("@s", subjectId);
            cmd.ExecuteNonQuery();
        }

        public HashSet<int> GetSubjectIdsForTeacher(SqlConnection c,SqlTransaction tx,int teacherId)
        {
            using var cmd = new SqlCommand("""
                SELECT SubjectId
                    FROM TeacherSubjects
                WHERE TeacherId = @t
            """, c, tx);

            cmd.Parameters.AddWithValue("@t", teacherId);

            using var r = cmd.ExecuteReader();
            var result = new HashSet<int>();

            while (r.Read())
                result.Add(Int(r, "SubjectId"));
            return result;
        }

        public void Remove(SqlConnection c,SqlTransaction tx, int teacherId, int subjectId)
        {
            using var cmd = new SqlCommand("""
                DELETE FROM TeacherSubjects
                    WHERE TeacherId=@t AND SubjectId=@s
                """, c,tx);

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
                WHERE ts.TeacherId = @t
                """, c);

            cmd.Parameters.AddWithValue("@t", teacherId);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new Subject
                {
                    Id = Int(r, "Id"),
                    Name = Str(r, "Name"),
                    Type = (SubjectType)(Int(r, "Type")),
                };
            }
        }
    }

}
