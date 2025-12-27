using Microsoft.Data.SqlClient;
using TimeSheets.Models;


namespace TimeSheets.Database.Repositories
{

    public class StudentGroupRepository : SqlRepositoryBase, ISqlRepository<StudentGroup>
    {
        public StudentGroupRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<StudentGroup> GetAll()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM StudentGroup", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new StudentGroup
                {
                    Id = Int(r, "Id"),
                    Name = Str(r, "Name"),
                    StudentCount = Int(r, "StudentCount")
                };
            }
        }

        public StudentGroup? GetById(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM StudentGroup WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new StudentGroup
            {
                Id = Int(r, "Id"),
                Name = Str(r, "Name"),
                StudentCount = Int(r, "StudentCount")
            };
        }

        public int Insert(StudentGroup g)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                INSERT INTO StudentGroup (Name, StudentCount)
                    OUTPUT INSERTED.Id
                VALUES (@n, @c)
             """, c);

            cmd.Parameters.AddWithValue("@n", g.Name);
            cmd.Parameters.AddWithValue("@c", g.StudentCount);

            return (int)cmd.ExecuteScalar();
        }

        public void Update(StudentGroup g)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                UPDATE StudentGroup SET
                    Name=@n,
                    StudentCount=@c
                WHERE Id=@id
            """, c);

            cmd.Parameters.AddWithValue("@id", g.Id);
            cmd.Parameters.AddWithValue("@n", g.Name);
            cmd.Parameters.AddWithValue("@c", g.StudentCount);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "DELETE FROM StudentGroup WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void InsertList(IEnumerable<StudentGroup> entities)
        {
            throw new NotImplementedException();
        }
    }
}
