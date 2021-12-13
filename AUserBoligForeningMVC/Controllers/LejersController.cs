using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AUserBoligForeningMVC.Data;
using AUserBoligForeningMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace AUserBoligForeningMVC.Controllers
{
    public class LejersController : Controller
    {
        private readonly UserContext _context;
        private static UserManager<IdentityUser> _userManager;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment;
        
        public LejersController(UserContext context, UserManager<IdentityUser> userManager, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Lejers
        public async Task<IActionResult> Index(string search)
        {

            return View(await _context.lejers.Where(a => a.Adresse.Contains(search) || search == null || a.Fornavn.Contains(search) || a.Email.Contains(search)).ToListAsync());
        }

        public async Task<IActionResult> UploadDokumenter(int? id, DokumentArkivViewModel model)
        {

            var lejerMail = _context.lejers.Where(e => e.Id == id).Select(m => m.Email).FirstOrDefault();

            if (ModelState.IsValid)
            {
                string uniqueFileNameLejeKontrakt = null;
                string uniqueFileNameIndflytningsPapir = null;
                if (model.LejeKontrakt != null && model.IndflytningsPapir != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "img"); //output at path as string
                   
                    //save lejekontrakt
                    uniqueFileNameLejeKontrakt =  Guid.NewGuid().ToString() + "_" + model.LejeKontrakt.FileName;
                    string filePathLejeKontrakt = Path.Combine(uploadFolder, uniqueFileNameLejeKontrakt);

                    model.LejeKontrakt.CopyTo(new FileStream(filePathLejeKontrakt, FileMode.Create));


                    //save indlytningspapir
                    uniqueFileNameIndflytningsPapir = Guid.NewGuid().ToString() + "_" + model.IndflytningsPapir.FileName;
                    string filePathIndflytningsPapir = Path.Combine(uploadFolder, uniqueFileNameIndflytningsPapir);

                    model.LejeKontrakt.CopyTo(new FileStream(filePathIndflytningsPapir, FileMode.Create));
                }

                var newDokArkiv = new DokumentArkiv
                {
                    LejeKontrakt = uniqueFileNameLejeKontrakt,
                    IndflytningsPapir = uniqueFileNameIndflytningsPapir,
                    BeboerMail = lejerMail
                    
                };

                _context.Add(newDokArkiv);
                await _context.SaveChangesAsync();

            }
                return View();
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userId = user?.Id;
            string mail = user?.Email;

            var userData = _context.lejers.Where(a => a.Email == mail).FirstOrDefault();

            
            

            return View(userData);
        }


        public async Task<IActionResult> EditBrugerSettings(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bruger = await _context.lejers.FindAsync(id);

            if (bruger == null)
            {
                return NotFound();
            }


            var model = new Lejer()
            {

                Fornavn = bruger.Fornavn,
                Efternavn = bruger.Efternavn,
                TlfNr = bruger.TlfNr
                
            };


            return View(model);
        }

        //user in user profile ->user settings edit
        // POST: Brugers/EditBrugerSettings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBrugerSettings(int id, Lejer model)
        {
            //int BId = id;
            if (id != model.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userId = user?.Id;
            string mail = user?.Email;

            var bruger = _context.lejers.Where(a => a.Email == mail).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {

                    bruger.Fornavn = model.Fornavn;
                    bruger.Efternavn = model.Efternavn;
                    bruger.TlfNr = model.TlfNr;
                    bruger.Alder = model.Alder;

                    _context.Update(bruger);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LejerExists(bruger.Id))
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
            return View(bruger);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SignUp()
        {
            //Brugere som ikke er registreret som lejer
            var lejers = await _context.lejers.Select(u => u.Email).ToListAsync();
            var usersNotLejers = await _userManager.Users.Where(m => !lejers.Contains(m.Email)).ToListAsync(); //get all emails that are not already a lejer

            List<SelectListItem> listLejers = new List<SelectListItem>();

            foreach(var email in usersNotLejers)
            {
                SelectListItem selListItem = new SelectListItem() { Value = "null", Text = email.ToString() };
                listLejers.Add(selListItem);
            }



            //Lejlgheder som ikke er lejet ud
            var lejligheder = await _context.lejers.Select(l => l.Adresse).ToListAsync();

            var ledigeLejligheder = await _context.Lejligheder.Where(m => !lejligheder.Contains(m.Adresse))
                .Select(m => m.Adresse) //tager kun adresser ellers ville vi også få id med
                .ToListAsync();
            
            List<SelectListItem> listLedigeLejligheder = new List<SelectListItem>();

            foreach (var lejlighed in ledigeLejligheder)
            {
                SelectListItem selListItem = new SelectListItem() { Value = "null", Text = lejlighed.ToString() };
                listLedigeLejligheder.Add(selListItem);
            }




            var model = new CombinedModelForSignUp
            {
                Lejers = new SelectList(listLejers, "Value", "Text", null),
                Lejligheders = new SelectList(listLedigeLejligheder, "Value", "Text", null)
            };



            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SignUp(Models.CombinedModelForSignUp model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model.Lejer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model.Lejer);
        }

        //// GET: Lejers/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var lejer = await _context.Lejer
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (lejer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(lejer);
        //}

        //// GET: Lejers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Lejers/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Fornavn,Efternavn,Adresse,PostNr,Email,TlfNr,By,Alder")] Lejer lejer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(lejer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(lejer);
        //}

        //// GET: Lejers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var lejer = await _context.Lejer.FindAsync(id);
        //    if (lejer == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(lejer);
        //}

        //// POST: Lejers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Fornavn,Efternavn,Adresse,PostNr,Email,TlfNr,By,Alder")] Lejer lejer)
        //{
        //    if (id != lejer.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(lejer);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LejerExists(lejer.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(lejer);
        //}

        // GET: Lejers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lejer = await _context.lejers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lejer == null)
            {
                return NotFound();
            }

            return View(lejer);
        }

        // POST: Lejers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lejer = await _context.lejers.FindAsync(id);
            _context.lejers.Remove(lejer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LejerExists(int id)
        {
            return _context.lejers.Any(e => e.Id == id);
        }
    }
}
