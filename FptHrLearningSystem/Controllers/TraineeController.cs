using FptHrLearningSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace FptHrLearningSystem.Controllers
{
    public class TraineeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Trainee
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            var classroomIds=db.TraineeClassrooms.Where(t => t.TraineeId == userId).Select(t => t.ClassroomId).ToList();
            var classrooms = db.classrooms
              .Where(c => classroomIds.Contains(c.Id)).Include(c => c.ClassProfile).Include(c => c.Course).Include(c => c.Course.Category).ToList();
            return View(classrooms);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classroom = db.classrooms.Include(c => c.Course).FirstOrDefaultAsync(m => m.Id == id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            return View(classroom);
        }
    }
}