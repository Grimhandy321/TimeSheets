using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/classrooms")]
    public class ClassroomsController : ControllerBase
    {
        private readonly SchoolDbContext _db;

        public ClassroomsController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _db.Classrooms.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Classroom classroom)
        {
            _db.Classrooms.Add(classroom);
            await _db.SaveChangesAsync();
            return Ok(classroom);
        }
    }
}
