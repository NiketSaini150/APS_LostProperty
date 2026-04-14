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
        // database connection (Dependency Injection)
        private readonly DBContext _context;

        // constructor runs when controller is created
        // gives us access to database
        public LostItemsController(DBContext context)
        {
            _context = context;
        }

        // GET: LostItems
        // shows list of all lost items + search + pagination
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            // number of items shown per page
            int pageSize = 5;

            // start building query from LostItem table
            // IQueryable allows filtering BEFORE hitting database
            var lostItemsQuery = _context.LostItem
                .Include(lm => lm.Category)   // loads category data for each item
                .Include(lm => lm.Location)   // loads location data for each item
                .AsQueryable();               // allows chaining filters

            // ---------------- SEARCH ----------------

            // checks if user typed anything in search box
            if (!string.IsNullOrEmpty(searchString))
            {
                // converts search text to lowercase for case-insensitive search
                var search = searchString.ToLower();

                // filters items where ItemName starts with search text
                lostItemsQuery = lostItemsQuery.Where(lm =>
                    lm.ItemName.ToLower().StartsWith(search)
                );
            }

            // ---------------- SORTING ----------------

            // sorts results so matching/starting letters appear first
            lostItemsQuery = lostItemsQuery
                .OrderBy(l =>
                    l.ItemName.StartsWith(searchString) ? 0 : 1 // prioritise matches
                )
                .ThenBy(l => l.ItemName); // then sort alphabetically

            // store search value so it stays in textbox after refresh
            ViewData["CurrentFilter"] = searchString;

            // count total records AFTER filtering
            var totalItems = await lostItemsQuery.CountAsync();

            // calculate how many pages we need
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // get only records for current page
            var lostItems = await lostItemsQuery
                .Skip((page - 1) * pageSize)  // skip previous pages
                .Take(pageSize)               // take only current page items
                .ToListAsync();               // execute query

            // send pagination info to view
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;

            // return list to view
            return View(lostItems);
        }

        // GET: Create page
        // shows form for adding new lost item
        public IActionResult Create()
        {
            // dropdown for categories (sorted alphabetically)
            ViewData["CategoryID"] = new SelectList(
                _context.Set<Category>().OrderBy(c => c.Name),
                "CategoryID",
                "Name"
            );

            // dropdown for locations (sorted alphabetically)
            ViewData["LocationID"] = new SelectList(
                _context.Set<Location>().OrderBy(l => l.LocationName),
                "LocationID",
                "LocationName"
            );

            return View();
        }

        // POST: Create
        // saves new lost item into database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LostItemID,ItemName,Description,DateFound,CategoryID,LocationID,IsClaimed")] LostItem lostItem)
        {
            // checks if all validation rules pass
            if (ModelState.IsValid)
            {
                // adds new item into database
                _context.Add(lostItem);

                // saves changes permanently
                await _context.SaveChangesAsync();

                // redirects back to list page
                return RedirectToAction(nameof(Index));
            }

            // if validation fails, reload dropdowns again

            ViewData["CategoryID"] = new SelectList(
                _context.Set<Category>().OrderBy(c => c.Name),
                "CategoryID",
                "Name",
                lostItem.CategoryID
            );

            ViewData["LocationID"] = new SelectList(
                _context.Set<Location>().OrderBy(l => l.LocationName),
                "LocationID",
                "LocationName",
                lostItem.LocationID
            );

            // return same page with errors
            return View(lostItem);
        }

        // GET: Edit page
        // loads existing item into form
        public async Task<IActionResult> Edit(int? id)
        {
            // checks if id was provided
            if (id == null)
            {
                return NotFound();
            }

            // finds item in database by id
            var lostItem = await _context.LostItem.FindAsync(id);

            // if not found return error
            if (lostItem == null)
            {
                return NotFound();
            }

            // reload dropdowns with current selected values
            ViewData["CategoryID"] = new SelectList(
                _context.Set<Category>().OrderBy(c => c.Name),
                "CategoryID",
                "Name",
                lostItem.CategoryID
            );

            ViewData["LocationID"] = new SelectList(
                _context.Set<Location>().OrderBy(l => l.LocationName),
                "LocationID",
                "LocationName",
                lostItem.LocationID
            );

            return View(lostItem);
        }

        // POST: Edit
        // updates existing lost item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LostItemID,ItemName,Description,DateFound,CategoryID,LocationID,IsClaimed")] LostItem lostItem)
        {
            // checks if URL id matches form id
            if (id != lostItem.LostItemID)
            {
                return NotFound();
            }

            // checks validation rules
            if (ModelState.IsValid)
            {
                try
                {
                    // updates record in database
                    _context.Update(lostItem);

                    // saves changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // checks if item still exists
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

            // reload dropdowns if validation fails

            ViewData["CategoryID"] = new SelectList(
               _context.Set<Category>().OrderBy(c => c.Name),
               "CategoryID",
               "Name",
               lostItem.CategoryID
           );

            ViewData["LocationID"] = new SelectList(
                _context.Set<Location>().OrderBy(l => l.LocationName),
                "LocationID",
                "LocationName",
                lostItem.LocationID
            );

            return View(lostItem);
        }

        // GET: Delete page
        // shows confirmation before deleting
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // includes category and location details
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

        // POST: Delete
        // actually deletes item from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lostItem = await _context.LostItem.FindAsync(id);

            // remove if exists
            if (lostItem != null)
            {
                _context.LostItem.Remove(lostItem);
            }

            // save changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // helper method
        // checks if lost item exists in database
        private bool LostItemExists(int id)
        {
            return _context.LostItem.Any(e => e.LostItemID == id);
        }
    }
}