using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly SchoolDbContext _db;

        public SubjectsController(SchoolDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _db.Subjects.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Subject subject)
        {
            _db.Subjects.Add(subject);
            await _db.SaveChangesAsync();
            return Ok(subject);
        }
    }
}
