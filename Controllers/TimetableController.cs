using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/timetable")]
    public class TimetableController : ControllerBase
    {
        private readonly SchoolDbContext _db;

        public TimetableController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _db.TimetableEntries
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .Include(t => t.Classroom)
                .Include(t => t.StudentGroup)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<TimetableEntry> entries)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                _db.TimetableEntries.AddRange(entries);
                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return Ok(entries);
            }
            catch
            {
                await tx.RollbackAsync();
                return StatusCode(500, "Failed to create timetable");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _db.TimetableEntries.FindAsync(id);
            if (entry == null) return NotFound();

            _db.TimetableEntries.Remove(entry);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
