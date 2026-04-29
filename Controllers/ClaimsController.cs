using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Models;
using Microsoft.AspNetCore.Authorization;
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
    // basically everything related to a user making a lost item claim
    public class ClaimsController : Controller
    {
        // database context (this lets us talk to SQL tables like Claim, Users, LostItem etc.)
        private readonly DBContext _context;

        // constructor runs when controller starts
        // dependency injection gives us database access
        public ClaimsController(DBContext context)
        {
            _context = context;
        }

        // GET: Claims
        // this loads the main claims page (list view)
        // also supports search + pagination
        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            // how many items we want per page (pagination size)
            int pageSize = 5;

            // start building query from Claim table
            // IQueryable means we can keep adding filters before actually running SQL
            var claimsQuery = _context.Claim
                .Include(c => c.IdentityUser)        // loads related user data (email, username)
                .Include(c => c.MatchedLostItem)     // loads matched lost item info
                .AsQueryable();                      // keeps it flexible for filtering

            // ---------------- SEARCH SECTION ----------------

            // check if user typed anything in the search bar
            if (!string.IsNullOrEmpty(searchString))
            {
                // convert search text to lowercase so search is not case-sensitive
                var search = searchString.ToLower();

                // filter claims based on email or username match
                claimsQuery = claimsQuery.Where(c =>
                    c.IdentityUser != null &&   // safety check (avoid null errors)

                    (
                        // check email match
                        (c.IdentityUser.Email != null &&
                         c.IdentityUser.Email.ToLower().Contains(search)) ||

                        // check username match
                        (c.IdentityUser.UserName != null &&
                         c.IdentityUser.UserName.ToLower().Contains(search))
                    )
                );
            }

            // sort results so better matches appear first
            // here we try to push "starts with" matches to the top
            claimsQuery = claimsQuery
    .OrderBy(c =>
        !string.IsNullOrEmpty(searchString) &&
        c.IdentityUser != null &&
        c.IdentityUser.UserName != null &&
        c.IdentityUser.UserName.StartsWith(searchString)
            ? 0 : 1
    )
   .ThenBy(c => c.IdentityUser != null ? c.IdentityUser.UserName : "");

            // ---------------- PAGINATION SECTION ----------------

            // count total records AFTER search filter is applied
            var totalItems = await claimsQuery.CountAsync();

            // calculate how many pages we need in total
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // skip previous pages and only take current page results
            var claims = await claimsQuery
                .Skip((page - 1) * pageSize)   // skip earlier pages
                .Take(pageSize)                // take only this page's items
                .ToListAsync();                // run query and convert to list

            // send current page number to the view
            ViewData["CurrentPage"] = page;

            // send total pages so UI can build page buttons
            ViewData["TotalPages"] = totalPages;

            // keep search text so it stays in search box after refresh
            ViewData["CurrentFilter"] = searchString;

            // return final filtered + paginated list to view
            return View(claims);
        }
        // GET: Claims/Details/5
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
        // this loads the create claim page (form view)
        [HttpGet]
        public IActionResult Create()
        {
            // load dropdown list of lost items so user can select one
            ViewData["MatchedLostItemID"] =
                new SelectList(_context.LostItem, "LostItemID", "ItemName");

            return View();
        }

        // POST: Create
        // this runs when user submits the create form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimedItemName,ClaimedDescription,DateLost")] APS_LostProperty.Models.Claim claim)
        {
            // automatically set logged-in user ID
            claim.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // automatically set submission date to today
            claim.DateSubmitted = DateTime.Today;

            // automatically set status when claim is created
            claim.Status = ClaimStatus.Submitted;

            // remove validation for fields we set automatically (so ModelState doesn't fail)
            ModelState.Remove("UserID");
            ModelState.Remove("DateSubmitted");
            ModelState.Remove("Status");
            ModelState.Remove("MatchedLostItemID");

            // date validation rules
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);

            // check if date is in the future
            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "Date Cannot Be in The Future");

            // check if date is too old
            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "date too old");

            // if everything is valid, save to database
            if (ModelState.IsValid)
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // if validation fails, reload page with errors
            return View(claim);
        }

        // GET: Edit
        // loads edit page for a specific claim
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            // if no id provided, return error
            if (id == null) return NotFound();

            // find claim in database
            var claim = await _context.Claim
      .Include(c => c.IdentityUser)
      .Include(c => c.MatchedLostItem)
      .FirstOrDefaultAsync(c => c.ClaimID == id);

            // if claim doesn't exist
            if (claim == null)
                return NotFound();

            // load dropdown of lost items (sorted alphabetically)
            ViewData["MatchedLostItemID"] =
                new SelectList(
                    _context.LostItem.OrderBy(l => l.ItemName),
                    "LostItemID",
                    "ItemName",
                    claim.MatchedLostItemID
                );

            return View(claim);
        }

        // POST: Edit
        // saves updated claim data
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int id,
            [Bind("ClaimID,ClaimedItemName,ClaimedDescription,DateLost,MatchedLostItemID,Status")] APS_LostProperty.Models.Claim claim)
        {
            // check id matches claim
            if (id != claim.ClaimID)
                return NotFound();

            // get existing record from database
            var existing = await _context.Claim.FindAsync(id);

            if (existing == null)
                return NotFound();

            // keep original system values
            claim.UserID = existing.UserID;
            claim.DateSubmitted = existing.DateSubmitted;

            // ignore validation for system field
            ModelState.Remove("UserID");

            // date validation
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);

            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "invalid");

            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "too old");

            // if valid, update fields
            if (ModelState.IsValid)
            {
                existing.ClaimedItemName= claim.ClaimedItemName;
                existing.ClaimedDescription = claim.ClaimedDescription;
                existing.DateLost = claim.DateLost;
                existing.MatchedLostItemID = claim.MatchedLostItemID;
                existing.Status = claim.Status;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // reload dropdown if validation fails
            ViewData["MatchedLostItemID"] = new SelectList(
                _context.LostItem.OrderBy(l => l.ItemName),
                "LostItemID",
                "ItemName",
                claim.MatchedLostItemID
            );

            return View(claim);
        }

        // GET: Delete
        // shows confirmation page before deleting claim
        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            // load claim with user + lost item details
            var claim = await _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .FirstOrDefaultAsync(m => m.ClaimID == id);

            if (claim == null)
                return NotFound();

            return View(claim);
        }

        // POST: Delete
        // permanently removes claim from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claim
     .Include(c => c.IdentityUser)
     .Include(c => c.MatchedLostItem)
     .FirstOrDefaultAsync(c => c.ClaimID == id);
            if (claim != null)
            {
                _context.Claim.Remove(claim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}