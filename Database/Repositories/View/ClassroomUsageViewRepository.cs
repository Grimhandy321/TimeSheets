using Microsoft.Data.SqlClient;
using TimeSheets.Models.Views;



namespace TimeSheets.Database.Repositories.View
{
    public class ClassroomUsageViewRepository : SqlRepositoryBase
    {
        public ClassroomUsageViewRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<ClassroomUsageView> Get()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM View_ClassroomUsage", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new ClassroomUsageView
                {
                    Classroom = Str(r, "ClassroomName"),
                    UsageCount = Int(r, "UsageCount")
                };
            }
        }
    }

}
