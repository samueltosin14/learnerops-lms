using Microsoft.AspNetCore.Authorization;
using LearnerOpsLms.Data;
using LearnerOpsLms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnerOpsLms.Controllers
{
    [Authorize(Policy = "AdminOrAssessor")]
    public class AssessmentsController : Controller
    {
        private readonly LearnerOpsDbContext _context;

        public AssessmentsController(LearnerOpsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var assessments = await _context.Assessments
                .OrderBy(a => a.Title)
                .ToListAsync();

            return View(assessments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment is null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Assessment
            {
                MaxScore = 100,
                PassScore = 70,
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,MaxScore,PassScore,IsActive")]
            Assessment assessment)
        {
            if (!ModelState.IsValid)
            {
                return View(assessment);
            }

            assessment.CreatedAt = DateTime.UtcNow;

            _context.Assessments.Add(assessment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Assessment '{assessment.Title}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessments.FindAsync(id);

            if (assessment is null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,MaxScore,PassScore,IsActive")]
            Assessment submittedAssessment)
        {
            if (id != submittedAssessment.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(submittedAssessment);
            }

            var existingAssessment =
                await _context.Assessments.FindAsync(id);

            if (existingAssessment is null)
            {
                return NotFound();
            }

            existingAssessment.Title = submittedAssessment.Title;
            existingAssessment.Description = submittedAssessment.Description;
            existingAssessment.MaxScore = submittedAssessment.MaxScore;
            existingAssessment.PassScore = submittedAssessment.PassScore;
            existingAssessment.IsActive = submittedAssessment.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Assessment '{existingAssessment.Title}' was updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var assessment = await _context.Assessments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assessment is null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            var assessment = await _context.Assessments.FindAsync(id);

            if (assessment is null)
            {
                return NotFound();
            }

            assessment.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Assessment '{assessment.Title}' was deactivated.";

            return RedirectToAction(nameof(Index));
        }
    }
}