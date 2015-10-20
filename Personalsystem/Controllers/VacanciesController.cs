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
    public class VacanciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vacancies
        [Authorize]
        public ActionResult Index(int? id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());
            string userId = User.Identity.GetUserId();
            if (id == null)
            {
                var adminCompanies = currentUser.AdminForCompanies.Select(c => c.Id);
                List<Vacancy> allVacanciesForAllCompanies = new List<Vacancy>();
                if (currentUser.BossForDepartments.Count > 0)
                {
                    ViewBag.isBoss = true;
                    var bossesDepartments = currentUser.BossForDepartments.Select(d => d.Id);
                    List<Vacancy> allVacanciesForDepartments = new List<Vacancy>();
                    foreach (int dep in bossesDepartments){
                        var vacanciesDepartments = db.Vacancies.Include(d => d.Department).Where(d => d.Department.Id == dep).ToList();
                        allVacanciesForDepartments.AddRange(vacanciesDepartments);
                    }
                    return View(allVacanciesForDepartments);
                }
                else if (currentUser.AdminForCompanies.Count > 0)
                {
                    foreach (int cmp in adminCompanies)
                    {
                        var allVacancies = db.Vacancies.Include(v => v.Department).Where(v => v.Department.CompanyId == cmp).ToList();
                        allVacanciesForAllCompanies.AddRange(allVacancies);
                    }
                    return View(allVacanciesForAllCompanies);
                }
                else
                {
                    ViewBag.isBoss = false;
                    var vacancies = db.Vacancies.Include(v => v.Department).Include(v => v.Creator);
                    return View(vacancies.ToList());
                }                
            }
            else
            {
                Department department = db.Departments.Find(id);                
                ViewBag.isBoss = department.Bosses.Contains(currentUser);
                //var adminCompanies = currentUser.AdminForCompanies.Select(c => c.Id);
                //List<Vacancy> allVacanciesForAllCompanies = new List<Vacancy>();
                if (department.Company.Admins.Contains(currentUser))
                {
                    //foreach (int cmp in adminCompanies)
                    //{
                    //    var allVacancies = db.Vacancies.Include(v => v.Department).Where(v => v.Department.CompanyId == cmp).ToList();
                    //    allVacanciesForAllCompanies.AddRange(allVacancies);
                    //}
                    //return View(allVacanciesForAllCompanies);
                } 

                else if (department.Bosses.Contains(currentUser))                    
                {
                    var allVacancies = currentUser.BossForDepartments.SelectMany(v => v.Vacancies);
                    
                }
                ViewBag.DepartmentId = id;
                var vacancies = db.Vacancies.Include(v => v.Department).Include(v => v.Creator).Where(d => d.DepartmentId == id || id == null);
                return View(vacancies.ToList());
            }            
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
        [Authorize]
        public ActionResult Create(int? id)
        {
            Department department = db.Departments.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (department == null || department.Bosses.Contains(currentUser))
                {
                    //Lägger till alla företag i en lista som läggs i Viewbaggen
                    ViewBag.DepartmentId = id;
                    //ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email");
                    return View();
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,Expired,DepartmentId")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                //Sätter dagens datum i kolumnen "Created" i objektet "vacancy"
                db.Entry(vacancy).Property("Created").CurrentValue = DateTime.Now;
                db.Entry(vacancy).Property("CreatorId").CurrentValue = User.Identity.GetUserId();
                //db.Entry(vacancy).Property("CompanyId").CurrentValue = db.Companies.Single(c => c. == User.Identity.GetUserId());
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = vacancy.DepartmentId });
            }

            ViewBag.DepartmentId = new SelectList(db.Companies, "Id", "Name", vacancy.DepartmentId);
            //ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", vacancy.CreatorId);
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        [Authorize]
        public ActionResult Edit(int? id, int? depId)
        {
            Department department = db.Departments.Find(depId);
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (department == null || department.Bosses.Contains(currentUser))
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
                    ViewBag.DepartmentId = vacancy.DepartmentId;
                    ViewBag.DepartmentId = new SelectList(db.Companies, "Id", "Name", vacancy.DepartmentId);
                    ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", vacancy.CreatorId);
                    return View(vacancy);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        // POST: Vacancies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Created,DepartmentId,CreatorId")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacancy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Companies, "Id", "Name", vacancy.DepartmentId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "Email", vacancy.CreatorId);
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, int? depId)
        {
            Department department = db.Departments.Find(depId);
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (department == null || department.Bosses.Contains(currentUser))

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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
