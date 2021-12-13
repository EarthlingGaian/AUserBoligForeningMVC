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
    public class DokumentArkivsController : Controller
    {
        private readonly UserContext _context;
        private static UserManager<IdentityUser> _userManager;
        public DokumentArkivsController(UserContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        // GET: DokumentArkivs
        public async Task<IActionResult> Index()
        {
            var pdfFile = _context.dokumentArkivs.FirstOrDefault();
            return View(pdfFile);
        }


        
        public async Task<IActionResult> Lejekontrakt()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            string mail = user?.Email;

            var pdfFile = await _context.dokumentArkivs.FirstOrDefaultAsync(m => m.BeboerMail == mail);

            return View(pdfFile);
        }
        public async Task<IActionResult> Indflytningspapir()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            string mail = user?.Email;

            var pdfFile = await _context.dokumentArkivs.FirstOrDefaultAsync(m => m.BeboerMail == mail);

            return View(pdfFile);
        }


        public async Task<IActionResult> LejekontraktCreate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentArkiv = await _context.lejers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dokumentArkiv == null)
            {
                return NotFound();
            }

            return View(dokumentArkiv);
        }

        // POST: DokumentArkivs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LejekontraktCreate(int? id, DokumentArkiv dokumentArkiv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dokumentArkiv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dokumentArkiv);
        }


        // GET: DokumentArkivs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentArkiv = await _context.dokumentArkivs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dokumentArkiv == null)
            {
                return NotFound();
            }

            return View(dokumentArkiv);
        }



        // GET: DokumentArkivs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DokumentArkivs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LejeKontrakt,IndflytningsPapir,BeboerMail")] DokumentArkiv dokumentArkiv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dokumentArkiv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dokumentArkiv);
        }

        // GET: DokumentArkivs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentArkiv = await _context.dokumentArkivs.FindAsync(id);
            if (dokumentArkiv == null)
            {
                return NotFound();
            }
            return View(dokumentArkiv);
        }

        // POST: DokumentArkivs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LejeKontrakt,IndflytningsPapir,BeboerMail")] DokumentArkiv dokumentArkiv)
        {
            if (id != dokumentArkiv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dokumentArkiv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DokumentArkivExists(dokumentArkiv.Id))
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
            return View(dokumentArkiv);
        }

        // GET: DokumentArkivs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dokumentArkiv = await _context.dokumentArkivs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dokumentArkiv == null)
            {
                return NotFound();
            }

            return View(dokumentArkiv);
        }

        // POST: DokumentArkivs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dokumentArkiv = await _context.dokumentArkivs.FindAsync(id);
            _context.dokumentArkivs.Remove(dokumentArkiv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DokumentArkivExists(int id)
        {
            return _context.dokumentArkivs.Any(e => e.Id == id);
        }
    }
}
