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
    public class CategoriesController : Controller
    {
    
      
            private readonly DBContext _context;

        public CategoriesController(DBContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            // number of categories per page
            int pageSize = 5;

            // start query from Category table
            var categoriesQuery = _context.Category.AsQueryable();

            // ---------------- SEARCH ----------------

            // check if user typed anything in search bar
            if (!string.IsNullOrEmpty(searchString))
            {
                // make search lowercase so it ignores case sensitivity
                var search = searchString.ToLower();

                // filter categories by name
                categoriesQuery = categoriesQuery.Where(c =>
                    c.Name != null &&                      // make sure name exists
                    c.Name.ToLower().Contains(search)      // check if it contains search text
                );
            }

          
            // sort results so better matches appear first
            categoriesQuery = categoriesQuery
                .OrderBy(c =>
                    c.Name.StartsWith(searchString) ? 0 : 1 // exact/starting matches first
                )
                .ThenBy(c => c.Name); // then alphabetical order

            // ---------------- PAGINATION ----------------

            // count total categories after filtering
            var totalItems = await categoriesQuery.CountAsync();

            // calculate total pages needed
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // get only items for current page
            var categories = await categoriesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // send values to view
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentFilter"] = searchString;

            return View(categories);
        }
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,Name")] Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }
    }
}
