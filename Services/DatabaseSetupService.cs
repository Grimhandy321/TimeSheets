using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

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


            string projectRoot =
                Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            _sqlPath = Path.Combine(projectRoot, "sql");
        }

        public void Create()
        {
            ExecuteSqlFile("migration.sql");
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

            var sql = File.ReadAllText(fullPath);

            // Rozdělení podle GO (case-insensitive, musí být samostatně na řádku)
            var batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch)) continue;
                using var cmd = new SqlCommand(batch, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
