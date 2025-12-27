using Microsoft.Data.SqlClient;
using TimeSheets.Models.Views;

namespace TimeSheets.Database.Repositories.View
{
    public class TeacherLoadViewRepository : SqlRepositoryBase
    {
        public TeacherLoadViewRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<TeacherLoadView> Get()
        {
            using var conn = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM View_TeacherLoad", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new TeacherLoadView
                {
                    TeacherName = Str(reader, "TeacherName"),
                    LessonCount = Int(reader, "LessonCount"),
                    TotalHours = Flt(reader, "TotalHours"),                 // float/double
                    AvgLessonMinutes = Flt(reader, "AvgLessonMinutes"),     // float/double
                    FirstLesson = Dt(reader, "FirstLesson"),
                    LastLesson = Dt(reader, "LastLesson"),
                    DistinctGroupsTaught = Int(reader, "DistinctGroupsTaught"),
                    DistinctClassroomsUsed = Int(reader, "DistinctClassroomsUsed")
                };
            }
        }
    }
}
