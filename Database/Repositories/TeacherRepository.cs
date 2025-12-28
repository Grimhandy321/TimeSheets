using Microsoft.Data.SqlClient;
using TimeSheets.Models;
using TimeSheets.Models.Enums;

namespace TimeSheets.Database.Repositories
{
    public class TeacherRepository : SqlRepositoryBase, ISqlRepository<Teacher>
    {
        public TeacherRepository(IConfiguration config, DatabaseContext db) : base(config, db) { }

        public IEnumerable<Teacher> GetAll()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT Id, FullName, Salary FROM Teachers", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new Teacher
                {
                    Id = Int(r, "Id"),
                    FullName = Str(r, "FullName"),
                    Salary = Flt(r, "Salary"),
                    Subjects = _db.TeacherSubjects.GetSubjectsForTeacher(Int(r, "Id"))
                };
            }
        }

        public Teacher? GetById(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM Teachers WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Teacher
            {
                Id = Int(r, "Id"),
                FullName = Str(r, "FullName"),
                Salary = Flt(r, "Salary"),
                Subjects = _db.TeacherSubjects.GetSubjectsForTeacher(Int(r, "Id"))
            };
        }

        public Teacher? GetByName(string name)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM Teachers WHERE FullName=@name", c);

            cmd.Parameters.AddWithValue("@name", name);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Teacher
            {
                Id = Int(r, "Id"),
                FullName = Str(r, "FullName"),
                Salary = Flt(r, "Salary"),
                Subjects = _db.TeacherSubjects.GetSubjectsForTeacher(Int(r, "Id"))
            };
        }

        public int Insert(Teacher t)
        {
            using var c = Open();
            using var tx = c.BeginTransaction();

            try
            {
                using var cmd = new SqlCommand("""
                    INSERT INTO Teachers (FullName, Salary)
                        OUTPUT INSERTED.Id
                    VALUES (@n, @s)
                """, c, tx);

                cmd.Parameters.AddWithValue("@n", t.FullName);
                cmd.Parameters.AddWithValue("@s", t.Salary);

                int teacherId = (int)cmd.ExecuteScalar();

                foreach (var subject in t.Subjects)
                {
                    int subjectId = _db.Subjects.GetOrCreate(c, tx, subject);
                    _db.TeacherSubjects.Assign(c, tx, t.Id, subjectId);
                }

                tx.Commit();
                return teacherId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }


        public void Update(Teacher t)
        {
            using var c = Open();
            using var tx = c.BeginTransaction();

            try
            {
                using var cmd = new SqlCommand("""
                    UPDATE Teachers SET
                        FullName = @n,
                        Salary   = @s
                    WHERE Id = @id
                 """, c, tx);

                cmd.Parameters.AddWithValue("@id", t.Id);
                cmd.Parameters.AddWithValue("@n", t.FullName);
                cmd.Parameters.AddWithValue("@s", t.Salary);
                cmd.ExecuteNonQuery();

                var existingSubjectIds =
                    _db.TeacherSubjects.GetSubjectIdsForTeacher(c, tx, t.Id);

                var newSubjectIds = new HashSet<int>();
                foreach (var subject in t.Subjects)
                {
                    int subjectId =
                        _db.Subjects.GetOrCreate(c, tx, subject);

        

                    newSubjectIds.Add(subjectId);
                    if (!existingSubjectIds.Contains(subjectId))
                    {
                        _db.TeacherSubjects.Assign(c, tx, t.Id, subjectId);
                    }
                }

                foreach (var subjectId in existingSubjectIds)
                {
                    if (!newSubjectIds.Contains(subjectId))
                    {
                        _db.TeacherSubjects.Remove(c, tx, t.Id, subjectId);
                    }
                }

                tx.Commit();
            }
            catch 
            {
                tx.Rollback();
                throw;
            }
        }
        public void Delete(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "DELETE FROM Teachers WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }


        public void InsertList(IEnumerable<Teacher> teachers)
        {
            InsertListInternal(
                teachers,
                (cmd, c) =>
                {

                    cmd.Parameters.AddWithValue("@n", c.FullName);
                    cmd.Parameters.AddWithValue("@s", c.Salary);
                },
                """
                    INSERT INTO Teachers (FullName, Salary)
                        OUTPUT INSERTED.Id
                    VALUES (@n, @s)
                """
            );
        }
    }
}
