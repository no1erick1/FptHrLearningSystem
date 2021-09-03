using FptHrLearningSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FptHrLearningSystem.Controllers
{
    
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: User
        [Authorize(Roles = "Admin")]
        public ViewResult Index(int page = 1, int pageSize = 3)
        {
            var applicationDbContext = ListAllPaging(page, pageSize);
            return View(applicationDbContext);
        }
        public IEnumerable<ApplicationUser> ListAllPaging(int page, int pageSize)
        {
            return db.Users.Include(c => c.Roles).Where(u => u.UserType != "Admin").OrderByDescending(x => x.Id).ToPagedList(page, pageSize);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
            ViewBag.UserType = new SelectList(db.Roles.Where(x => !x.Name.Contains("Admin")).ToList(), "Name", "Name",user.UserType);
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        private bool UserExists(String id)
        {
            return db.Users.Any(e => e.Id == id);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
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
            ViewBag.UserType = new SelectList(db.Roles.Where(x => !x.Name.Contains("Admin")).ToList(), "Name", "Name", user.UserType);
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
       
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            DeleteId(id);
            return RedirectToAction("Index");
        }

        public ActionResult EditYourSelf(string id)
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
            ViewBag.UserType = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View(user);
        }
        private bool UserExistsYourSelf(String id)
        {
            return db.Users.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditYourSelf(string id, ApplicationUser user)
        {
            if (id != user.Id)
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
                return RedirectToAction("Index","Users");
            }
            ViewBag.UserType = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View(user);
        }



    }
}