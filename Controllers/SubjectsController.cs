using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Database;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectsController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public SubjectsController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
            => Ok(_db.Subjects.GetAll().ToList());

        [HttpPost]
        public IActionResult Create(Subject subject)
        {
            try
            {
                _db.Subjects.Insert(subject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            return Ok(subject);
        }
    }
}
