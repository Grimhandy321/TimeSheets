using Microsoft.Data.SqlClient;

namespace TimeSheets.Services
{
    public class DatabaseSetupService
    {
        private readonly string _connectionString;
        private readonly string _sqlPath;

        public DatabaseSetupService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Missing connection string");

            _sqlPath = Path.Combine(AppContext.BaseDirectory, "sql");
        }

        public void Create()
        {
            ExecuteSqlFile("database.sql");
        }

        public void Seed()
        {
            ExecuteSqlFile("seed.sql");
        }

        private void ExecuteSqlFile(string fileName)
        {
            var fullPath = Path.Combine(_sqlPath, fileName);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException(fullPath);

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand(
                File.ReadAllText(fullPath),
                conn);

            cmd.ExecuteNonQuery();
        }
    }
}
