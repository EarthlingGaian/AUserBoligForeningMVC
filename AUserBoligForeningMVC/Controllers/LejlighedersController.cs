using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AUserBoligForeningMVC.Data;
using AUserBoligForeningMVC.Models;

namespace AUserBoligForeningMVC.Controllers
{
    public class LejlighedersController : Controller
    {
        private readonly UserContext _context;

        public LejlighedersController(UserContext context)
        {
            _context = context;
        }

        // GET: Lejligheders
        public async Task<IActionResult> Index(string search)
        {
            
            return View(await _context.Lejligheder.Where(a => a.Adresse.Contains(search) || search == null).ToListAsync());
        }

        // GET: Lejligheders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lejligheder = await _context.Lejligheder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lejligheder == null)
            {
                return NotFound();
            }

            return View(lejligheder);
        }

        // GET: Lejligheders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lejligheders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adresse")] Lejligheder lejligheder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lejligheder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lejligheder);
        }

        // GET: Lejligheders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lejligheder = await _context.Lejligheder.FindAsync(id);
            if (lejligheder == null)
            {
                return NotFound();
            }
            return View(lejligheder);
        }

        // POST: Lejligheders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adresse")] Lejligheder lejligheder)
        {
            if (id != lejligheder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lejligheder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LejlighederExists(lejligheder.Id))
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
            return View(lejligheder);
        }

        // GET: Lejligheders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lejligheder = await _context.Lejligheder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lejligheder == null)
            {
                return NotFound();
            }

            return View(lejligheder);
        }

        // POST: Lejligheders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lejligheder = await _context.Lejligheder.FindAsync(id);
            _context.Lejligheder.Remove(lejligheder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LejlighederExists(int id)
        {
            return _context.Lejligheder.Any(e => e.Id == id);
        }
    }
}
