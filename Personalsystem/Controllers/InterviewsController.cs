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
using System.Net.Mail;

namespace Personalsystem.Controllers
{
    public class InterviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       

        // GET: Interviews
        public ActionResult Index()
        {
            var interviews = db.Interviews.Include(i => i.Applications).Include(i => i.Interviewer);
            return View(interviews.ToList());
        }

       public ActionResult GetVacancies()
        {
            var vacancies = db.Vacancies
                .Select(v => new { v.Id, v.Title })
                .OrderBy(v => v.Title);
            return Json(vacancies, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetApplicants(int vacId)
       {
           var applicants = db.Applications
               .Where(a => a.VacancyId == vacId)
               .Select(a => new { a.Id, a.Applicant.Email })
               .OrderBy(a => a.Email);
           return Json(applicants, JsonRequestBehavior.AllowGet);
       }

        // GET: Interviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // GET: Interviews/Create
        public ActionResult Create(int? id)
        {

            ViewBag.Application_Id = id;
            var vacancyTitle = db.Applications.Where(a => a.Id == id).Single().Vacancy.Title;
            ViewBag.vacancyTitle = vacancyTitle;
            var applicantEmail = db.Applications.Where(a => a.Id == id).Single().Applicant.Email;
            ViewBag.applicantEmail = applicantEmail;
            return View();            
        }

        // POST: Interviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Application_Id,InterviewDate")] Interview interview, string applicantEmail)
        {
            if (ModelState.IsValid)
            {
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress("testkurs@gavle.brynassupport.se");
                msg.To.Add(applicantEmail);
                msg.Subject = "Intervju";
                msg.Body = "Hej. Du är kallad på intervju den "+interview.InterviewDate;
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = true;
                client.Host = "smtp.gavle.brynassupport.se";
                client.Port = 587;
                client.EnableSsl = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("testkurs@gavle.brynassupport.se", "Abc12345");
                client.Timeout = 20000;               
                client.Send(msg);                  
                db.Entry(interview).Property("InterviewerId").CurrentValue = User.Identity.GetUserId();
                db.Interviews.Add(interview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                       
            return View(interview);
        }

        // GET: Interviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
           ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email", interview.Applications.ApplicantId);
            ViewBag.InterviewerId = new SelectList(db.Users, "Id", "Email", interview.InterviewerId);
            
            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,ApplicantId,InterviewerId")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantId = new SelectList(db.Users, "Id", "Email", interview.Applications.ApplicantId);
            ViewBag.InterviewDate = new SelectList(db.Users, "Id", "Email", interview.InterviewerId);
            return View(interview);
        }

        // GET: Interviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interview interview = db.Interviews.Find(id);
            db.Interviews.Remove(interview);
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
