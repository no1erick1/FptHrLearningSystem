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
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Categories
        public ActionResult Index(int page =1, int pageSize=3)
        {
            var model = ListAllPaging(page, pageSize);
            return View(model);
        }
        public IEnumerable<Category> ListAllPaging(int page, int pageSize)
        {
            return db.categories.OrderByDescending(x=>x.Id).ToPagedList(page,pageSize);
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.categories.Add(category);
                    db.SaveChanges();
                    SetAlert("Add user be successfull!", "success");
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(category);
        }
        // GET: Categories/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.categories.FirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        // GET: Categories/Edit/
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        //check categories exxted
        private bool CategoryExists(int id)
        {
            return db.categories.Any(e => e.Id == id);
        }
        // POST: Categories/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch(RetryLimitExceededException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }
        // GET: Categories/Delete/
        public bool DeleteId(int id)
        {
            try
            {
                var category = db.categories.Find(id);
                db.categories.Remove(category);
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
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //var category = db.categories.FirstOrDefault(m => m.Id == id);
            //if (category == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(category);
            DeleteId(id);
            return RedirectToAction("Index");
        }
        
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var category = db.categories.Find(id);
        //    db.categories.Remove(category);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}