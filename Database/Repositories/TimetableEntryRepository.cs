using Microsoft.Data.SqlClient;
using TimeSheets.Models;


namespace TimeSheets.Database.Repositories
{
 
    public class TimetableEntryRepository: SqlRepositoryBase, ISqlRepository<TimetableEntry>
    {
        public TimetableEntryRepository(IConfiguration config, DatabaseContext db) : base(config, db) { }

        public IEnumerable<TimetableEntry> GetAll()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM TimetableEntries", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                var teacher = _db.Teachers.GetById(Int(r, "TeacherId"));
                yield return new TimetableEntry
                {
                    Id = Int(r, "Id"),
                    Teacher = new Teacher {
                        Id = teacher?.Id ?? 0,
                        FullName = teacher?.FullName ?? "Unknown",
                        Salary = teacher?.Salary ?? 0f
                    },
                    SubjectId = Int(r, "SubjectId"),
                    ClassroomId = Int(r, "ClassroomId"),
                    StudentGroupId = Int(r, "StudentGroupId"),
                    StartTime = Dt(r, "StartTime"),
                    EndTime = Dt(r, "EndTime")
                };
            }
        }

        public TimetableEntry? GetById(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM TimetableEntries WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new TimetableEntry
            {
                Id = Int(r, "Id"),
                TeacherId = Int(r, "TeacherId"),
                SubjectId = Int(r, "SubjectId"),
                ClassroomId = Int(r, "ClassroomId"),
                StudentGroupId = Int(r, "StudentGroupId"),
                StartTime = Dt(r, "StartTime"),
                EndTime = Dt(r, "EndTime")
            };
        }

        public int Insert(TimetableEntry t)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                INSERT INTO TimetableEntries
                    (TeacherId, SubjectId, ClassroomId, StudentGroupId, StartTime, EndTime)
                OUTPUT INSERTED.Id
                VALUES (@t, @s, @c, @g, @st, @et)
            """, c);

            cmd.Parameters.AddWithValue("@t", t.TeacherId);
            cmd.Parameters.AddWithValue("@s", t.SubjectId);
            cmd.Parameters.AddWithValue("@c", t.ClassroomId);
            cmd.Parameters.AddWithValue("@g", t.StudentGroupId);
            cmd.Parameters.AddWithValue("@st", t.StartTime);
            cmd.Parameters.AddWithValue("@et", t.EndTime);

            return (int)cmd.ExecuteScalar();
        }

        public void Update(TimetableEntry t)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                UPDATE TimetableEntries SET
                    TeacherId=@t,
                    SubjectId=@s,
                    ClassroomId=@c,
                    StudentGroupId=@g,
                    StartTime=@st,
                    EndTime=@et
                WHERE Id=@id
            """, c);

            cmd.Parameters.AddWithValue("@id", t.Id);
            cmd.Parameters.AddWithValue("@t", t.TeacherId);
            cmd.Parameters.AddWithValue("@s", t.SubjectId);
            cmd.Parameters.AddWithValue("@c", t.ClassroomId);
            cmd.Parameters.AddWithValue("@g", t.StudentGroupId);
            cmd.Parameters.AddWithValue("@st", t.StartTime);
            cmd.Parameters.AddWithValue("@et", t.EndTime);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "DELETE FROM TimetableEntries WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void InsertList(IEnumerable<TimetableEntry> entities)
        {
            throw new NotImplementedException();
        }
    }

}
