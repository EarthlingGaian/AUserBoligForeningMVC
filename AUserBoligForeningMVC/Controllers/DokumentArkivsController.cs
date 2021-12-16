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
           
            return View();
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


    }
}
