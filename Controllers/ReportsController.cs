using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSheets.Models;

namespace TimeSheets.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly SchoolDbContext _db;

        public ReportsController(SchoolDbContext db)
        {
            _db = db;
        }

        // ============================
        // REPORT 1: Teacher workload
        // ============================
        [HttpGet("teacher-load")]
        public async Task<IActionResult> GetTeacherLoad()
        {
            var data = await _db.TeacherLoadViews.ToListAsync();
            return Ok(data);
        }

        // ============================
        // REPORT 2: Classroom usage
        // ============================
        [HttpGet("classroom-usage")]
        public async Task<IActionResult> GetClassroomUsage()
        {
            var data = await _db.ClassroomUsageViews.ToListAsync();
            return Ok(data);
        }
    }
}
