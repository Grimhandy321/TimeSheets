using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeSheets.Database;
using TimeSheets.Dto;
using TimeSheets.Models;
using TimeSheets.Models.Enums;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/import")]
    public class ImportController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public ImportController(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Imports teachers from a CSV file.
        /// if the file is missing or invalid, returns BadRequest.
        /// if the data is duplicate or invalid, the database will roll back.
        /// see Saples/teachers.csv for expected format.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("teachers/csv")]
        public IActionResult ImportTeachersCsv(IFormFile file)
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

                _db.Teachers.InsertList(teachers);


                return Ok($"Imported {teachers.Count()} teachers");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"CSV import failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Imports subjects from a json file.
        /// if the file is missing or invalid, returns BadRequest.
        /// if the data is duplicate or invalid, the database will roll back.
        /// see Saples/subjects.json for expected format.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("subjects/json")]
        public IActionResult ImportSubjectsJson(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("JSON file is missing");

            try
            {
                // Deserialize JSON with string-to-enum conversion
                var subjects = JsonSerializer.Deserialize<List<SubjectImportDto>>(
                    file.OpenReadStream(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() } 
                    });

                if (subjects == null || !subjects.Any())
                    return BadRequest("JSON file contains no valid data");

                var entities = subjects.Select(s => new Subject
                {
                    Name = s.Name,
                    Type = s.Type 
                });

                _db.Subjects.InsertList(entities);

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
