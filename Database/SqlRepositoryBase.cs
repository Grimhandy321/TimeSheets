using Microsoft.Data.SqlClient;
using System.Data;

namespace TimeSheets.Database
{

    public abstract class SqlRepositoryBase
    {
        protected readonly string _connectionString;
        protected readonly DatabaseContext _db;

        protected SqlRepositoryBase(IConfiguration config, DatabaseContext db)
        {
            _connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Missing DB connection string");
            _db = db;
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
        protected static DateTime Dt(IDataRecord r, string col)
        {
            var val = r[col];
            return val == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(val);
        }

        protected static DateTime DtNullable(IDataRecord r, string col)
        {
            var val = r[col];
            return val == DBNull.Value ? new DateTime() : Convert.ToDateTime(val);
        }
        protected static float Flt(IDataRecord r, string col)
        {
            var val = r[col];
            return val == DBNull.Value ? 0f : Convert.ToSingle(val);
        }

        protected static double Dbl(IDataRecord r, string col)
        {
            var val = r[col];
            return val == DBNull.Value ? 0.0 : Convert.ToDouble(val);
        }


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
