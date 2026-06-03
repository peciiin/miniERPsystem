using Microsoft.AspNetCore.Mvc;
using miniERPsystem.Services;

namespace miniERPsystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var report = await _reportService.GetDashboardReportAsync();
            return Ok(report);
        }
    }
}