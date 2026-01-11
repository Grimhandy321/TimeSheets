using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Database;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/timetable")]
    public class TimetableController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public TimetableController(DatabaseContext db)
        {
            _db = db;
        }

        // GET: api/timetable
        [HttpGet]
        public IActionResult Get()
        {
            var entries = _db.Timetable.GetAll().ToList();
            foreach (var entry in entries)
            {
                entry.Subject = _db.Subjects.GetById(entry.SubjectId);
                entry.Classroom = _db.Classrooms.GetById(entry.ClassroomId);
                entry.StudentGroup = _db.StudentGroups.GetById(entry.StudentGroupId);
            }

            return Ok(entries);
        }

        // POST: api/timetable
        [HttpPost]
        public IActionResult Create([FromBody] TimetableEntry entries)
        {
            if (entries == null)
                return BadRequest("No timetable entries provided");

            try
            {
                _db.Timetable.Insert(entries);
                return Ok(entries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create timetable: {ex.Message}");
            }
        }

        // DELETE: api/timetable/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entry = _db.Timetable.GetById(id);
            if (entry == null) return NotFound();

            try
            {
                _db.Timetable.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete entry: {ex.Message}");
            }
        }
    }
}
