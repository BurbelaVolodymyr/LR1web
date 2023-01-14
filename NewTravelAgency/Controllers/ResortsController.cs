using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTravelAgency;
using NewTravelAgency.Models;

namespace NewTravelAgency.Controllers
{
    [Authorize]
    public class ResortsController : Controller
    {
        private readonly TravelAgencyContext _context;

        public ResortsController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: Resorts
        public async Task<IActionResult> Index()
        {
            var travelAgencyContext = _context.Resorts.Include(r => r.Country);
            return View(await travelAgencyContext.ToListAsync());
        }

        // GET: Resorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resorts == null)
            {
                return NotFound();
            }

            var resort = await _context.Resorts
                .Include(r => r.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resort == null)
            {
                return NotFound();
            }

            return View(resort);
        }
        [Authorize(Roles = "AdminRole")]
        // GET: Resorts/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "AdminRole")]
        // POST: Resorts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryId,Name")] Resort resort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", resort.CountryId);
            return View(resort);
        }
        [Authorize(Roles = "AdminRole")]
        // GET: Resorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Resorts == null)
            {
                return NotFound();
            }

            var resort = await _context.Resorts.FindAsync(id);
            if (resort == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", resort.CountryId);
            return View(resort);
        }
        [Authorize(Roles = "AdminRole")]
        // POST: Resorts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CountryId,Name")] Resort resort)
        {
            if (id != resort.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resort);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResortExists(resort.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", resort.CountryId);
            return View(resort);
        }
        [Authorize(Roles = "AdminRole")]
        // GET: Resorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Resorts == null)
            {
                return NotFound();
            }

            var resort = await _context.Resorts
                .Include(r => r.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resort == null)
            {
                return NotFound();
            }

            return View(resort);
        }
        [Authorize(Roles = "AdminRole")]
        // POST: Resorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Resorts == null)
            {
                return Problem("Entity set 'TravelAgencyContext.Resorts'  is null.");
            }
            var resort = await _context.Resorts.FindAsync(id);
            if (resort != null)
            {
                _context.Resorts.Remove(resort);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResortExists(int id)
        {
          return _context.Resorts.Any(e => e.Id == id);
        }
    }
}
