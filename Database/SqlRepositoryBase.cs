using Microsoft.Data.SqlClient;
using System.Data;

namespace TimeSheets.Database
{

    public abstract class SqlRepositoryBase
    {
        protected readonly string _connectionString;

        protected SqlRepositoryBase(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Missing DB connection string");
        }

        protected SqlConnection Open()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }


        /* * Helpers to read typed values from a data record */
        protected static int Int(IDataRecord r, string col) => (int)r[col];
        protected static string Str(IDataRecord r, string col) => (string)r[col];
        protected static bool Bool(IDataRecord r, string col) => (bool)r[col];
        protected static DateTime Dt(IDataRecord r, string col) => (DateTime)r[col];
        protected static float Flt(IDataRecord r, string col) => Convert.ToSingle(r[col]);


        protected void InsertListInternal<T>(IEnumerable<T> entities,Action<SqlCommand, T> parameterBinder,string sql)
        {
            using var conn = Open();
            using var tx = conn.BeginTransaction();

            try
            {
                foreach (var entity in entities)
                {
                    using var cmd = new SqlCommand(sql, conn, tx);
                    parameterBinder(cmd, entity);
                    cmd.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
