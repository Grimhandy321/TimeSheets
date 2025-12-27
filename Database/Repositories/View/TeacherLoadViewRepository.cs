using Microsoft.Data.SqlClient;
using TimeSheets.Models.Views;

namespace TimeSheets.Database.Repositories.View
{
    public class TeacherLoadViewRepository : SqlRepositoryBase
    {
        public TeacherLoadViewRepository(IConfiguration cfg) : base(cfg) { }

        public IEnumerable<TeacherLoadView> Get()
        {
            using var c = Open();
            using var cmd = new SqlCommand(
                "SELECT * FROM View_TeacherLoad", c);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new TeacherLoadView
                {
                    FullName = Str(r, "FullName"),
                    LessonCount = Int(r, "LessonCount"),
                    FirstLesson = Dt(r, "FirstLesson"),
                    LastLesson = Dt(r, "LastLesson")
                };
            }
        }
    }
}
