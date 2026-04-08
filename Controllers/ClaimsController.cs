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
    public class ClaimsController : Controller
    {
        private readonly DBContext _context;

        // Constructor to inject the database context
        public ClaimsController(DBContext context)
        {
            _context = context;
        }

        // GET: Claims
        // Displays a list of claims, with optional search by user email or username
        public async Task<IActionResult> Index(string searchString)
        {
            // Start with all claims including related user and matched item
            var claims = _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .AsQueryable();

            // Filter claims if a search string is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                claims = claims.Where(c =>
                    c.IdentityUser != null &&
                    (c.IdentityUser.Email.ToLower().Contains(searchString.ToLower()) ||
                     c.IdentityUser.UserName.ToLower().Contains(searchString.ToLower())));
            }

            ViewData["CurrentFilter"] = searchString; // keep search string in view

            return View(await claims.ToListAsync());
        }

        // GET: Claims/Details/5
        // Displays details of a single claim
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            // Load the claim including user and matched item
            var claim = await _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .FirstOrDefaultAsync(m => m.ClaimID == id);

            if (claim == null)
                return NotFound();

            return View(claim);
        }

        // GET: Claims/Create
        // Shows a form to create a new claim
        public IActionResult Create()
        {
            // Dropdown for users (usually just logged-in user will be used)
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email");

            // Dropdown for matched lost items
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName");

            return View();
        }

        // POST: Claims/Create
        // Handles form submission for creating a claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimID,ItemName,Description,DateLost,MatchedLostItemID")] APS_LostProperty.Models.Claim claim)
        {
            // Set the user ID to the currently logged-in user
            claim.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Set the submission date to today
            claim.DateSubmitted = DateTime.Today;

            // Remove model validation errors for fields we manually set
            ModelState.Remove("UserID");
            ModelState.Remove("DateSubmitted");

            // Validate DateLost manually
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);
            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "Date lost cannot be in the future.");
            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "Date lost cannot be more than 1 year ago.");

            if (ModelState.IsValid)
            {
                _context.Add(claim); // add new claim to database
                await _context.SaveChangesAsync(); // save changes
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, repopulate dropdowns
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);
            return View(claim);
        }

        // GET: Claims/Edit/5
        // Shows a form to edit an existing claim
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Find the claim to edit
            var claim = await _context.Claim.FindAsync(id);
            if (claim == null)
                return NotFound();

            // Populate dropdown for matched lost items
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);

            return View(claim);
        }

        // POST: Claims/Edit/5
        // Handles form submission for editing a claim
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimID,ItemName,Description,DateLost,MatchedLostItemID,Status")] APS_LostProperty.Models.Claim claim)
        {
            if (id != claim.ClaimID)
                return NotFound();

            // Retrieve the existing claim from the database
            var existingClaim = await _context.Claim.FindAsync(id);
            if (existingClaim == null)
                return NotFound();

            // Preserve original UserID and DateSubmitted to keep ModelState valid
            ModelState.Remove("UserID"); // ignore validation for required UserID
            claim.UserID = existingClaim.UserID;
            claim.DateSubmitted = existingClaim.DateSubmitted;

            // Manual validation for DateLost
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);
            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "Date lost cannot be in the future.");
            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "Date lost cannot be more than 1 year ago.");

            if (ModelState.IsValid)
            {
                // Update only editable fields
                existingClaim.ItemName = claim.ItemName;
                existingClaim.Description = claim.Description;
                existingClaim.DateLost = claim.DateLost;
                existingClaim.MatchedLostItemID = claim.MatchedLostItemID;
                existingClaim.Status = claim.Status;

                await _context.SaveChangesAsync(); // save changes
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown if validation fails
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);
            return View(claim);
        }

        // GET: Claims/Delete/5
        // Displays confirmation page for deleting a claim
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

        // POST: Claims/Delete/5
        // Handles deletion of a claim
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claim.FindAsync(id);
            if (claim != null)
            {
                _context.Claim.Remove(claim); // remove claim from database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if a claim exists by ID
        private bool ClaimExists(int id)
        {
            return _context.Claim.Any(e => e.ClaimID == id);
        }
    }
}