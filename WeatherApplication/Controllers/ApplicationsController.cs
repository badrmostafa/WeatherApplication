using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using WeatherApplication.Models.Classes;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList;

namespace WeatherApplication.Controllers
{
    public class ApplicationsController : Controller
    {
        private WeatherContext db = new WeatherContext();
        // GET: Applications
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Head = string.IsNullOrEmpty(sort) ? "head_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var applications = from a in db.Applications select a;
            if (!string.IsNullOrEmpty(search))
            {
                applications = applications.Where(a => a.Head.ToUpper().Contains(search.ToUpper()) ||
                  a.Text1.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    applications = applications.OrderByDescending(a => a.Head);
                    break;
                default:
                    applications = applications.OrderBy(a => a.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(applications.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Application application = db.Applications.Find(id);
            if (application==null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //Post Create
        public ActionResult Create(Application application)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Application application)
        {
            try
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Application)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The application was deleted by another user.");

                }
                else
                {
                    var databaseValues = (Application)databaseEntry.ToObject();
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Degree != clientValues.Degree)
                        ModelState.AddModelError("Degree", "Current Value:" + databaseValues.Degree);
                    if (databaseValues.Text1 != clientValues.Text1)
                        ModelState.AddModelError("Text1", "Current Value:" + databaseValues.Text1);
                    if (databaseValues.Text2 != clientValues.Text2)
                        ModelState.AddModelError("Text2", "Current Value:" + databaseValues.Text2);
                    if (databaseValues.Text3 != clientValues.Text3)
                        ModelState.AddModelError("Text3", "Current Value:" + databaseValues.Text3);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The " + "edit operation was cancelled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    application.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again,and if the problem persists contact your system administrator.");
            }
            return View(application);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (application==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";

                }
            }
            return View(application);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Application application)
        {
            try
            {
                db.Entry(application).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = application.ApplicationID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(application);
        }
    }
}