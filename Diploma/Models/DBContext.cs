using Diploma.Models.DataBase;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Diploma.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DefaultConnection")
        {

        }

        public DbSet<Admit> Admits { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Rooms> Rooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class Initializer : DropCreateDatabaseIfModelChanges<DBContext>
    {
        protected void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };

            roleManager.Create(role1);
            roleManager.Create(role2);

            var admin = new ApplicationUser { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
            string password = "Danycool12!";
            var result = userManager.Create(admin, password);   

            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }
        }
    }
}