using Microsoft.AspNetCore.Mvc;
using TimeSheets.Services;

namespace TimeSheets.Controllers
{
    public class IndexController : ControllerBase
    {
        private readonly DatabaseSetupService _db;

        public IndexController(DatabaseSetupService db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult Setup()
        {
            try
            {
                _db.Create();
                _db.Seed();

                return Ok("Database created and seeded");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
