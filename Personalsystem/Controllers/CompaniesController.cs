using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Personalsystem.Models;
using Personalsystem.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personalsystem.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

       
        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            var model = new CompanyDetailsViewmodel(company);
            //Disable unauthorized buttons
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            ViewBag.IsAdmin = company.Admins.Contains(currentUser);
            ViewBag.IsBoss = company.Bosses.Contains(currentUser);
            

            var isBossFor = new Dictionary<int, bool>();
            foreach (var department in company.Departments)
            {
                isBossFor[department.Id] =department.Bosses.Contains(currentUser);
            }
            ViewBag.IsBossFor = isBossFor;

            var isCreatorFor = new Dictionary<int, bool>();
            foreach (var newsItem in company.NewsItems)
            {
                isCreatorFor[newsItem.Id] = newsItem.CreatorId == currentUser.Id;
            }
            ViewBag.IsCreatorFor = isCreatorFor;

            return View(model);
        }

        // GET: Companies/AddAdmins/5
        public ActionResult AddAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            //var model = db.Users.Select(q => new CompanyUserRoleListitemViewmodel() { Id = q.Id, Name = q.UserName, HasRole = company.Admins.Contains(q) }).ToList();
            ViewBag.userId = new SelectList(db.Users, "Id", "Email");
            return View(company);
        }

        // POST: Companies/AddAdmins/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (company == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (company.Admins.Contains(currentUser))
                {
                    //Add new user to admin
                    company.Admins.Add(user);
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Companies", new { id = id });
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

        // GET: Companies/RemoveAdmin/5
        public ActionResult RemoveAdmin(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (company == null || user == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(company.Admins, "Id", "Email",userId);
            return View(company);
        }

        // POST: Companies/RemoveAdmin/5
        [HttpPost, ActionName("RemoveAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAdmin(int id, string userId)
        {

            if (id == 0 || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (company == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (company.Admins.Contains(currentUser))
                {
                    //Remove user from admin
                    company.Admins.Remove(user);
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Companies", new { id = id });
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


        // GET: Companies/AddLeader/5
        public ActionResult AddLeader(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            //var model = db.Users.Select(q => new CompanyUserRoleListitemViewmodel() { Id = q.Id, Name = q.UserName, HasRole = company.Admins.Contains(q) }).ToList();
            ViewBag.userId = new SelectList(db.Users, "Id", "Email");
            return View(company);
        }

        // POST: Companies/AddLeader/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLeader(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (company == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (company.Admins.Contains(currentUser))
                {
                    //Add user to leadership
                    company.Leadership.Add(user);
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Companies", new { id = id });
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

        // GET: Companies/RemoveLeader/5
        public ActionResult RemoveLeader(int? id, string userId)
        {
            if (id == null || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (company == null || user == null)
            {
                return HttpNotFound();
            }
            ViewBag.userId = new SelectList(company.Leadership, "Id", "Email", userId);
            return View(company);
        }

        // POST: Companies/RemoveLeader/5
        [HttpPost, ActionName("RemoveLeader")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveLeader(int id, string userId)
        {

            if (id == 0 || userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            ApplicationUser user = db.Users.Find(userId);
            if (company == null || user == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (company.Admins.Contains(currentUser))
                {
                    //Remove user from leadership
                    company.Leadership.Remove(user);
                    db.Entry(company).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Companies", new { id = id });
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



        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                //Add new user to admin
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                company.Admins = new List<ApplicationUser>();
                company.Admins.Add(currentUser);

                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
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
