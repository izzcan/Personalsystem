using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personalsystem.Models;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Identity;

namespace Personalsystem.Controllers
{
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applications
        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.Applicant).Include(a => a.Vacancy);
            return View(applications.ToList());
        }

        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create(int? id)
        {
            ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email");
            //ViewBag.VacancyId = new SelectList(db.Vacancies, "Id", "Title", id);
            ViewBag.VacancyId = id;
            ViewBag.VacancyTitle = db.Vacancies.Find(id).Title;
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,VacancyId,fileuploadPdf")] Application application)
        {
            HttpPostedFileBase file = Request.Files["fileuploadPdf"];
            Random rand = new Random();
            int randomnumber = rand.Next();
            // Sparar filen till angiven sökväg
            string uploadPath = Server.MapPath("~/CV/");
            file.SaveAs(uploadPath + randomnumber + file.FileName);

            if (ModelState.IsValid)
            {
                db.Entry(application).Property("ApplicantId").CurrentValue = User.Identity.GetUserId();
                db.Entry(application).Property("CvPath").CurrentValue = randomnumber + file.FileName;
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email", application.ApplicantId);
            ViewBag.VacancyId = new SelectList(db.Vacancies, "Id", "Title", application.VacancyId);
            return View(application);
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email", application.ApplicantId);
            ViewBag.VacancyId = new SelectList(db.Vacancies, "Id", "Title", application.VacancyId);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,ApplicantId,VacancyId")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email", application.ApplicantId);
            ViewBag.VacancyId = new SelectList(db.Vacancies, "Id", "Title", application.VacancyId);
            return View(application);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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
