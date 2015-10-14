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
    public class SchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schedules
        public ActionResult Index(int? departmentId, int? groupId)
        {
            var schedules = db.Schedules.Include(s => s.Department).Include(s => s.Group)
                .Where(q => departmentId == null || q.DepartmentId == departmentId)
                .Where(q => groupId == null || q.GroupId == groupId);

            ViewBag.HrefValues = new { departmentId, groupId };

            return View(schedules.ToList());
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            var model = new ScheduleDetailsViewmodel(schedule);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Schedules/Create
        public ActionResult Create(int? departmentId, int? groupId)
        {
            Department department = db.Departments.Find(departmentId);
            DepartmentGroup group = db.DepartmentGroups.Find(groupId);

            if(department == null && group == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (department.Bosses.Contains(currentUser) || (group != null && group.Department.Bosses.Contains(currentUser)))
                {
                    //ViewBag.DepartmentId = new SelectList(db.Departments.ToList().Where(q=>q.Bosses.Contains(currentUser)).ToList(), "Id","Name",departmentId);
                    //ViewBag.GroupId = new SelectList(db.DepartmentGroups.ToList().Where(q=>q.Department.Bosses.Contains(currentUser)).ToList(), "Id","Name",groupId);
                    ViewBag.DepartmentId = new SelectList(db.Departments.ToList().Where(q => q.Bosses.Contains(currentUser)).ToList(), "Id", "Name", departmentId);
                    var groups = new SelectList(db.DepartmentGroups.ToList().Where(q => q.Department.Bosses.Contains(currentUser)).ToList(), "Id", "Name", groupId).ToList();
                    groups.Insert(0,new SelectListItem() { Value = "", Text = "All" });
                    ViewBag.GroupId = groups;
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

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartTime,EndTime,DepartmentId,GroupId")] Schedule schedule)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (schedule.DepartmentId == 0 && schedule.GroupId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Department department = db.Departments.Find(schedule.DepartmentId) ?? db.DepartmentGroups.Find(schedule.GroupId).Department;
                if (department == null)
                {
                    return HttpNotFound();
                }
                if (department.Bosses.Contains(currentUser)) //Current logged in user must be boss for the department
                {
                    db.Schedules.Add(schedule);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Schedules", new { id = department.Id });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }
            ViewBag.DepartmentId = new SelectList(db.Departments.ToList().Where(q => q.Bosses.Contains(currentUser)).ToList(), "Id", "Name", schedule.DepartmentId);
            var groups = new SelectList(db.DepartmentGroups.ToList().Where(q => q.Department.Bosses.Contains(currentUser)).ToList(), "Id", "Name", schedule.GroupId).ToList();
            groups.Insert(0,new SelectListItem(){ Value = "", Text = "All"});
            ViewBag.GroupId = groups;

            return View(schedule);  
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (schedule.Department.Bosses.Contains(currentUser) || (schedule.Group != null && schedule.Group.Department.Bosses.Contains(currentUser)))
                {
                    ViewBag.DepartmentId = new SelectList(db.Departments.ToList().Where(q => q.Bosses.Contains(currentUser)).ToList(), "Id", "Name", schedule.DepartmentId);
                    var groups = new SelectList(db.DepartmentGroups.ToList().Where(q => q.Department.Bosses.Contains(currentUser)).ToList(), "Id", "Name", schedule.GroupId).ToList();
                    groups.Insert(0, new SelectListItem() { Value = "", Text = "All" });
                    ViewBag.GroupId = groups;

                    return View(schedule);
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

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartTime,EndTime,DepartmentId,GroupId")] Schedule schedule)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (schedule.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Schedule dbSchedule = db.Schedules.Find(schedule.Id);
                if (dbSchedule == null)
                {
                    return HttpNotFound();
                }
                if (dbSchedule.Department.Bosses.Contains(currentUser) || (dbSchedule.Group != null && dbSchedule.Group.Department.Bosses.Contains(currentUser)))
                {
                    db.Entry(dbSchedule).State = EntityState.Detached;

                    db.Entry(schedule).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Schedules", new { id = schedule.DepartmentId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }

            ViewBag.DepartmentId = new SelectList(db.Departments.ToList().Where(q => q.Bosses.Contains(currentUser)).ToList(), "Id", "Name", schedule.DepartmentId);
            var groups = new SelectList(db.DepartmentGroups.ToList().Where(q => q.Department.Bosses.Contains(currentUser)).ToList(), "Id", "Name", schedule.GroupId).ToList();
            groups.Insert(0, new SelectListItem() { Value = "", Text = "All" });
            ViewBag.GroupId = groups;

            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (schedule.Department.Bosses.Contains(currentUser) || (schedule.Group != null && schedule.Group.Department.Bosses.Contains(currentUser)))
                {
                    return View(schedule);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (schedule.Department.Bosses.Contains(currentUser) || (schedule.Group != null && schedule.Group.Department.Bosses.Contains(currentUser)))
                {
                    var departmentId = schedule.DepartmentId;
                    db.Schedules.Remove(schedule);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Schedules", new { id = departmentId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
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
