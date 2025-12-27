using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Database;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/classrooms")]
    public class ClassroomsController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public ClassroomsController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
            => Ok(_db.Classrooms.GetAll().ToList());

        [HttpPost]
        public  IActionResult Create(Classroom classroom)
        {
            _db.Classrooms.Insert(classroom);
            return Ok(classroom);
        }
    }
}
