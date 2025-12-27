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
        public TeacherSubjectRepository TeacherSubjects { get; }
        public ClassroomRepository Classrooms { get; }
        public StudentGroupRepository StudentGroups { get; } 

        public TeacherLoadViewRepository TeacherLoadView { get; }
        public ClassroomUsageViewRepository ClassroomUsageView { get; }


        public DatabaseContext(IConfiguration cfg)
        {
            Teachers = new TeacherRepository(cfg,this);
            Subjects = new SubjectRepository(cfg,this);
            Timetable = new TimetableEntryRepository(cfg, this);
            Classrooms = new ClassroomRepository(cfg, this);
            StudentGroups = new StudentGroupRepository(cfg, this);
            TeacherSubjects = new TeacherSubjectRepository(cfg, this);

            TeacherLoadView = new TeacherLoadViewRepository(cfg,this);
            ClassroomUsageView = new ClassroomUsageViewRepository(cfg,this);
        }
    }
}
