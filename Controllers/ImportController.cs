using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using TimeSheets.Dto;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/import")]
    public class ImportController : ControllerBase
    {
        private readonly SchoolDbContext _db;

        public ImportController(SchoolDbContext db)
        {
            _db = db;
        }

        // ========================
        // CSV IMPORT → TEACHERS
        // ========================
        [HttpPost("teachers/csv")]
        public async Task<IActionResult> ImportTeachersCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("CSV file is missing");

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<TeacherImportDto>().ToList();

                if (!records.Any())
                    return BadRequest("CSV file contains no data");

                var teachers = records.Select(t => new Teacher
                {
                    FullName = t.FullName,
                    Salary = t.Salary
                });

                _db.Teachers.AddRange(teachers);
                await _db.SaveChangesAsync();

                return Ok($"Imported {teachers.Count()} teachers");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"CSV import failed: {ex.Message}");
            }
        }

        // ========================
        // JSON IMPORT → SUBJECTS
        // ========================
        [HttpPost("subjects/json")]
        public async Task<IActionResult> ImportSubjectsJson(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("JSON file is missing");

            try
            {
                var subjects = await JsonSerializer.DeserializeAsync<List<SubjectImportDto>>(
                    file.OpenReadStream(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (subjects == null || !subjects.Any())
                    return BadRequest("JSON file contains no valid data");

                var entities = subjects.Select(s => new Subject
                {
                    Name = s.Name,
                    Type = s.Type
                });

                _db.Subjects.AddRange(entities);
                await _db.SaveChangesAsync();

                return Ok($"Imported {entities.Count()} subjects");
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON format");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"JSON import failed: {ex.Message}");
            }
        }
    }
}
