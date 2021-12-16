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
    public class PostsController : Controller
    {
        private readonly UserContext _context;
        private static UserManager<IdentityUser> _userManager;
        public PostsController(UserContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        { 
            List<Post> modelList = new List<Post>();
            DateTime curdate = DateTime.Now;
            curdate = curdate.AddDays(-30); 
            var posts =  await _context.posts.Where(d => d.Created > curdate).ToListAsync();

            foreach (var item in posts)
            {
                //Post model = new Post
                //{
                //    Id = item.Id,
                //    AuthorId = item.AuthorId,
                //    AuthorName = item.AuthorName,
                //    Created = item.Created,
                //    PostContent = item.PostContent,
                //    isAdmin = item.isAdmin
                //};
                modelList.Add(item);
            }
            return View(modelList);
        }

        // GET: Posts/Details/5
        

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }
  
        [HttpPost]      
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostContent")] Post post)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
           
            var userId = user?.Id;
            string mail = user?.Email;
            
            var bruger = _context.lejers.Where(a => a.Email == mail).FirstOrDefault();
           
            bool IsAdmin = false;
            if (User.IsInRole("Admin"))
            {
                IsAdmin = true;
            }
            Post model = new Post
            {
                Id = post.Id,
                AuthorId = userId,
                AuthorName = mail,
                Created = DateTime.Now,
                PostContent = post.PostContent,
                isAdmin = IsAdmin
            };

            if (ModelState.IsValid)
            {

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.posts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.posts.FindAsync(id);
            _context.posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.posts.Any(e => e.Id == id);
        }
    }
}
