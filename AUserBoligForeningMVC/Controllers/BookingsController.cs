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
using System.Globalization;

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
                if (DateTime.ParseExact(model.Date, "M/d/yyyy", CultureInfo.InvariantCulture) > DateTime.Now)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                }

            }
            return Json(booking);
        }




        public async Task<IActionResult> FaellesBil()
        {
            var EventsFromDb = await _context.bookings.Where(events => 
            events.Calendar == "Bil").ToListAsync();
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
                if (DateTime.ParseExact(model.Date, "M/d/yyyy", CultureInfo.InvariantCulture) > DateTime.Now)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                }

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
                if (DateTime.ParseExact(model.Date, "M/d/yyyy", CultureInfo.InvariantCulture) > DateTime.Now)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                }

            }
            return Json(booking);
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
                if (EventUser.CurrentUserMail == mail || User.IsInRole("Admin"))
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
