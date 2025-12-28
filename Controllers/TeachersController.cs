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
            try
            {
                _db.Teachers.Insert(teacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var teacher = _db.Teachers.GetById(id);
            if (teacher == null) return NotFound();

            _db.Teachers.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update(Teacher teacher)
        {
            try
            {
                //bind id by unique FullName
                teacher.Id = (_db.Teachers.GetByName(teacher.FullName) ?? throw new Exception("Teacher not found")).Id;
                _db.Teachers.Update(teacher);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            return Ok(teacher);
        }

    }
}
