using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AUserBoligForeningMVC.Data;
using AUserBoligForeningMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace AUserBoligForeningMVC.Controllers
{
    public class BookingsController : Controller
    {
        private readonly UserContext _context;
        private static UserManager<IdentityUser> _userManager;
        public BookingsController(UserContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> ErrorWrongUser()
        {
            return View();
        }

        public IActionResult FaellesKaekken()
        {
            var EventsFromDb = _context.bookings.Where(events => events.Calendar == "Kaekken").ToList();
            return View(EventsFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> FaellesKaekken(Booking booking)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

           
            string mail = user?.Email;

            var model = new Booking
            {
                Id = booking.Id,
                Date = booking.Date,
                Title = booking.Title,
                Calendar = "Kaekken",
                CurrentUserMail = mail
            };

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

            }
            return Json(booking);
        }




        public async Task<IActionResult> FaellesBil()
        {
            var EventsFromDb = _context.bookings.Where(events => events.Calendar == "Bil").ToList();
            return View(EventsFromDb);
        }
        [HttpPost]
        public async Task<IActionResult> FaellesBil(Booking booking)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            string mail = user?.Email;
            var model = new Booking
            {
                Id = booking.Id,
                Date = booking.Date,
                Title = booking.Title,
                Calendar = "Bil",
                CurrentUserMail = mail
            };

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

            }
            return Json(booking);
        }




        public async Task<IActionResult> FaellesLokale()
        {
            var EventsFromDb = _context.bookings.Where(events => events.Calendar == "Lokale").ToList();
            return View(EventsFromDb);
        }


        [HttpPost]
        public async Task<IActionResult> FaellesLokale(Booking booking)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            string mail = user?.Email;
            var model = new Booking
            {
                Id = booking.Id,
                Date = booking.Date,
                Title = booking.Title,
                Calendar = "Lokale",
                CurrentUserMail = mail
            };

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

            }
            return Json(booking);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Title")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Title")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.bookings.FindAsync(id);
            _context.bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.bookings.Any(e => e.Id == id);
        }

        public async Task<IActionResult> DeleteConfirmedEvent(Booking b)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            string mail = user?.Email;

            var EventUser = await _context.bookings
               .FirstOrDefaultAsync(m => m.Date == b.Date && m.Calendar == b.Calendar);
            if (EventUser != null)
            {
                if (EventUser.CurrentUserMail == mail)
                {
               
                    _context.bookings.Remove(EventUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
    }
}
