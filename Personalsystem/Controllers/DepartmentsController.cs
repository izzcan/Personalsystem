using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personalsystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personalsystem.Controllers
{
    public class DepartmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Departments
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.Company);
            return View(departments.ToList());
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            Company company = db.Companies.Find(id);

            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (company == null || company.Admins.Contains(currentUser))
                {
                    ViewBag.CompanyId = new SelectList(db.Companies.ToList().Where(q => q.Admins.Contains(currentUser)).ToList(), "Id", "Name", id);
                    return View();

                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CompanyId")] Department department)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (department.CompanyId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Company company = db.Companies.Find(department.CompanyId);
                if (company == null)
                {
                    return HttpNotFound();
                }
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (company.Admins.Contains(currentUser)) //Current logged in user must be admin for the company that the department is being added to
                {
                    db.Departments.Add(department);
                    db.SaveChanges();

                    return RedirectToAction("Employees", "Companies", new { id = department.CompanyId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", department.CompanyId);
            return View(department);
        }

        // GET: Companies/AddBoss/5
        public ActionResult AddBoss(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "Email");
            return View(department);
        }

        // POST: Companies/AddBoss/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBoss(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (department == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (department.Company.Admins.Contains(currentUser))
                {
                    //Add new user to bosses
                    department.Bosses.Add(user);
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Employees", "Companies", new { id = department.CompanyId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }

        // GET: Companies/RemoveBoss/5
        public ActionResult RemoveBoss(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (department == null || user == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(department.Bosses, "Id", "Email", userId);
            return View(department);
        }

        // POST: Companies/RemoveBoss/5
        [HttpPost, ActionName("RemoveBoss")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBoss(int id, string userId)
        {

            if (id == 0 || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (department == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (department.Company.Admins.Contains(currentUser))
                {
                    //Remove user from bosses
                    department.Bosses.Remove(user);
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Employees", "Companies", new { id = department.CompanyId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
        }


        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", department.CompanyId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CompanyId")] Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", department.CompanyId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
