using Microsoft.Data.SqlClient;
using TimeSheets.Models;

namespace TimeSheets.Database.Repositories
{
    public class TeacherRepository: SqlRepositoryBase, ISqlRepository<Teacher>
    {
        public TeacherRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<Teacher> GetAll()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT Id, FullName, Salary FROM Teacher", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new Teacher
                {
                    Id = Int(r, "Id"),
                    FullName = Str(r, "FullName"),
                    Salary = Flt(r, "Salary")
                };
            }
        }

        public Teacher? GetById(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM Teacher WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Teacher
            {
                Id = Int(r, "Id"),
                FullName = Str(r, "FullName"),
                Salary = Flt(r, "Salary")
            };
        }

        public int Insert(Teacher t)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                INSERT INTO Teacher (FullName, Salary)
                    OUTPUT INSERTED.Id
                VALUES (@n, @s)
                """, c);

            cmd.Parameters.AddWithValue("@n", t.FullName);
            cmd.Parameters.AddWithValue("@s", t.Salary);

            return (int)cmd.ExecuteScalar();
        }

        public void Update(Teacher t)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                UPDATE Teacher SET
                    FullName=@n,
                    Salary=@s
                WHERE Id=@id
                """, c);

            cmd.Parameters.AddWithValue("@id", t.Id);
            cmd.Parameters.AddWithValue("@n", t.FullName);
            cmd.Parameters.AddWithValue("@s", t.Salary);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "DELETE FROM Teacher WHERE Id=@id", c);

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
                    INSERT INTO Teacher (FullName, Salary)
                        OUTPUT INSERTED.Id
                    VALUES (@n, @s)
                """
            );
        }
    }
}
