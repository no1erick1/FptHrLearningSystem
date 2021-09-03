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
    public class ClassProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ClassProfiles
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            var model = ListAllPaging(page, pageSize);
            return View(model);
        }
        public IEnumerable<ClassProfile> ListAllPaging(int page, int pageSize)
        {
            return db.ClassProfiles.OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
        }
        // GET: ClassProfiles/Details/
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classProfile = db.ClassProfiles.FirstOrDefault(m => m.Id == id);
            if (classProfile == null)
            {
                return HttpNotFound();
            }
            return View(classProfile);
        }
        //GET: ClassProfiles/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: ClassProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,EnrolmentYear,PerferredCampus")] ClassProfile classProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ClassProfiles.Add(classProfile);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(classProfile);
        }
        // GET: ClassProfiles/Edit/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var classProfile = db.ClassProfiles.Find(id);
            if (classProfile == null)
            {
                return HttpNotFound();
            }
            return View(classProfile);
        }
        // POST: ClassProfiles/Edit/
        private bool ClassProfileExists(int id)
        {
            return db.ClassProfiles.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Id,Code,EnrolmentYear,PerferredCampus")] ClassProfile classProfile)
        {
            if (id != classProfile.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(classProfile).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (RetryLimitExceededException)
                {
                    if (!ClassProfileExists(classProfile.Id))
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
            return View(classProfile);
        }
        public bool DeleteId(int id)
        {
            try
            {
                var classProfile = db.ClassProfiles.Find(id);
                db.ClassProfiles.Remove(classProfile);
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
    }
}