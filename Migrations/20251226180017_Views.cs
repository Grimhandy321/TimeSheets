using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSheets.Migrations
{
    /// <inheritdoc />
    public partial class Views : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE VIEW View_TeacherLoad AS
                    SELECT 
                        t.FullName,
                        COUNT(te.Id) AS LessonCount,
                        MIN(te.StartTime) AS FirstLesson,
                        MAX(te.EndTime) AS LastLesson
                    FROM Teachers t
                        JOIN TimetableEntries te ON te.TeacherId = t.Id
                    GROUP BY t.FullName
            ");

            migrationBuilder.Sql(@"
                CREATE VIEW View_ClassroomUsage AS
                    SELECT
                        c.Name AS Classroom,
                        COUNT(te.Id) AS UsageCount
                    FROM Classrooms c
                        LEFT JOIN TimetableEntries te ON te.ClassroomId = c.Id
                    GROUP BY c.Name
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS View_TeacherLoad");
            migrationBuilder.Sql("DROP VIEW IF EXISTS View_ClassroomUsage");
        }
    }
}
