﻿using System;
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
    public class NewsItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NewsItems
        public ActionResult Index()
        {
            var newsItems = db.NewsItems.Include(n => n.Company).Include(n => n.Creator);
            return View(newsItems.ToList());
        }

        // GET: NewsItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            return View(newsItem);
        }
        
        // GET: NewsItems/Create
        public ActionResult Create(int? id)
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name",id);
            return View();
        }

        // POST: NewsItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,CompanyId")] NewsItem newsItem)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                    if (newsItem.CompanyId == 0)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Company company = db.Companies.Find(newsItem.CompanyId);
                    if (company == null)
                    {
                        return HttpNotFound();
                    }


                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    var currentUser = userManager.FindById(User.Identity.GetUserId());
                    if (company.Bosses.Contains(currentUser) ||  company.Admins.Contains(currentUser))
                    {
                        newsItem.Created = DateTime.Now;
                        newsItem.Creator = currentUser;
                        db.NewsItems.Add(newsItem);
                        db.SaveChanges();

                        return RedirectToAction("Details", "Companies", new { id = newsItem.CompanyId });
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    }


            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", newsItem.CompanyId);
            return View(newsItem);
        }

        // GET: NewsItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (newsItem.Company.Admins.Contains(currentUser) || newsItem.Company.Bosses.Contains(currentUser))
                {
                    return View(newsItem);
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

        // POST: NewsItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content")] NewsItem newsItem)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                if (newsItem.Id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NewsItem dbNewsItem = db.NewsItems.Find(newsItem.Id);
                if (dbNewsItem == null)
                {
                    return HttpNotFound();
                }
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (dbNewsItem.Company.Bosses.Contains(currentUser) || dbNewsItem.Company.Admins.Contains(currentUser))
                {
                    dbNewsItem.Title = newsItem.Title;
                    dbNewsItem.Content = newsItem.Content;

                    db.Entry(dbNewsItem).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Companies", new { id = dbNewsItem.Company.Id });
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }
            return View(newsItem);
        }

        // GET: NewsItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (newsItem.Company.Bosses.Contains(currentUser) || newsItem.Company.Admins.Contains(currentUser))
                {
                    return View(newsItem);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }

            }
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        // POST: NewsItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsItem newsItem = db.NewsItems.Find(id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                if (newsItem.Company.Bosses.Contains(currentUser) || newsItem.Company.Admins.Contains(currentUser))
                {
                    var companyId = newsItem.Company.Id;
                    db.NewsItems.Remove(newsItem);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Companies", new { id = companyId });
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
