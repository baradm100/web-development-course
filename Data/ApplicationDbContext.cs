using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_development_course.Models;
using web_development_course.Models.OrderModels;

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
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<User>().HasData(new User { Id = 1, FirstName = "Admin", LastName = "Admin", Email = "admin@admin.com", Password = "1234", UserType = UserLevel.Admin });

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Men" },
                new Category { Id = 2, Name = "Women" },
                new Category { Id = 3, Name = "Men Shirts", ParentCategoryId = 1 },
                new Category { Id = 4, Name = "Women Shirts", ParentCategoryId = 2 },
                new Category { Id = 5, Name = "Men Pants", ParentCategoryId = 1 },
                new Category { Id = 6, Name = "Women Pants", ParentCategoryId = 2 },
                new Category { Id = 7, Name = "Men Hats", ParentCategoryId = 1 },
                new Category { Id = 8, Name = "Women Hats", ParentCategoryId = 2 }
            );
        }
    }

}
