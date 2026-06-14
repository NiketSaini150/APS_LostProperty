using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Migrations;
using APS_LostProperty.Models;
using APS_LostProperty.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
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
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        // constructor runs when controller starts
        // dependency injection gives us database access
        public ClaimsController(DBContext context, IEmailSender emailSender, UserManager<User> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        // GET: Claims
        // this loads the main claims page (list view)
        // also supports search + pagination
        [HttpGet]

        
        public async Task<IActionResult> Index(string searchString, string statusFilter, int page = 1)
        {
            // how many items we want per page (pagination size)
            int pageSize = 10;

            // start building query from Claim table
            // IQueryable means we can keep adding filters before actually running SQL
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claimsQuery = _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .AsQueryable();

            // if user is NOT staff, only show their own claims
            if (!User.IsInRole("Staff"))
            {
                claimsQuery = claimsQuery.Where(c => c.UserID == userId);
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                switch (statusFilter.ToLower())
                {
                    case "submitted":
                        claimsQuery = claimsQuery.Where(c => c.Status == ClaimStatus.Submitted);
                        break;
                    case "approved":
                        claimsQuery = claimsQuery.Where(c => c.Status == ClaimStatus.Approved);
                        break;
                    case "collected":
                        claimsQuery = claimsQuery.Where(c => c.Status == ClaimStatus.Collected);
                        break;
                    case "rejected":
                        claimsQuery = claimsQuery.Where(c => c.Status == ClaimStatus.Rejected);
                        break;
                    default:
                        break;
                }
            }
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

            // Order by submitted date descending so newest claims appear first
            // then fallback to username for stable ordering
            claimsQuery = claimsQuery
                .OrderByDescending(c => c.DateSubmitted)
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

            // send current page number and filters to the view
            ViewData["CurrentPage"] = page;
            ViewBag.SearchString = searchString;
            ViewBag.StatusFilter = statusFilter;

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
            var oneYearAgo = today.AddMonths(-3);

            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "Date Lost cannot be in The Future! ");

            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "Date Lost cannot be more than 3 months in the Past!");

            // if everything is valid, save to database
           
                if (ModelState.IsValid)
                {
                    _context.Add(claim);
                    await _context.SaveChangesAsync();

               
                    if (!string.IsNullOrEmpty(claim.UserID))
                    {
                        var user = await _userManager.FindByIdAsync(claim.UserID);

                    if (user?.Email != null)
                    {

                        await _emailSender.SendEmailAsync(
                            user.Email,
                            "Claim Submitted - " + claim.ClaimedItemName,
                            $"Dear {user.UserName},\n\n" +
                            $"Thank you for submitting a claim for '{claim.ClaimedItemName}'.\n\n" +
                            "Your claim has been received and is currently being reviewed by the APS Lost Property Team.\n\n" +
                            "We will contact you once a decision has been made regarding your claim. Additional information may be requested if required.\n\n" +
                            "Thank you for using the APS Lost Property System.\n\n" +
                            "Kind regards,\n" +
                            "APS Lost Property Team"
                        );
                    }

                }

                    return RedirectToAction(nameof(Index));
                }

         
            
            // if validation fails, reload page with errors
            return View(claim);
        }

        // GET: Edit
        // loads edit page for a specific claim
        [HttpGet]
        [Authorize]
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Staff") && claim.UserID != currentUserId)
            {
                return Forbid();
            }

            // load dropdown of lost items (sorted alphabetically)
            ViewData["MatchedLostItemID"] = new SelectList(
   _context.LostItem
   .Where(l => !l.IsClaimed)   // only show available items
   .OrderBy(l => l.ItemName),
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
        [Authorize]
        public async Task<IActionResult> Edit(int id,
         [Bind("ClaimID,ClaimedItemName,ClaimedDescription,DateLost,MatchedLostItemID,Status")]
    APS_LostProperty.Models.Claim claim)
        {
            if (id != claim.ClaimID)
                return NotFound();

            var existing = await _context.Claim
                .Include(c => c.IdentityUser)
                .FirstOrDefaultAsync(c => c.ClaimID == id);

            if (existing == null)
                return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Staff") && existing.UserID != currentUserId)
            {
                return Forbid();
            }

            var oldStatus = existing.Status;
            var oldLostItemId = existing.MatchedLostItemID;

            ModelState.Remove("UserID");

            var today = DateTime.Today;
            var oneYearAgo = today.AddMonths(-3);

            if (claim.DateLost > today)
                ModelState.AddModelError("DateLost", "Date Lost cannot be in The Future! ");

            if (claim.DateLost < oneYearAgo)
                ModelState.AddModelError("DateLost", "Date Lost cannot be more than 3 months in the Past!");

            if (ModelState.IsValid)
            {
                existing.ClaimedItemName = claim.ClaimedItemName;
                existing.ClaimedDescription = claim.ClaimedDescription;
                existing.DateLost = claim.DateLost;

                if (User.IsInRole("Staff"))
                {
                    existing.MatchedLostItemID = claim.MatchedLostItemID;
                    existing.Status = claim.Status;
                }


                if (oldLostItemId != null && oldLostItemId != existing.MatchedLostItemID)
                {
                    var oldItem = await _context.LostItem
                        .FirstOrDefaultAsync(l => l.LostItemID == oldLostItemId);

                    if (oldItem != null)
                    {
                        oldItem.IsClaimed = false;
                    }
                }

              
                if (existing.MatchedLostItemID != null)
                {
                    var lostItem = await _context.LostItem
                        .FirstOrDefaultAsync(l => l.LostItemID == existing.MatchedLostItemID);

                    if (lostItem != null)
                    {
                        lostItem.IsClaimed = claim.Status == ClaimStatus.Collected;
                    }
                }

                await _context.SaveChangesAsync();

          
                if (oldStatus != ClaimStatus.Approved && claim.Status == ClaimStatus.Approved)
                {
                    var user = await _userManager.FindByIdAsync(existing.UserID);

                    if (user?.Email != null)
                    {
                        await _emailSender.SendEmailAsync(
                        user.Email,
                      "Claim Approved - " + claim.ClaimedItemName,$"Dear {user.UserName},\n\n" +$"We are pleased to inform you that your claim for '{existing.ClaimedItemName}' has been approved.\n\n" +
                      "Your item is now ready for collection from the College Shop.\n\n" +
                      "Please bring a form of identification when collecting your item.\n\n" +
                      "If you have any questions, please contact the APS Lost Property Team.\n\n" +
                      "Kind regards,\n" +
                      "APS Lost Property Team"

                        );
                    }
                }

             
              
                if (oldStatus != ClaimStatus.Rejected && claim.Status == ClaimStatus.Rejected)
                {
                    await _emailSender.SendEmailAsync(
                        existing.IdentityUser.Email,
                        "Claim Rejected - " + existing.ClaimedItemName,
                        $"Dear {existing.IdentityUser.UserName},\n\n" +
                        $"Unfortunately, your claim for '{existing.ClaimedItemName}' has not been approved.\n\n" +
                        "This may be because the information provided did not match the item or additional verification was required.\n\n" +
                        "If you believe this decision was made in error, please contact the APS Lost Property Team for further assistance.\n\n" +
                        "Kind regards,\n" +
                        "APS Lost Property Team"


                    );
                }

                return RedirectToAction(nameof(Index));
            }

            // reload dropdown
            ViewData["MatchedLostItemID"] = new SelectList(
                _context.LostItem.Where(l => !l.IsClaimed).OrderBy(l => l.ItemName),
                "LostItemID",
                "ItemName",
                claim.MatchedLostItemID
            );

            return View(claim);
        }
        // GET: Delete
        // shows confirmation page before deleting claim
        [HttpGet]
        [Authorize]
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!User.IsInRole("Staff") && claim.UserID != currentUserId)
            {
                return Forbid();
            }
            return View(claim);
        }

        // POST: Delete
        // permanently removes claim from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claim
     .Include(c => c.IdentityUser)
     .Include(c => c.MatchedLostItem)
     .FirstOrDefaultAsync(c => c.ClaimID == id);
            if (claim != null)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!User.IsInRole("Staff") && claim.UserID != currentUserId)
                {
                    return Forbid();
                }
                _context.Claim.Remove(claim);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}