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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personalsystem.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applications
        public ActionResult Index(int? id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            string userId = User.Identity.GetUserId();
            if (id == null)
            {
                var adminCompanies = currentUser.AdminForCompanies.Select(c => c.Id);
                List<Application> applicationList = new List<Application>();
                if (currentUser.BossForDepartments.Count > 0)
                {
                    ViewBag.isBoss = true;
                    var bossesDepartments = currentUser.BossForDepartments.Select(d => d.Id);                   
                    foreach (int dep in bossesDepartments)
                    {
                        var applicationsDepartments = db.Applications.Include(d => d.Vacancy).Where(d => d.Vacancy.DepartmentId == dep).ToList();
                        applicationList.AddRange(applicationsDepartments);
                    }
                    return View(applicationList);
                }
                else if (currentUser.AdminForCompanies.Count > 0)
                {
                    ViewBag.isBoss = true;
                    foreach (int cmp in adminCompanies)
                    {
                        var allApplications = db.Applications.Include(v => v.Vacancy).Where(v => v.Vacancy.Department.CompanyId == cmp).ToList();
                        applicationList.AddRange(allApplications);
                    }
                    return View(applicationList);
                }
                else
                {
                    ViewBag.isBoss = false;
                    var applications = db.Applications.Include(a => a.Vacancy).Where(a => a.ApplicantId == userId);
                    return View(applications.ToList());
                }
            }
            else
            {
                Department department = db.Departments.Find(id);
                ViewBag.isBoss = department.Bosses.Contains(currentUser);                
                if (department.Bosses.Contains(currentUser))
                {
                    var applications = db.Applications.Include(v => v.Vacancy).Where(d => d.Vacancy.DepartmentId == id).ToList();
                    return View(applications);
                }
                else
                {
                    ViewBag.isBoss = false;
                    var applications = db.Applications.Include(a => a.Vacancy).Where(a => a.ApplicantId == userId);
                    return View(applications.ToList());
                }
            }
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
        [Authorize]
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
            //Generar ett unikt nummer till CV-filen
            Random rand = new Random();
            int randomnumber = rand.Next();
            // Sparar CV-filen till angiven sökväg
            string uploadPath = Server.MapPath("~/CV/");
            file.SaveAs(uploadPath + randomnumber + file.FileName);

            if (ModelState.IsValid)
            {
                ////Sätter in "ApplicantId" objektet "application"
                db.Entry(application).Property("ApplicantId").CurrentValue = User.Identity.GetUserId();
                ////Sätter in "CvPath" objektet "application"
                db.Entry(application).Property("CvPath").CurrentValue = randomnumber + file.FileName;
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email", application.ApplicantId);
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
