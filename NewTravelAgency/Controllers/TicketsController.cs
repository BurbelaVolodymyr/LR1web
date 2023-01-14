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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NewTravelAgency.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly TravelAgencyContext _context;

        public TicketsController(TravelAgencyContext context)
        {
            _context = context;
        }

        // GET: Tickets
        //public async Task<IActionResult> Index()
        //{
        //    var travelAgencyContext = _context.Tickets.Include(t => t.Hotel).ThenInclude(t => t.Resort).ThenInclude(t => t.Country).Include(t => t.Ordering);    
        //    return View(await travelAgencyContext.ToListAsync());

        //}

        public async Task<IActionResult> Index(int page = 1, int hotel = 0,
    SortTicket sortOrder = SortTicket.HotelNameAsc)
        {
            int pageSize = 10;

            // фільтрація
            IQueryable<Ticket> tickets = _context.Tickets.Include(t => t.Hotel).ThenInclude(r => r.Resort).ThenInclude(c => c.Country).Include(t => t.Ordering);

            if (hotel != 0)
            {
                tickets = tickets.Where(p => p.HotelId == hotel);
            }

            // сортування
            switch (sortOrder)
            {
                case SortTicket.HotelNameDesc:
                    tickets = tickets.OrderByDescending(s => s.Hotel!.Name);
                    break;
                case SortTicket.CostAsc:
                    tickets = tickets.OrderBy(s => s.Cost);
                    break;
                case SortTicket.CostDesc:
                    tickets = tickets.OrderByDescending(s => s.Cost);
                    break;
                default:
                    tickets = tickets.OrderBy(s => s.Hotel!.Name);
                    break;
            }

            // пагінація
            var count = await tickets.CountAsync();
            var items = await tickets.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формуємо модель представлення
            IndexViewModel viewModel = new IndexViewModel(
            items,
            new PageViewModel(count, page, pageSize),
                new FilterViewModel(_context.Hotels.ToList(), hotel),
                new SortViewModel(sortOrder)
            );
            return View(viewModel);
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
        [Authorize(Roles = "AdminRole")]
        // GET: Tickets/Create
        public IActionResult Create()
        {
            ///////////     ////    //////

            ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
            ViewData["OrderingId"] = new SelectList(_context.Orderings, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
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



    }
}
