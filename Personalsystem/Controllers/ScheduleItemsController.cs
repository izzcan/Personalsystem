﻿using System;
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
    [Authorize]
    public class ScheduleItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScheduleItems
        public ActionResult Index(int? scheduleId)
        {
            var scheduleItems = db.ScheduleItems.Include(s => s.Schedule).Where(q => scheduleId == null || q.ScheduleId == scheduleId).ToList();
            var model = scheduleItems.Select( q => new ScheduleItemListitemViewmodel(q));

            ViewBag.ScheduleId = scheduleId;
            return View(model.ToList());
        }


        // GET: ScheduleItems/Create
        public ActionResult Create(int? scheduleId)
        {
            var model = new ScheduleItemEditViewmodel();
            model.WeekDays = new List<ScheduleItemWeekday>();
            foreach (var weekDay in db.ScheduleWeekDays.ToList())
            {
                model.WeekDays.Add(new ScheduleItemWeekday() { Id = weekDay.Id, Checked = false, Name = weekDay.Description });
            }

            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id",scheduleId);
            return View(model);
        }

        // POST: ScheduleItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartTime,EndTime,ScheduleId,Description")] ScheduleItem scheduleItem, int[] weekDays)
        {
            if (ModelState.IsValid)
            {
                scheduleItem.WeekDays = db.ScheduleWeekDays.Where(q => weekDays.Contains(q.Id)).ToList();
                db.ScheduleItems.Add(scheduleItem);
                db.SaveChanges();
                return RedirectToAction("Index", new { scheduleItem.ScheduleId });
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
            var model = new ScheduleItemEditViewmodel(scheduleItem);

            foreach (var weekDay in db.ScheduleWeekDays.ToList())
            {
                model.WeekDays.Add(new ScheduleItemWeekday() { Id = weekDay.Id, Checked = scheduleItem.WeekDays != null && scheduleItem.WeekDays.Contains(weekDay), Name = weekDay.Description });
            }

            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", scheduleItem.ScheduleId);
            return View(model);
        }

        // POST: ScheduleItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartTime,EndTime,ScheduleId,Description")] ScheduleItemEditViewmodel model, int[] weekDays)
        {
            if (ModelState.IsValid)
            {
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
            ViewBag.ScheduleId = new SelectList(db.Schedules, "Id", "Id", model.ScheduleId);
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
