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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //Add roles
            context.Roles.AddOrUpdate(
                q => q.Name,
                new IdentityRole() { Name = "SuperAdmin" },
                new IdentityRole() { Name = "Admin" },
                new IdentityRole() { Name = "Boss" },
                new IdentityRole() { Name = "Employee" },
                new IdentityRole() { Name = "Unemployed" }
                );


            //Add users
            var hasher = new PasswordHasher();
            string password = hasher.HashPassword("password");

            var adminUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@admin.admin",
                PasswordHash = password,
                SecurityStamp = "UnchangedPassword"
            };

            context.Users.AddOrUpdate(
                u => u.UserName,
                    adminUser
                );
            context.SaveChanges();

            //Add roles to users
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!userManager.IsInRole(adminUser.Id, "SuperAdmin"))
            {
                userManager.AddToRole(adminUser.Id, "SuperAdmin");
            }
        }
    }
}
