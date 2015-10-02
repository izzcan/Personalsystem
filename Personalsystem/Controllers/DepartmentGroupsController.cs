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
    public class DepartmentGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DepartmentGroups
        public ActionResult Index()
        {
            var departmentGroups = db.DepartmentGroups.Include(d => d.Department);
            return View(departmentGroups.ToList());
        }

        // GET: DepartmentGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentGroup departmentGroup = db.DepartmentGroups.Find(id);
            if (departmentGroup == null)
            {
                return HttpNotFound();
            }
            return View(departmentGroup);
        }

        // GET: DepartmentGroups/Create
        public ActionResult Create(int? id)
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name",id);
            return View();
        }

        // POST: DepartmentGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DepartmentId")] DepartmentGroup departmentGroup)
        {
            if (ModelState.IsValid)
            {
                db.DepartmentGroups.Add(departmentGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", departmentGroup.DepartmentId);
            return View(departmentGroup);
        }

        // GET: Companies/AddEmployee/5
        public ActionResult AddEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentGroup departmentGroup = db.DepartmentGroups.Find(id);
            if (departmentGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(db.Users, "Id", "Email");
            return View(departmentGroup);
        }

        // POST: Companies/AddEmployee/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEmployee(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentGroup departmentGroup = db.DepartmentGroups.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (departmentGroup == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (departmentGroup.Department.Bosses.Contains(currentUser) || departmentGroup.Department.Company.Admins.Contains(currentUser))
                {
                    //Add new user to bosses
                    departmentGroup.Employees.Add(user);
                    db.Entry(departmentGroup).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Info", "Companies", new { id = departmentGroup.Department.CompanyId });
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

        // GET: DepartmentGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentGroup departmentGroup = db.DepartmentGroups.Find(id);
            if (departmentGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", departmentGroup.DepartmentId);
            return View(departmentGroup);
        }

        // POST: DepartmentGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DepartmentId")] DepartmentGroup departmentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departmentGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", departmentGroup.DepartmentId);
            return View(departmentGroup);
        }

        // GET: DepartmentGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentGroup departmentGroup = db.DepartmentGroups.Find(id);
            if (departmentGroup == null)
            {
                return HttpNotFound();
            }
            return View(departmentGroup);
        }

        // POST: DepartmentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DepartmentGroup departmentGroup = db.DepartmentGroups.Find(id);
            db.DepartmentGroups.Remove(departmentGroup);
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
