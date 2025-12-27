using TimeSheets.Database.Repositories.View;
using TimeSheets.Database.Repositories;
using TimeSheets.Models;

namespace TimeSheets.Database
{
    public class DatabaseContext
    {

        public TeacherRepository Teachers { get; }
        public SubjectRepository Subjects { get; }
        public TimetableEntryRepository Timetable { get; }
        public ClassroomRepository Classrooms { get; }
        public StudentGroupRepository StudentGroups { get; } 

        public TeacherLoadViewRepository TeacherLoadView { get; }
        public ClassroomUsageViewRepository ClassroomUsageView { get; }


        public DatabaseContext(IConfiguration cfg)
        {
            Teachers = new TeacherRepository(cfg);
            Subjects = new SubjectRepository(cfg);
            Timetable = new TimetableEntryRepository(cfg);
            Classrooms = new ClassroomRepository(cfg);
            StudentGroups = new StudentGroupRepository(cfg);

            TeacherLoadView = new TeacherLoadViewRepository(cfg);
            ClassroomUsageView = new ClassroomUsageViewRepository(cfg);
        }
    }
}
