using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data;
using System.Net;
using WeatherApplication.Models.Classes;
using PagedList;

namespace WeatherApplication.Controllers
{
    public class QualitiesController : Controller
    {
        private WeatherContext db = new WeatherContext();
        // GET: Qualities
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
            var qualities = db.Qualities.Include(q => q.Application).Include(q => q.Feature);
            if (!string.IsNullOrEmpty(search))
            {
                qualities = qualities.Where(q => q.Application.Head.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    qualities = qualities.OrderByDescending(q => q.Application.Head);
                    break;
                default:
                    qualities = qualities.OrderBy(q => q.Application.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(qualities.ToPagedList(PageNumber,PageSize));
        }
        //Get Create
        public ActionResult Create()
        {

           ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "ApplicationID");
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID");
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Quality quality)
        {
            if (ModelState.IsValid)
            {
                db.Qualities.Add(quality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "ApplicationID",quality.ApplicationID);
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID",quality.FeatureID);
            return View(quality);
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality quality = db.Qualities.Find(id);
            if (quality==null)
            {
                return HttpNotFound();
            }
            return View(quality);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality quality = db.Qualities.Find(id);
            if (quality == null)
            {
                return HttpNotFound();
            }

            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "ApplicationID");
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID");
            return View(quality);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Quality quality)
        {
            try
            {

                db.Entry(quality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Quality)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The review was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Quality)databaseEntry.ToObject();
                    if (databaseValues.ApplicationID != clientValues.ApplicationID)
                        ModelState.AddModelError("ApplicationID", "Current Value:" + db.Applications.Find(databaseValues.ApplicationID));
                    if (databaseValues.FeatureID != clientValues.FeatureID)
                        ModelState.AddModelError("FeatureID", "Current Value:" + db.Features.Find(databaseValues.FeatureID));

                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value"
                 + " The edit operation was cancelled and the current values in the database have been displayed"
                 + " If you still want to edit this record click the save button again"
                 + " Otherwise click the back to list hyperlink.");
                    quality.RowVwrsion = databaseValues.RowVwrsion;
                        




                }
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again, and if the problem persists contact your system administrator.");

            }


            ViewBag.ApplicationID = new SelectList(db.Applications, "ApplicationID", "ApplicationID", quality.ApplicationID);
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID", quality.FeatureID);
            return View(quality);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quality quality = db.Qualities.Find(id);
            if (quality == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
                {
                    if (quality==null)
                    {
                        ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                    }
                    else
                    {
                        ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                    }
                }
             
            return View(quality);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Quality quality)
        {
            try
            {
                db.Entry(quality).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                return RedirectToAction("Delete", new { concurrencyError = true, id = quality.QualityID });
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again, and if the problem persists contact your system administrator.");
            }
            return View(quality);
        }

    }
}