using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AUserBoligForeningMVC.Data;
using AUserBoligForeningMVC.Models;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace AUserBoligForeningMVC.Controllers
{
    public class RegningersController : Controller
    {
        private readonly UserContext _context;

        private static UserManager<IdentityUser> _userManager;

        public RegningersController(UserContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Regningers
        public async Task<IActionResult> Index(string search)
        
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

      
            string mail = user?.Email;

         

            List<Regninger> modelList = new List<Regninger>();
            var bookingsModel = await _context.bookings.Where(a => a.CurrentUserMail == mail).ToListAsync();

            foreach (var OneBooking in bookingsModel)
            {

                if (DateTime.ParseExact(OneBooking.Date, "M/d/yyyy", CultureInfo.InvariantCulture) < DateTime.Now)
                {
                    var price = 0;

                    if (OneBooking.Calendar == "Kaekken" || OneBooking.Calendar == "Lokale")
                    {
                        price = 5;
                    }
                    else if (OneBooking.Calendar == "Bil")
                    {
                        price = 50;
                    }

                    Regninger model = new Regninger
                    {
                        Regning = price.ToString(),
                        BeboerMail = OneBooking.CurrentUserMail,
                        Date = OneBooking.Date,
                        Calendar = OneBooking.Calendar
                    };



                    

                    _context.Add(model);
                    await _context.SaveChangesAsync();

                    _context.bookings.Remove(OneBooking);
                    await _context.SaveChangesAsync();

                    modelList.Add(model);
                }

            }

            var regninger = await _context.regningers.Where(a => a.BeboerMail == mail).Where(a => search == null || a.Calendar.Contains(search) || a.Date.Contains(search)).ToListAsync();
             
            return View(regninger);
        }


        public async Task<IActionResult> AdminIndex(string search)
        {             
            return View(await _context.regningers.Where(a => a.BeboerMail.Contains(search) || search == null || a.Calendar.Contains(search) || a.Date.Contains(search)).ToListAsync());
        }


            // GET: Regningers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regninger = await _context.regningers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (regninger == null)
            {
                return NotFound();
            }

            return View(regninger);
        }

        // GET: Regningers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regningers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Regning,BeboerMail,Date,Calendar")] Regninger regninger)
        {
            if (ModelState.IsValid)
            {
                _context.Add(regninger);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(regninger);
        }

        // GET: Regningers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regninger = await _context.regningers.FindAsync(id);
            if (regninger == null)
            {
                return NotFound();
            }
            return View(regninger);
        }

        // POST: Regningers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Regning,BeboerMail,Date,Calendar")] Regninger regninger)
        {
            if (id != regninger.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(regninger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegningerExists(regninger.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(regninger);
        }

        // GET: Regningers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regninger = await _context.regningers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (regninger == null)
            {
                return NotFound();
            }

            return View(regninger);
        }

        // POST: Regningers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var regninger = await _context.regningers.FindAsync(id);
            _context.regningers.Remove(regninger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        private bool RegningerExists(int id)
        {
            return _context.regningers.Any(e => e.Id == id);
        }
    }
}
