using FptHrLearningSystem.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FptHrLearningSystem.Controllers
{
    [Authorize(Roles = "Staff")]
    public class TraineeListController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: TraineeList
        public ActionResult Index(int page = 1, int pageSize = 3)
        {

            var trainerIds = ListAllPaging(page, pageSize);


            return View( trainerIds);
        }
        public IEnumerable<ApplicationUser> ListAllPaging(int page, int pageSize)
        {
            return db.Users.Where(u => u.UserType == "Trainee").OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        private bool UserExists(string id)
        {
            return db.Users.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ApplicationUser user)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    if (!UserExists(user.Id))
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }
        public bool DeleteId(string id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            DeleteId(id);
            return RedirectToAction("Index");
        }
       
        public ActionResult Create()
        {
            ViewBag.UserType = new SelectList(db.Roles.Where(x => x.Name.Contains("Trainee")).ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            ViewBag.UserType = new SelectList(db.Roles.Where(x => x.Name.Contains("Trainee")).ToList(), "Name", "Name");
            return View(user);
        }

    }
}