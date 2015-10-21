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
    [Authorize]
    public class ScheduleItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScheduleItems
        public ActionResult Index(int? scheduleId)
        {
            Schedule schedule = db.Schedules.Find(scheduleId);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            //If user is not boss for the department and is not an employee in that department: Unauthorized
            if (!((schedule.Department == null && schedule.Group.Department.Bosses.Contains(currentUser)) || schedule.Department.Bosses.Contains(currentUser)) &&
                !((schedule.Department == null && schedule.Group.Department.Employees.Contains(currentUser)) || schedule.Department.Employees.Contains(currentUser)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var scheduleItems = db.ScheduleItems.Include(s => s.Schedule).Where(q => scheduleId == null || q.ScheduleId == scheduleId).ToList();
            var model = scheduleItems.Select(q => new ScheduleItemListitemViewmodel(q));

            ViewBag.ScheduleId = scheduleId;
            return View(model.ToList());
        }


        // GET: ScheduleItems/Create
        public ActionResult Create(int? scheduleId)
        {
            Schedule schedule = db.Schedules.Find(scheduleId);

            if (schedule == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (schedule.Department.Bosses.Contains(currentUser))
                {
                    var model = new ScheduleItemEditViewmodel();
                    model.WeekDays = new List<ScheduleItemWeekday>();
                    foreach (var weekDay in db.ScheduleWeekDays.ToList())
                    {
                        model.WeekDays.Add(new ScheduleItemWeekday() { Id = weekDay.Id, Checked = false, Name = weekDay.Description });
                    }

                    //ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id",scheduleId);
                    ViewBag.ScheduleId = db.Schedules.ToList().Select(q => new SelectListItem()
                    {
                        Value = q.Id.ToString(),
                        Text = q.StartTime.ToShortDateString() + " : " + (q.EndTime == null ? "Ongoing" : q.EndTime.GetValueOrDefault().ToShortDateString()),
                        Selected = (q.Id == scheduleId)
                    });
                    return View(model);
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

        // POST: ScheduleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartTime,EndTime,ScheduleId,Description")] ScheduleItem scheduleItem, int[] weekDays)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (scheduleItem.ScheduleId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Schedule schedule = db.Schedules.Find(scheduleItem.ScheduleId);
                if (schedule == null)
                {
                    return HttpNotFound();
                }
                if (schedule.Department.Bosses.Contains(currentUser)) //Current logged in user must be boss for the department
                {
                    scheduleItem.WeekDays = db.ScheduleWeekDays.Where(q => weekDays.Contains(q.Id)).ToList();
                    db.ScheduleItems.Add(scheduleItem);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { scheduleItem.ScheduleId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }

            //ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", scheduleItem.ScheduleId);
            ViewBag.ScheduleId = db.Schedules.ToList().Select(q => new SelectListItem()
            {
                Value = q.Id.ToString(),
                Text = q.StartTime.ToShortDateString() + " : " + (q.EndTime == null ? "Ongoing" : q.EndTime.GetValueOrDefault().ToShortDateString()),
                Selected = (q.Id == scheduleItem.ScheduleId)
            });
            return View(scheduleItem);
        }

        // GET: ScheduleItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleItem scheduleItem = db.ScheduleItems.Find(id);
            if (scheduleItem == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (scheduleItem.Schedule.Department.Bosses.Contains(currentUser))
                {
                    var model = new ScheduleItemEditViewmodel(scheduleItem);

                    foreach (var weekDay in db.ScheduleWeekDays.ToList())
                    {
                        model.WeekDays.Add(new ScheduleItemWeekday() { Id = weekDay.Id, Checked = scheduleItem.WeekDays != null && scheduleItem.WeekDays.Contains(weekDay), Name = weekDay.Description });
                    }

                    //ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", scheduleItem.ScheduleId);
                    ViewBag.ScheduleId = db.Schedules.ToList().Select(q => new SelectListItem()
                    {
                        Value = q.Id.ToString(),
                        Text = q.StartTime.ToShortDateString() + " : " + (q.EndTime == null ? "Ongoing" : q.EndTime.GetValueOrDefault().ToShortDateString()),
                        Selected = (q.Id == scheduleItem.ScheduleId)
                    });
                    return View(model);
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

        // POST: ScheduleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartTime,EndTime,ScheduleId,Description")] ScheduleItemEditViewmodel model, int[] weekDays)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (model.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ScheduleItem dbScheduleItem = db.ScheduleItems.Find(model.Id);
                if (dbScheduleItem == null)
                {
                    return HttpNotFound();
                }
                if (dbScheduleItem.Schedule.Department.Bosses.Contains(currentUser))
                {
                    db.Entry(dbScheduleItem).State = EntityState.Detached;

                    ScheduleItem scheduleItem = new ScheduleItem(model);
                    db.Entry(scheduleItem).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Entry(scheduleItem).State = EntityState.Detached;
                    scheduleItem = db.ScheduleItems.Find(scheduleItem.Id);
                    scheduleItem.WeekDays.Clear();

                    scheduleItem.WeekDays = db.ScheduleWeekDays.Where(q => weekDays.Contains(q.Id)).ToList();
                    db.Entry(scheduleItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { scheduleItem.ScheduleId });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }

            //ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", model.ScheduleId);
            ViewBag.ScheduleId = db.Schedules.ToList().Select(q => new SelectListItem()
            {
                Value = q.Id.ToString(),
                Text = q.StartTime.ToShortDateString() + " : " + (q.EndTime == null ? "Ongoing" : q.EndTime.GetValueOrDefault().ToShortDateString()),
                Selected = (q.Id == model.ScheduleId)
            });
            model.WeekDays = new List<ScheduleItemWeekday>();
            foreach (var weekDay in db.ScheduleWeekDays)
            {
                model.WeekDays.Add(new ScheduleItemWeekday() { Id = weekDay.Id, Checked = weekDays.Contains(weekDay.Id), Name = weekDay.Description });
            }
            return View(model);
        }

        // GET: ScheduleItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleItem scheduleItem = db.ScheduleItems.Find(id);
            if (scheduleItem == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (scheduleItem.Schedule.Department.Bosses.Contains(currentUser))
                {
                    return View(scheduleItem);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        // POST: ScheduleItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduleItem scheduleItem = db.ScheduleItems.Find(id);
            if (scheduleItem == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (scheduleItem.Schedule.Department.Bosses.Contains(currentUser))
                {
                    var scheduleId = scheduleItem.ScheduleId;
                    db.ScheduleItems.Remove(scheduleItem);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { scheduleId  });
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
