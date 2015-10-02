namespace Personalsystem.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Personalsystem.Models;
    using System;
    using System.Collections.Generic;
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
                UserName = "admin@admin.admin",
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
            context.SaveChanges();

            //Add test company and employees
            var userList = new string[]{"magnus@magnusson.se", "peter@petersson.se","olle@ollesson.se", "erik@eriksson.se", "bengt@bengtsson.se", "david@davidsson.se", "adam@adamsson.se", "viktor@viktorsson.se", "ake@akesson.se", "orjan@orjansson.se","jan@jansson.se"};
            foreach(var user in userList )
            {
                AddUser(context, user);
            }

            var company = new Company() { Name = "The Company", Admins = new List<ApplicationUser>(), Departments = new List<Department>() };
            context.Companies.Add(company);
            context.SaveChanges();

            var hr = new Department() { Name = "HR", Bosses = new List<ApplicationUser>(), Groups = new List<DepartmentGroup>() };
            var pr = new Department() { Name = "PR", Bosses = new List<ApplicationUser>(), Groups = new List<DepartmentGroup>() };
            company.Admins.Add(userManager.FindByName("magnus@magnusson.se"));
            company.Departments.Add(hr);
            company.Departments.Add(pr);
            context.SaveChanges();

            var hr1 = new DepartmentGroup() { Name = "HR1", Employees = new List<ApplicationUser>() };
            var hr2 = new DepartmentGroup() { Name = "HR2", Employees = new List<ApplicationUser>() };
            var pr1 = new DepartmentGroup() { Name = "PR1", Employees = new List<ApplicationUser>() };
            var pr2 = new DepartmentGroup() { Name = "PR2", Employees = new List<ApplicationUser>() };
            hr.Bosses.Add(userManager.FindByName("peter@petersson.se"));
            pr.Bosses.Add(userManager.FindByName("olle@ollesson.se"));
            hr.Groups.Add(hr1);
            hr.Groups.Add(hr2);
            pr.Groups.Add(pr1);
            pr.Groups.Add(pr2);
            context.SaveChanges();

            hr1.Employees.Add(userManager.FindByName("erik@eriksson.se"));
            hr1.Employees.Add(userManager.FindByName("bengt@bengtsson.se"));
            hr2.Employees.Add(userManager.FindByName("david@davidsson.se"));
            hr2.Employees.Add(userManager.FindByName("adam@adamsson.se"));
            pr1.Employees.Add(userManager.FindByName("viktor@viktorsson.se"));
            pr1.Employees.Add(userManager.FindByName("ake@akesson.se"));
            pr2.Employees.Add(userManager.FindByName("orjan@orjansson.se"));
            pr2.Employees.Add(userManager.FindByName("jan@jansson.se"));
            context.SaveChanges();
        }

        private void AddUser(Personalsystem.Models.ApplicationDbContext context, string username)
        {
            ApplicationUser adminUser = new ApplicationUser
            {
                UserName = username,
                Email = username
            };

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (userManager.Find(adminUser.UserName, "password") == null)
            {
                userManager.Create(adminUser, "password");
            }
            context.SaveChanges();
        }
    }
}
