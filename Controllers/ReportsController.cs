using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Database;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly DatabaseContext _db;

        public ReportsController(DatabaseContext db)
        {
            _db = db;
        }

        // ============================
        // REPORT 1: Teacher workload
        // ============================
        [HttpGet("teacher-load")]
        public IActionResult GetTeacherLoad()
        {
            var data = _db.TeacherLoadView.Get();
            return Ok(data);
        }

        // ============================
        // REPORT 2: Classroom usage
        // ============================
        [HttpGet("classroom-usage")]
        public IActionResult GetClassroomUsage()
        {
            var data =  _db.ClassroomUsageView.Get();
            return Ok(data);
        }
    }
}
