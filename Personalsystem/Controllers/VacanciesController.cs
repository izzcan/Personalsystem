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

namespace Personalsystem.Controllers
{
    public class VacanciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vacancies
        public ActionResult Index()
        {
            var vacancies = db.Vacancies.Include(v => v.Company).Include(v => v.Creator);
            return View(vacancies.ToList());
        }

        // GET: Vacancies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = db.Vacancies.Find(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            return View(vacancy);
        }

        // GET: Vacancies/Create
        public ActionResult Create()
        {
            //Lägger till alla företag i en lista som läggs i Viewbaggen
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            //ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,Expired,CompanyId")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                //Sätter dagens datum i kolumnen "Created" i objektet "vacancy"
                db.Entry(vacancy).Property("Created").CurrentValue = DateTime.Now;
                db.Entry(vacancy).Property("CreatorId").CurrentValue = User.Identity.GetUserId();
                //db.Entry(vacancy).Property("CompanyId").CurrentValue = db.Companies.Single(c => c. == User.Identity.GetUserId());
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", vacancy.CompanyId);
            //ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", vacancy.CreatorId);
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = db.Vacancies.Find(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", vacancy.CompanyId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", vacancy.CreatorId);
            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Created,CompanyId,CreatorId")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacancy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", vacancy.CompanyId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", vacancy.CreatorId);
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacancy vacancy = db.Vacancies.Find(id);
            if (vacancy == null)
            {
                return HttpNotFound();
            }
            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vacancy vacancy = db.Vacancies.Find(id);
            db.Vacancies.Remove(vacancy);
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
