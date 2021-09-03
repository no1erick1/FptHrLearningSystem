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
    
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Courses
        [Authorize(Roles = "Staff,Trainee,Trainer")]
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            var applicationDbContext = ListAllPaging(page, pageSize);
            return View(applicationDbContext);
        }
        public IEnumerable<Course> ListAllPaging(int page, int pageSize)
        {
            return db.courses.Include(c => c.Category).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.courses.Include(x=>x.Category).FirstOrDefault(m => m.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.categories, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Id,Code,Description,Name_EN,Name_VI,Credit,Hour,CategoryId")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.courses.Add(course);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            ViewData["CategoryId"] = new SelectList(db.categories, "Id", "Name", course.CategoryId);
            return View(course);
        }
        [Authorize(Roles = "Staff")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.categories, "Id", "Name", course.CategoryId);
            return View(course);
        }
        [Authorize(Roles = "Staff")]
        private bool CourseExists(int id)
        {
            return db.courses.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include ="Id,Code,Description,Name_EN,Name_VI,Credit,Hour,CategoryId")] Course course)
        {
            if (id != course.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["CategoryId"] = new SelectList(db.categories, "Id", "Name", course.CategoryId);
            return View(course);
        }
        public bool DeleteId(int id)
        {
            try
            {
                var course = db.courses.Find(id);
                db.courses.Remove(course);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
      
        [Authorize(Roles = "Staff")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            DeleteId(id);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Staff")]
        public ActionResult AddToCourse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var trainerIds = db.Users.Where(u => u.UserType == "Trainer").Select(u => u.Id).ToList();
            var trainers = db.Users.Where(u => trainerIds.Contains(u.Id)).ToList();

            var currentTrainerIds = db.TrainerCourses.Where(t => t.CourseId == id).Select(t => t.TrainerId).ToList();
            var currentTrainers = trainers.Where(t => currentTrainerIds.Contains(t.Id)).ToList();
            var remainingTrainers = trainers.Where(t => !currentTrainerIds.Contains(t.Id)).ToList();

            ViewData["RemainingTrainers"] = remainingTrainers;
            ViewData["CurrentTrainers"] = currentTrainers;

            return View(course);
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTrainer(int? courseId, string trainerId)
        {
            if (courseId != null && trainerId != null)
            {
                var trainerCourse = new TrainerCourse()
                {
                    CourseId = (int)courseId,
                    TrainerId = trainerId
                };

                db.TrainerCourses.Add(trainerCourse);
                db.SaveChanges();
            }

            return RedirectToAction("AddToCourse", new { id = courseId });
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveTrainer(int? courseId, string trainerId)
        {
            var trainerCourse = db.TrainerCourses.FirstOrDefault(t => t.CourseId == courseId && t.TrainerId == trainerId);

            if (trainerCourse != null)
            {
                db.TrainerCourses.Remove(trainerCourse);
                db.SaveChanges();
            }

            return RedirectToAction("AddToCourse", new { id = courseId });
        }

    }
}