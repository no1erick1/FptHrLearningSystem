using FptHrLearningSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;

namespace FptHrLearningSystem.Controllers
{
    public class TrainerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Trainer
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            var courseIds = db.TrainerCourses.Where(t => t.TrainerId == userId).Select(t => t.CourseId).ToList();
            var courses = db.courses.ToList();
            return View(courses);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var course = db.courses.FirstOrDefault(m => m.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewData["Trainers"] = db.TrainerCourses
            .Include(t => t.Trainer)
            .Where(t => t.CourseId == id)
            .ToList();
            return View(course);

        }
    }
}