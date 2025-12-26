using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
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
        }
    }
}
