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

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new ClassroomUsageView
                {
                    ClassroomName = Str(reader, "ClassroomName"),
                    UsageCount = Int(reader, "UsageCount"),
                    TotalHoursUsed = Flt(reader, "TotalHoursUsed"),
                    GroupsCount = Int(reader, "GroupsCount"),
                    DistinctTeachers = Int(reader, "DistinctTeachers")
                };
            }
        }
    }

}
