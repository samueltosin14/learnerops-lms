using Microsoft.AspNetCore.Authorization;
using LearnerOpsLms.Data;
using LearnerOpsLms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnerOpsLms.Controllers
{
    [Authorize(Policy = "AdminOrAssessor")]
    public class CoursesController : Controller
    {
        private readonly LearnerOpsDbContext _context;

        public CoursesController(LearnerOpsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .OrderBy(c => c.Title)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,Level,IsActive")] Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            course.CreatedAt = DateTime.UtcNow;

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Course '{course.Title}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);

            if (course is null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,Level,IsActive")] Course submittedCourse)
        {
            if (id != submittedCourse.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(submittedCourse);
            }

            var existingCourse = await _context.Courses.FindAsync(id);

            if (existingCourse is null)
            {
                return NotFound();
            }

            existingCourse.Title = submittedCourse.Title;
            existingCourse.Description = submittedCourse.Description;
            existingCourse.Level = submittedCourse.Level;
            existingCourse.IsActive = submittedCourse.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Course '{existingCourse.Title}' was updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course is null)
            {
                return NotFound();
            }

            course.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Course '{course.Title}' was deactivated.";

            return RedirectToAction(nameof(Index));
        }
    }
}