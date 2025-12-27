using Microsoft.Data.SqlClient;
using TimeSheets.Models;
using TimeSheets.Models.Enums;

namespace TimeSheets.Database.Repositories
{
    public class SubjectRepository: SqlRepositoryBase, ISqlRepository<Subject>
    {
        public SubjectRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<Subject> GetAll()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT Id, Name, Type FROM Subjects", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new Subject
                {
                    Id = Int(r, "Id"),
                    Name = Str(r, "Name"),
                    Type = (SubjectType)Int(r, "Type")
                };
            }
        }

        public Subject? GetById(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM Subjects WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Subject
            {
                Id = Int(r, "Id"),
                Name = Str(r, "Name"),
                Type = (SubjectType)Int(r, "Type")
            };
        }

        public int Insert(Subject s)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                INSERT INTO Subjects (Name, Type)
                    OUTPUT INSERTED.Id
                VALUES (@n, @t)
                """, c);

            cmd.Parameters.AddWithValue("@n", s.Name);
            cmd.Parameters.AddWithValue("@t", (int)s.Type);

            return (int)cmd.ExecuteScalar();
        }

        public void Update(Subject s)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                UPDATE Subjects SET
                    Name=@n,
                    Type=@t
                WHERE Id=@id
                """, c);

            cmd.Parameters.AddWithValue("@id", s.Id);
            cmd.Parameters.AddWithValue("@n", s.Name);
            cmd.Parameters.AddWithValue("@t", (int)s.Type);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "DELETE FROM Subjects WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public void InsertList(IEnumerable<Subject> subjects)
        {
            InsertListInternal(
                subjects,
                (cmd, s) =>
                {
                    cmd.Parameters.AddWithValue("@n", s.Name);
                    cmd.Parameters.AddWithValue("@t", (int)s.Type);
                },
                """
                    INSERT INTO Subjects (Name, Type)
                        OUTPUT INSERTED.Id
                    VALUES (@n, @t)
                """
            );
        }
    }

}
