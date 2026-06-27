using Microsoft.AspNetCore.Authorization;
using LearnerOpsLms.Data;
using LearnerOpsLms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnerOpsLms.Controllers
{
    [Authorize(Policy = "AdminOrAssessor")]
    public class LearnersController : Controller
    {
        private readonly LearnerOpsDbContext _context;

        public LearnersController(LearnerOpsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var learners = await _context.Learners
                .OrderBy(l => l.FullName)
                .ToListAsync();

            return View(learners);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (learner is null)
            {
                return NotFound();
            }

            return View(learner);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Learner());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("FullName,Email,Department,IsActive")] Learner learner)
        {
            if (!ModelState.IsValid)
            {
                return View(learner);
            }

            var emailExists = await _context.Learners
                .AnyAsync(l => l.Email == learner.Email);

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(Learner.Email),
                    "A learner with this email address already exists.");

                return View(learner);
            }

            learner.EnrolledAt = DateTime.UtcNow;

            _context.Learners.Add(learner);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Learner '{learner.FullName}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var learner = await _context.Learners.FindAsync(id);

            if (learner is null)
            {
                return NotFound();
            }

            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,FullName,Email,Department,IsActive")] Learner submittedLearner)
        {
            if (id != submittedLearner.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(submittedLearner);
            }

            var emailExists = await _context.Learners
                .AnyAsync(l =>
                    l.Email == submittedLearner.Email &&
                    l.Id != submittedLearner.Id);

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(Learner.Email),
                    "Another learner already uses this email address.");

                return View(submittedLearner);
            }

            var existingLearner = await _context.Learners.FindAsync(id);

            if (existingLearner is null)
            {
                return NotFound();
            }

            existingLearner.FullName = submittedLearner.FullName;
            existingLearner.Email = submittedLearner.Email;
            existingLearner.Department = submittedLearner.Department;
            existingLearner.IsActive = submittedLearner.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Learner '{existingLearner.FullName}' was updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (learner is null)
            {
                return NotFound();
            }

            return View(learner);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);

            if (learner is null)
            {
                return NotFound();
            }

            learner.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Learner '{learner.FullName}' was deactivated.";

            return RedirectToAction(nameof(Index));
        }
    }
}