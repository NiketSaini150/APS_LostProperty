using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APS_LostProperty.Controllers
{
    // this controller is for handling claims (create, edit, delete, view)
    public class ClaimsController : Controller
    {
        private readonly DBContext _context;

        public ClaimsController(DBContext context)
        {
            _context = context;
        }

        // GET: Claims
        // shows all claims and also search feature for email/username
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var claims = _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = searchString.ToLower();

                claims = claims.Where(c =>
                    c.IdentityUser != null &&
                    (
                        c.IdentityUser.Email != null &&
                        c.IdentityUser.Email.ToLower().Contains(search)
                        ||
                        c.IdentityUser.UserName != null &&
                        c.IdentityUser.UserName.ToLower().Contains(search)
                    ));
            }

            claims = claims.OrderBy(c => c.ItemName);

            ViewData["CurrentFilter"] = searchString;

            return View(await claims.ToListAsync());
        }

        // GET: Details
        // shows full info about one claim
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var claim = await _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .FirstOrDefaultAsync(m => m.ClaimID == id);

            if (claim == null)
                return NotFound();

            return View(claim);
        }

        // GET: Create
        // loads create page
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["MatchedLostItemID"] =
                new SelectList(_context.LostItem, "LostItemID", "ItemName");

            return View();
        }

        // POST: Create
        // saves new claim into database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemName,Description,DateLost")] APS_LostProperty.Models.Claim claim)
        {
            claim.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            claim.DateSubmitted = DateTime.Today;
            claim.Status = ClaimStatus.Submitted;

            ModelState.Remove("UserID");
            ModelState.Remove("DateSubmitted");
            ModelState.Remove("Status");
            ModelState.Remove("MatchedLostItemID");

            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);

            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "invalid date");

            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "date too old");

            if (ModelState.IsValid)
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(claim);
        }

        // GET: Edit
        // shows edit page (staff only idea)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var claim = await _context.Claim.FindAsync(id);

            if (claim == null)
                return NotFound();

            ViewData["MatchedLostItemID"] =
                new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);

            return View(claim);
        }

        // POST: Edit
        // updates claim info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("ClaimID,ItemName,Description,DateLost,MatchedLostItemID,Status")] APS_LostProperty.Models.Claim claim)
        {
            if (id != claim.ClaimID)
                return NotFound();

            var existing = await _context.Claim.FindAsync(id);

            if (existing == null)
                return NotFound();

            claim.UserID = existing.UserID;
            claim.DateSubmitted = existing.DateSubmitted;

            ModelState.Remove("UserID");

            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);

            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "invalid");

            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "too old");

            if (ModelState.IsValid)
            {
                existing.ItemName = claim.ItemName;
                existing.Description = claim.Description;
                existing.DateLost = claim.DateLost;
                existing.MatchedLostItemID = claim.MatchedLostItemID;
                existing.Status = claim.Status;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

        
             ViewData["MatchedLostItemID"] = new SelectList(
    _context.LostItem.OrderBy(l => l.ItemName),
    "LostItemID",
    "ItemName",
    claim.MatchedLostItemID
);

            return View(claim);
        }

        // GET: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var claim = await _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .FirstOrDefaultAsync(m => m.ClaimID == id);

            if (claim == null)
                return NotFound();

            return View(claim);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claim.FindAsync(id);

            if (claim != null)
            {
                _context.Claim.Remove(claim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}