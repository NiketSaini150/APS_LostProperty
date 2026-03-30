using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Models;

namespace APS_LostProperty.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly DBContext _context;

        public ClaimsController(DBContext context)
        {
            _context = context;
        }

        // GET: Claims
        public async Task<IActionResult> Index(string searchString)
        {
            // Start query
            var claims = _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .AsQueryable();

            // Apply search if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                claims = claims.Where(c => c.IdentityUser.Email.ToLower().Contains(searchString.ToLower())
                                        || c.IdentityUser.UserName.ToLower().Contains(searchString.ToLower()));
            }

            ViewData["CurrentFilter"] = searchString;

            return View(await claims.ToListAsync());
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .FirstOrDefaultAsync(m => m.ClaimID == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName");
            return View();
        }
       

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimID,UserID,ItemName,Description,DateLost,DateSubmitted,Status,MatchedLostItemID")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", claim.UserID);
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);
            return View(claim);
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Email", claim.UserID);
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimID,UserID,ItemName,Description,DateLost,DateSubmitted,Status,MatchedLostItemID")] Claim claim)
        {
            if (id != claim.ClaimID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimExists(claim.ClaimID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", claim.UserID);
            ViewData["MatchedLostItemID"] = new SelectList(_context.LostItem, "LostItemID", "ItemName", claim.MatchedLostItemID);
            return View(claim);
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim
                .Include(c => c.IdentityUser)
                .Include(c => c.MatchedLostItem)
                .FirstOrDefaultAsync(m => m.ClaimID == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claim.FindAsync(id);
            if (claim != null)
            {
                _context.Claim.Remove(claim);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaimExists(int id)
        {
            return _context.Claim.Any(e => e.ClaimID == id);
        }
    }
}
