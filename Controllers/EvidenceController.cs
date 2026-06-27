using Microsoft.AspNetCore.Authorization;
using LearnerOpsLms.Data;
using LearnerOpsLms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnerOpsLms.Controllers
{
    [Authorize(Policy = "AdminOrAssessor")]
    public class EvidenceController : Controller
    {
        private readonly LearnerOpsDbContext _context;

        public EvidenceController(LearnerOpsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var evidenceItems = await _context.EvidenceItems
                .OrderByDescending(e => e.UploadedAt)
                .ToListAsync();

            return View(evidenceItems);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var evidenceItem = await _context.EvidenceItems
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evidenceItem is null)
            {
                return NotFound();
            }

            return View(evidenceItem);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new EvidenceItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Notes,FilePath,IsArchived")]
            EvidenceItem evidenceItem)
        {
            if (!ModelState.IsValid)
            {
                return View(evidenceItem);
            }

            evidenceItem.UploadedAt = DateTime.UtcNow;

            _context.EvidenceItems.Add(evidenceItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Evidence '{evidenceItem.Title}' was created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var evidenceItem = await _context.EvidenceItems.FindAsync(id);

            if (evidenceItem is null)
            {
                return NotFound();
            }

            return View(evidenceItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Notes,FilePath,IsArchived")]
            EvidenceItem submittedEvidence)
        {
            if (id != submittedEvidence.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(submittedEvidence);
            }

            var existingEvidence =
                await _context.EvidenceItems.FindAsync(id);

            if (existingEvidence is null)
            {
                return NotFound();
            }

            existingEvidence.Title = submittedEvidence.Title;
            existingEvidence.Notes = submittedEvidence.Notes;
            existingEvidence.FilePath = submittedEvidence.FilePath;
            existingEvidence.IsArchived = submittedEvidence.IsArchived;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Evidence '{existingEvidence.Title}' was updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Archive(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var evidenceItem = await _context.EvidenceItems
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evidenceItem is null)
            {
                return NotFound();
            }

            return View(evidenceItem);
        }

        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArchiveConfirmed(int id)
        {
            var evidenceItem = await _context.EvidenceItems.FindAsync(id);

            if (evidenceItem is null)
            {
                return NotFound();
            }

            evidenceItem.IsArchived = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Evidence '{evidenceItem.Title}' was archived.";

            return RedirectToAction(nameof(Index));
        }
    }
}