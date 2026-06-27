using LearnerOpsLms.Data;
using LearnerOpsLms.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnerOpsLms.Controllers
{
    public class DashboardController : Controller
    {
        private readonly LearnerOpsDbContext _context;
        private readonly IConfiguration _configuration;

        public DashboardController(LearnerOpsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var environmentName = _configuration["ENVIRONMENT"] ?? "Local";
            var applicationName = _configuration["APPLICATION_NAME"] ?? "LearnerOps LMS";
            var sqlServerName = _configuration["SQL_SERVER"] ?? "Not configured";
            var sqlDatabaseName = _configuration["SQL_DATABASE"] ?? "Not configured";

            var model = new DashboardViewModel
            {
                CourseCount = await _context.Courses.CountAsync(),
                LearnerCount = await _context.Learners.CountAsync(),
                AssessmentCount = await _context.Assessments.CountAsync(),
                EvidenceItemCount = await _context.EvidenceItems.CountAsync(),

                EnvironmentName = environmentName,
                ApplicationName = applicationName,
                SqlServerName = sqlServerName,
                SqlDatabaseName = sqlDatabaseName
            };

            return View(model);
        }
    }
}