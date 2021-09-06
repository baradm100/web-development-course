using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_development_course.Models;

namespace web_development_course.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Address> Address { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<OpeningHour> OpeningHour { get; set; }
    }
}
