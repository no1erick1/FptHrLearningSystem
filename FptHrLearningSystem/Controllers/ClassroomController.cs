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
    public class ClassroomController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Classroom
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            var classrooms = ListAllPaging(page, pageSize);
            return View(classrooms);
        }
        public IEnumerable<Classroom> ListAllPaging(int page, int pageSize)
        {
            return db.classrooms.Include(c => c.ClassProfile).Include(c => c.Course).OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var classroom = db.classrooms
                .Include(c => c.ClassProfile)
                .Include(c => c.Course)
                .FirstOrDefault(m => m.Id == id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            ViewData["Trainees"] = db.TraineeClassrooms.Include(t => t.Trainee).Where(t => t.ClassroomId == id).ToList();
            return View(classroom);
        }
        public ActionResult Create()
        {
            db.ClassProfiles.ToList();
            ViewData["ClassProfileId"] = new SelectList(db.ClassProfiles, "Id", "Code");
            ViewData["CourseId"] = new SelectList(db.courses, "Id", "Code");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Id,Code,StartDate,EndDate,Year,Semester,Part,ClassProfileId,CourseId")] Classroom classroom)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    classroom.Year = classroom.StartDate.Year;
                    db.classrooms.Add(classroom);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            ViewData["ClassProfileId"] = new SelectList(db.ClassProfiles, "Id", "Code", classroom.ClassProfile);
            ViewData["CourseId"] = new SelectList(db.courses, "Id", "Code", classroom.CourseId);
            return View(classroom);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classroom = db.classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }
            ViewData["ClassProfileId"] = new SelectList(db.ClassProfiles, "Id", "Code");
            ViewData["CourseId"] = new SelectList(db.courses, "Id", "Code");
            return View(classroom);
        }
        private bool ClassroomExists(int id)
        {
            return db.classrooms.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Id,Code,StartDate,EndDate,Year,Semester,Part,ClassProfileId,CourseId")] Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(classroom).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    if (!ClassroomExists(classroom.Id))
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
            ViewData["ClassProfileId"] = new SelectList(db.ClassProfiles, "Id", "Code", classroom.ClassProfile);
            ViewData["CourseId"] = new SelectList(db.courses, "Id", "Code", classroom.CourseId);
            return View(classroom);
        }
        public bool DeleteId(int id)
        {
            try
            {
                var classroom = db.classrooms.Find(id);
                db.classrooms.Remove(classroom);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            DeleteId(id);
            return RedirectToAction("Index");
        }
       
        public ActionResult AddToClassroom(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classroom = db.classrooms.Find(id);
            if (classroom == null)
            {
                return HttpNotFound();
            }

            var traineeIds = db.Users.Where(u => u.UserType == "Trainee").Select(u => u.Id).ToList();
            var trainees = db.Users.Where(u => traineeIds.Contains(u.Id)).ToList();

            var currentTraineeIds = db.TraineeClassrooms.Where(t => t.ClassroomId == id).Select(t => t.TraineeId).ToList();
            var currentTrainees = trainees.Where(t => currentTraineeIds.Contains(t.Id)).ToList();
            var remainingTrainees = trainees.Where(t => !currentTraineeIds.Contains(t.Id)).ToList();

            ViewData["RemainingTrainees"] = remainingTrainees;
            ViewData["CurrentTrainees"] = currentTrainees;

            return View(classroom);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTrainee(int? classroomId, string traineeId)
        {
            if (classroomId != null && traineeId != null)
            {
                var traineeClassroom = new TraineeClassroom()
                {
                    ClassroomId = (int)classroomId,
                    TraineeId = traineeId
                };

                db.TraineeClassrooms.Add(traineeClassroom);
                db.SaveChanges();
            }

            return RedirectToAction("AddToClassroom", new { id = classroomId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveTrainee(int? classroomId, string traineeId)
        {
            var traineeClassroom = db.TraineeClassrooms.FirstOrDefault(t => t.ClassroomId == classroomId && t.TraineeId == traineeId);

            if (traineeClassroom != null)
            {
                db.TraineeClassrooms.Remove(traineeClassroom);
                db.SaveChanges();
            }

            return RedirectToAction("AddToClassroom", new { id = classroomId });
        }
    }
}