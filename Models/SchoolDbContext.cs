using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TimeSheets.Models.Enums;
using TimeSheets.Models.Views;

namespace TimeSheets.Models
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options) { }

        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<TeacherSubject> TeacherSubjects => Set<TeacherSubject>();
        public DbSet<Classroom> Classrooms => Set<Classroom>();
        public DbSet<StudentGroup> StudentGroups => Set<StudentGroup>();
        public DbSet<TimetableEntry> TimetableEntries => Set<TimetableEntry>();
        public DbSet<TeacherLoadView> TeacherLoadViews => Set<TeacherLoadView>();
        public DbSet<ClassroomUsageView> ClassroomUsageViews => Set<ClassroomUsageView>();
        protected override void OnModelCreating(ModelBuilder mb)
        {

            // ======================
            // Configuration
            // ======================

            mb.Entity<TeacherSubject>()
              .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

            mb.Entity<TeacherSubject>()
              .HasOne(ts => ts.Teacher)
              .WithMany(t => t.TeacherSubjects)
              .HasForeignKey(ts => ts.TeacherId);

            mb.Entity<TeacherSubject>()
              .HasOne(ts => ts.Subject)
              .WithMany(s => s.TeacherSubjects)
              .HasForeignKey(ts => ts.SubjectId);

            mb.Entity<TeacherSubject>()
              .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

            mb.Entity<TeacherLoadView>()
              .HasNoKey()
              .ToView("View_TeacherLoad");

            mb.Entity<ClassroomUsageView>()
              .HasNoKey()
              .ToView("View_ClassroomUsage");

            // ======================
            // VIEW mapping
            // ======================
            mb.Entity<TeacherLoadView>().HasNoKey().ToView("View_TeacherLoad");
            mb.Entity<ClassroomUsageView>().HasNoKey().ToView("View_ClassroomUsage");

            // ======================
            // Seed Data
            // ======================

            mb.Entity<Teacher>().HasData(
                new Teacher { Id = 1, FullName = "John Smith", Salary = 45000 },
                new Teacher { Id = 2, FullName = "Anna Novak", Salary = 48000 },
                new Teacher { Id = 3, FullName = "Peter Johnson", Salary = 43000 }
            );

            mb.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "Mathematics", Type = SubjectType.Mandatory },
                new Subject { Id = 2, Name = "Physics", Type = SubjectType.Mandatory },
                new Subject { Id = 3, Name = "Programming", Type = SubjectType.Optional }
            );

            mb.Entity<TeacherSubject>().HasData(
                new TeacherSubject { TeacherId = 1, SubjectId = 1 },
                new TeacherSubject { TeacherId = 1, SubjectId = 2 },
                new TeacherSubject { TeacherId = 2, SubjectId = 1 },
                new TeacherSubject { TeacherId = 2, SubjectId = 3 },
                new TeacherSubject { TeacherId = 3, SubjectId = 3 }
            );
            mb.Entity<Classroom>().HasData(
                new Classroom { Id = 1, Name = "A101", Capacity = 30, HasProjector = true },
                new Classroom { Id = 2, Name = "B202", Capacity = 25, HasProjector = false },
                new Classroom { Id = 3, Name = "C303", Capacity = 40, HasProjector = true }
            );
            mb.Entity<StudentGroup>().HasData(
                new StudentGroup { Id = 1, Name = "1A", StudentCount = 28 },
                new StudentGroup { Id = 2, Name = "2B", StudentCount = 24 }
            );

            mb.Entity<TimetableEntry>().HasData(
                new TimetableEntry
                {
                    Id = 1,
                    TeacherId = 1,
                    SubjectId = 1,
                    ClassroomId = 1,
                    StudentGroupId = 1,
                    StartTime = new DateTime(2025, 3, 10, 8, 0, 0),
                    EndTime = new DateTime(2025, 3, 10, 8, 45, 0)
                },
                new TimetableEntry
                {
                    Id = 2,
                    TeacherId = 2,
                    SubjectId = 3,
                    ClassroomId = 3,
                    StudentGroupId = 2,
                    StartTime = new DateTime(2025, 3, 10, 9, 0, 0),
                    EndTime = new DateTime(2025, 3, 10, 9, 45, 0)
                },
                new TimetableEntry
                {
                    Id = 3,
                    TeacherId = 1,
                    SubjectId = 2,
                    ClassroomId = 2,
                    StudentGroupId = 1,
                    StartTime = new DateTime(2025, 3, 11, 10, 0, 0),
                    EndTime = new DateTime(2025, 3, 11, 10, 45, 0)
                }
            );
        }
    }
}
