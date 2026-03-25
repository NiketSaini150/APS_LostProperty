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
    public class LostItemsController : Controller
    {
        private readonly DBContext _context;

        public LostItemsController(DBContext context)
        {
            _context = context;
        }

        // GET: LostItems

        public async Task<IActionResult> Index(string searchString)
        {
            // Start query including Category and Location
            var lostItems = _context.LostItem
                                .Include(lm => lm.Category)
                                .Include(lm => lm.Location)
                                .AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                lostItems = lostItems.Where(lm => lm.ItemName.ToLower().StartsWith(searchString.ToLower()));
            }

            ViewData["CurrentFilter"] = searchString;

            // Execute the query
            return View(await lostItems.ToListAsync());
        }
        // GET: LostItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItem
                .Include(l => l.Category)
                .Include(l => l.Location)
                .FirstOrDefaultAsync(m => m.LostItemID == id);
            if (lostItem == null)
            {
                return NotFound();
            }

            return View(lostItem);
        }

        // GET: LostItems/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "CategoryID", "Name");
            ViewData["LocationID"] = new SelectList(_context.Set<Location>(), "LocationID", "LocationName");
            return View();
        }

        // POST: LostItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LostItemID,ItemName,Description,DateFound,CategoryID,LocationID,IsClaimed")] LostItem lostItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lostItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "CategoryID", "Name", lostItem.CategoryID);
            ViewData["LocationID"] = new SelectList(_context.Set<Location>(), "LocationID", "LocationName", lostItem.LocationID);
            return View(lostItem);
        }

        // GET: LostItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItem.FindAsync(id);
            if (lostItem == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "CategoryID", "Name", lostItem.CategoryID);
            ViewData["LocationID"] = new SelectList(_context.Set<Location>(), "LocationID", "LocationName", lostItem.LocationID);
            return View(lostItem);
        }

        // POST: LostItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LostItemID,ItemName,Description,DateFound,CategoryID,LocationID,IsClaimed")] LostItem lostItem)
        {
            if (id != lostItem.LostItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lostItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostItemExists(lostItem.LostItemID))
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
            ViewData["CategoryID"] = new SelectList(_context.Set<Category>(), "CategoryID", "Name", lostItem.CategoryID);
            ViewData["LocationID"] = new SelectList(_context.Set<Location>(), "LocationID", "LocationName", lostItem.LocationID);
            return View(lostItem);
        }

        // GET: LostItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lostItem = await _context.LostItem
                .Include(l => l.Category)
                .Include(l => l.Location)
                .FirstOrDefaultAsync(m => m.LostItemID == id);
            if (lostItem == null)
            {
                return NotFound();
            }

            return View(lostItem);
        }

        // POST: LostItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lostItem = await _context.LostItem.FindAsync(id);
            if (lostItem != null)
            {
                _context.LostItem.Remove(lostItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LostItemExists(int id)
        {
            return _context.LostItem.Any(e => e.LostItemID == id);
        }
    }
}
