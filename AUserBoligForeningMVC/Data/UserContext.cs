using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Data
{
   
        public class UserContext : DbContext
    {
            public UserContext(DbContextOptions<UserContext> options)
                : base(options)
            {
            }

        public DbSet<Models.Lejer> lejers { get; set; }
        public DbSet<Models.Post> posts { get; set; }
        public DbSet<Models.Booking> bookings { get; set; }
        public DbSet<Models.DokumentArkiv> dokumentArkivs { get; set; }
        public DbSet<Models.Regninger> regningers { get; set; }
    }
    
}
