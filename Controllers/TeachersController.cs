using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TimeSheets.Database;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public TeachersController(DatabaseContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Get()
            => Ok(_db.Teachers.GetAll().ToList());

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {
            _db.Teachers.Insert(teacher);
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var teacher =  _db.Teachers.GetById(id);
            if (teacher == null) return NotFound();

            _db.Teachers.Delete(id);
            return NoContent();
        }

    }
}
