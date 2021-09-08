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
    public class TrainerListController : Controller
    {
       
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: TrainerList
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            var trainerIds = ListAllPaging(page, pageSize);
            return View(trainerIds);
        }
        public IEnumerable<ApplicationUser> ListAllPaging(int page, int pageSize)
        {
            return db.Users.Where(u => u.UserType == "Trainer").OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.trainers.Include(x=>x.UserCourses).FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewData["Courses"] = db.UserCourses.Include(t => t.Course).Where(t => t.UserId == id).ToList();
            return View(user);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.trainers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        private bool UserExists(string id)
        {
            return db.trainers.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Trainer user)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;
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
            ViewBag.UserType = new SelectList(db.Roles.Where(x => x.Name.Contains("Trainer")).ToList(), "Name", "Name");
            
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
            ViewBag.UserType = new SelectList(db.Roles.Where(x => x.Name.Contains("Trainer")).ToList(), "Name", "Name");
          
            return View(user);
        }
        [HttpGet]
        public ActionResult AssignToCourse(string id)
        {
            ApplicationUser trainer = db.Users.FirstOrDefault(u => u.Id == id);
            var courses = db.courses.ToList();
            ViewBag.CourseId = new SelectList(courses, "Id", "Code");
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignToCourse([Bind(Include = "UserId,CourseId")] UserCourse userCourse)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.UserCourses.Add(userCourse);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            ApplicationUser trainer = userCourse.User;
            var courses = db.courses.ToList();
            ViewBag.CourseId = new SelectList(courses, "Id", "Code");
            return View(userCourse);
        }
      
    }
}