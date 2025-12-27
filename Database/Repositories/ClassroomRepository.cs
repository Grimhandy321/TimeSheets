using Microsoft.Data.SqlClient;
using TimeSheets.Models;

namespace TimeSheets.Database.Repositories
{
    public class ClassroomRepository : SqlRepositoryBase, ISqlRepository<Classroom>
    {
        public ClassroomRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<Classroom> GetAll()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM Classrooms", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new Classroom
                {
                    Id = Int(r, "Id"),
                    Name = Str(r, "Name"),
                    Capacity = Int(r, "Capacity"),
                    HasProjector = Bool(r, "HasProjector")
                };
            }
        }

        public Classroom? GetById(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM Classrooms WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Classroom
            {
                Id = Int(r, "Id"),
                Name = Str(r, "Name"),
                Capacity = Int(r, "Capacity"),
                HasProjector = Bool(r, "HasProjector")
            };
        }

        public int Insert(Classroom croom)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                INSERT INTO Classrooms (Name, Capacity, HasProjector)
                    OUTPUT INSERTED.Id
                VALUES (@n, @c, @p)
                """, c);

            cmd.Parameters.AddWithValue("@n", croom.Name);
            cmd.Parameters.AddWithValue("@c", croom.Capacity);
            cmd.Parameters.AddWithValue("@p", croom.HasProjector);

            return (int)cmd.ExecuteScalar();
        }

        public void Update(Classroom croom)
        {
            using var c = Open();
            using var cmd = new SqlCommand("""
                UPDATE Classrooms SET
                    Name=@n,
                    Capacity=@c,
                    HasProjector=@p
                WHERE Id=@id
            """, c);

            cmd.Parameters.AddWithValue("@id", croom.Id);
            cmd.Parameters.AddWithValue("@n", croom.Name);
            cmd.Parameters.AddWithValue("@c", croom.Capacity);
            cmd.Parameters.AddWithValue("@p", croom.HasProjector);

            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "DELETE FROM Classrooms WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void InsertList(IEnumerable<Classroom> classrooms)
        {
            InsertListInternal(
                classrooms,
                (cmd, c) =>
                {
                    cmd.Parameters.AddWithValue("@name", c.Name);
                    cmd.Parameters.AddWithValue("@capacity", c.Capacity);
                    cmd.Parameters.AddWithValue("@projector", c.HasProjector);
                },
                """
                  INSERT INTO Classrooms (Name, Capacity, HasProjector)
                        VALUES (@name, @capacity, @projector)
                """
            );
        }
    }

}
