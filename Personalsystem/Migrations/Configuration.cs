namespace Personalsystem.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Personalsystem.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Personalsystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Personalsystem.Models.ApplicationDbContext context)
        {

            //Add roles
            //context.Roles.AddOrUpdate(
            //    q => q.Name,
            //    new IdentityRole() { Name = "SuperAdmin" },
            //    new IdentityRole() { Name = "Admin" },
            //    new IdentityRole() { Name = "Boss" },
            //    new IdentityRole() { Name = "Employee" },
            //    new IdentityRole() { Name = "Unemployed" }
            //    );

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                roleManager.Create(new IdentityRole() { Name = "SuperAdmin" });
            }
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole() { Name = "Admin" });
            }
            if (!roleManager.RoleExists("Boss"))
            {
                roleManager.Create(new IdentityRole() { Name = "Boss" });
            }
            if (!roleManager.RoleExists("Employee"))
            {
                roleManager.Create(new IdentityRole() { Name = "Employee" });
            }
            if (!roleManager.RoleExists("Unemployed"))
            {
                roleManager.Create(new IdentityRole() { Name = "Unemployed" });
            }
            context.SaveChanges();

            //Add users
            //var hasher = new PasswordHasher();
            //string password = hasher.HashPassword("password");

            //var adminUser = new ApplicationUser
            //{
            //    UserName = "Admin",
            //    Email = "admin@admin.admin",
            //    PasswordHash = password,
            //    SecurityStamp = "UnchangedPassword"
            //};
            ApplicationUser adminUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@admin.admin"
            };

            //context.Users.AddOrUpdate(
            //    u => u.UserName,
            //        adminUser
            //    );

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (userManager.Find(adminUser.UserName, "password") == null)
            {
                userManager.Create(adminUser, "password");
            }
            
            context.SaveChanges();

            //Add roles to users
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            

            adminUser = context.Users.Single(q => q.UserName == adminUser.UserName);

            if (!userManager.IsInRole(adminUser.Id, "SuperAdmin"))
            {
                userManager.AddToRole(adminUser.Id, "SuperAdmin");
            }

        }
    }
}
