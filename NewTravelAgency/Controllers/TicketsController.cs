using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewTravelAgency;
using NewTravelAgency.Models;

namespace NewTravelAgency.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TravelAgencyContext _context;

        public TicketsController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var travelAgencyContext = _context.Tickets.Include(t => t.Hotel).Include(t => t.Ordering);
            return View(await travelAgencyContext.ToListAsync());

        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Hotel)
                .Include(t => t.Ordering)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ///////////     ////    //////

            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
            ViewData["OrderingId"] = new SelectList(_context.Orderings, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderingId,HotelId,TimeDeparture,TimeArrival,Cost,TouristNumber")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Id", ticket.HotelId);
            ViewData["OrderingId"] = new SelectList(_context.Orderings, "Id", "Id", ticket.OrderingId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ///////////     ////    //////

            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", ticket.HotelId);
            ViewData["OrderingId"] = new SelectList(_context.Orderings, "Id", "Name", ticket.OrderingId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderingId,HotelId,TimeDeparture,TimeArrival,Cost,TouristNumber")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Id", ticket.HotelId);
            ViewData["OrderingId"] = new SelectList(_context.Orderings, "Id", "Id", ticket.OrderingId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Hotel)
                .Include(t => t.Ordering)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'TravelAgencyContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        // Sort
        public async Task<IActionResult> Index(SortTicket sortOrder = SortTicket.HotelNameAsc)
        {
            IQueryable<Ticket> tickets = _context.Tickets.Include(x => x.Hotel);

            ViewData["HotelNameSort"] = sortOrder == SortTicket.HotelNameAsc ? SortTicket.HotelNameDesc : SortTicket.HotelNameAsc;
            ViewData["CostSort"] = sortOrder == SortTicket.CostAsc ? SortTicket.CostDesc : SortTicket.CostAsc;

            tickets = sortOrder switch
            {
                SortTicket.HotelNameDesc => tickets.OrderByDescending(s => s.Hotel!.Name),
                SortTicket.CostAsc => tickets.OrderBy(s => s.Cost),
                SortTicket.CostDesc => tickets.OrderByDescending(s => s.Cost),
                _ => tickets.OrderBy(s => s.Cost),
            };

            return View(await tickets.AsNoTracking().ToListAsync());
        }

    }
}
