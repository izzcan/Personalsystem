using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personalsystem.Models;

namespace Personalsystem.Controllers
{
    public class ScheduleItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScheduleItems
        public ActionResult Index()
        {
            var scheduleItems = db.ScheduleItems.Include(s => s.Schedule);
            return View(scheduleItems.ToList());
        }

        // GET: ScheduleItems/Details/5
        public ActionResult Details(int? id)
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
            return View(scheduleItem);
        }

        // GET: ScheduleItems/Create
        public ActionResult Create()
        {
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id");
            return View();
        }

        // POST: ScheduleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartTime,EndTime,ScheduleId")] ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                db.ScheduleItems.Add(scheduleItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", scheduleItem.ScheduleId);
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
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", scheduleItem.ScheduleId);
            return View(scheduleItem);
        }

        // POST: ScheduleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartTime,EndTime,ScheduleId")] ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheduleItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", scheduleItem.ScheduleId);
            return View(scheduleItem);
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
            return View(scheduleItem);
        }

        // POST: ScheduleItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScheduleItem scheduleItem = db.ScheduleItems.Find(id);
            db.ScheduleItems.Remove(scheduleItem);
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
