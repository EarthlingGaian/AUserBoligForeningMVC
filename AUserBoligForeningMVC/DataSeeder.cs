
using AUserBoligForeningMVC.Data;
using AUserBoligForeningMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC
{
    public class DataSeeder
    {
        private ApplicationDbContext _context;
        public DataSeeder(ApplicationDbContext contezt)
        {
            _context = contezt;
        }


        public async Task SeedSuperUser()
        {

            var RolseStore = new RoleStore<IdentityRole>(_context);//using this to create new identity role
            var UserStore = new UserStore<IdentityUser>(_context);


            var user = new IdentityUser
            {
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var hashed = new PasswordHasher<IdentityUser>();
            var hashedPassword = hashed.HashPassword(user, "admin");

            user.PasswordHash = hashedPassword;

            var HasAdminRole = _context.Roles.Any(roles => roles.Name == "Admin");


            if(!HasAdminRole) //create admin role if it doesnt exsist
            {
                 await RolseStore.CreateAsync(new IdentityRole 
                { 
                    Name = "Admin", 
                    NormalizedName ="admin"
                });
            }

            var HasSuperUser = _context.Users.Any(users => users.NormalizedUserName == user.UserName); //Note user is from the creation of the superuser above and we ask here if it is created
           //user = _context.Users.FirstOrDefault(users => users.NormalizedUserName == user.UserName);
            var roles = _context.Roles.FirstOrDefault(roles => roles.Name == "Admin");

            if (!HasSuperUser)
            {
                UserStore.CreateAsync(user).Wait();
                UserStore.AddToRoleAsync(user, "Admin").Wait(); //asign admin role to superuser
            }

           

            _context.SaveChangesAsync().Wait();

           

        }
    }
}
