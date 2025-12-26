using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TimeSheets.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "Capacity", "HasProjector", "Name" },
                values: new object[,]
                {
                    { 1, 30, true, "A101" },
                    { 2, 25, false, "B202" },
                    { 3, 40, true, "C303" }
                });

            migrationBuilder.InsertData(
                table: "StudentGroups",
                columns: new[] { "Id", "Name", "StudentCount" },
                values: new object[,]
                {
                    { 1, "1A", 28 },
                    { 2, "2B", 24 }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Mathematics", 0 },
                    { 2, "Physics", 0 },
                    { 3, "Programming", 1 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "FullName", "Salary" },
                values: new object[,]
                {
                    { 1, "John Smith", 45000f },
                    { 2, "Anna Novak", 48000f },
                    { 3, "Peter Johnson", 43000f }
                });

            migrationBuilder.InsertData(
                table: "TeacherSubjects",
                columns: new[] { "SubjectId", "TeacherId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "TimetableEntries",
                columns: new[] { "Id", "ClassroomId", "EndTime", "StartTime", "StudentGroupId", "SubjectId", "TeacherId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 10, 8, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 },
                    { 2, 3, new DateTime(2025, 3, 10, 9, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 2 },
                    { 3, 2, new DateTime(2025, 3, 11, 10, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 11, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TeacherSubjects",
                keyColumns: new[] { "SubjectId", "TeacherId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TeacherSubjects",
                keyColumns: new[] { "SubjectId", "TeacherId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TeacherSubjects",
                keyColumns: new[] { "SubjectId", "TeacherId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "TeacherSubjects",
                keyColumns: new[] { "SubjectId", "TeacherId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "TeacherSubjects",
                keyColumns: new[] { "SubjectId", "TeacherId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StudentGroups",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StudentGroups",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
