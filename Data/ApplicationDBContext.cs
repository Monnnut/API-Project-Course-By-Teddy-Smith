using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        //allow us to pass to DbContextoptions to dbcontext
        {

        }
        //accessing table via DbSet<T>
        public DbSet<Stock> Stocks { get; set; } //manipuate
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        //OnmodelCreating(Model...) responsible for setting up how database will look 
        //including what role exist
        //base.OnmodelCreating(builder) ensure any existing configuration from the base class
        //are applied before adding customer configs
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId })); //Primary Key

            //This code defines a relationship between two 
            //entities in Entity Framework: Portfolio and AppUser. 
            //Specifically, it sets up a one-to-many relationship, 
            //where one AppUser can have many Portfolios, 
            //and each Portfolio belongs to a single AppUser. 
            builder.Entity<Portfolio>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
            .HasOne(u => u.Stock)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.StockId);



            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName = "ADMIN"
                },
                     new IdentityRole
                {
                    Name="User",
                    NormalizedName = "USER"
                }
            };
            //This line uses the HasData method to seed the roles 
            //into the database. It tells Entity Framework to add 
            //the roles (Admin and User) into the IdentityRole 
            //table if they don't already exist when the database 
            //is created or migrated.
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

// class to search for individual table
//The DbContext class provides functionality for querying and saving data to a database.
// //The constructor accepts an object of type DbContextOptions which contains configuration settings for the database (such as the connection string, database provider, etc.).
// The base(dbContextOptions) call passes these options to the base DbContext class, allowing it to configure how it connects to the databas