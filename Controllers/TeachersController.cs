using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly SchoolDbContext _db;

        public TeachersController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _db.Teachers.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            _db.Teachers.Add(teacher);
            await _db.SaveChangesAsync();
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _db.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            _db.Teachers.Remove(teacher);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
