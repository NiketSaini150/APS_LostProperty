using APS_LostProperty.Areas.Identity.Data;
using APS_LostProperty.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APS_LostProperty.Controllers
{
    [Authorize(Roles = "Staff")]
    public class LocationsController : Controller
    {
        // database connection (dependency injection)
        private readonly DBContext _context;

        // constructor runs when controller is created
        public LocationsController(DBContext context)
        {
            _context = context;
        }

        // GET: Locations
        // shows list of locations with search + pagination
        public async Task<IActionResult> Index(string Searchstring, int page = 1)
        {
            // number of items per page
            int pagesize = 5;

            // start query from Location table
            var LocationQuery = _context.Location.AsQueryable();

            // ---------------- SEARCH ----------------

            // checks if user typed anything in search box
            if (!string.IsNullOrEmpty(Searchstring))
            {
                // convert search to lowercase for case-insensitive search
                var search = Searchstring.ToLower();

                // filter locations where name contains search text
                LocationQuery = LocationQuery.Where(l =>
                    l.LocationName != null &&
                    l.LocationName.ToLower().Contains(search)
                );
            }

            // ---------------- SORTING ----------------

            // sorts so matching/starting results appear first
            LocationQuery = LocationQuery
                          .OrderBy(c =>
                              c.LocationName != null &&
                              Searchstring != null &&
                              c.LocationName.ToLower().StartsWith(Searchstring.ToLower())
                                  ? 0
                                  : 1 // push matches to top
                          )
                          .ThenBy(c => c.LocationName); // alphabetical order

            // count total results AFTER filtering
            var totalItems = await LocationQuery.CountAsync();

            // calculate total pages needed
            var totalPages = (int)Math.Ceiling(totalItems / (double)pagesize);

            // fix page if user goes out of range
            if (page < 1)
                page = 1;

            if (page > totalPages && totalPages > 0)
                page = totalPages;

            // get only items for current page
            var locations = await LocationQuery
                .Skip((page - 1) * pagesize)   // skip previous pages
                .Take(pagesize)                // take current page items
                .ToListAsync();               // execute query

            // send pagination data to view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // keep search text in textbox after refresh
            ViewBag.searchstring = Searchstring;

            // return data to view
            return View(locations);
        }

        // GET: Details
        // shows one location
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.LocationID == id);

            if (location == null)
                return NotFound();

            return View(location);
        }

        // GET: Create
        // shows create form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        // saves new location
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationID,LocationName")] Location location)
        {
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(location);
        }

        // GET: Edit
        // loads existing location into form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var location = await _context.Location.FindAsync(id);

            if (location == null)
                return NotFound();

            return View(location);
        }

        // POST: Edit
        // updates location in database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationID,LocationName")] Location location)
        {
            if (id != location.LocationID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.LocationID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(location);
        }

        // GET: Delete
        // confirms delete action
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.LocationID == id);

            if (location == null)
                return NotFound();

            return View(location);
        }

        // POST: Delete
        // actually deletes from database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Location.FindAsync(id);

            if (location != null)
            {
                _context.Location.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // helper method
        // checks if location exists in database
        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.LocationID == id);
        }
    }
}