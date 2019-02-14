using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WeatherApplication.Models.Classes;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList;

namespace WeatherApplication.Controllers
{
    
    public class FeaturesController : Controller
    {
        private WeatherContext db = new WeatherContext();
        // GET: Features
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Head1 = string.IsNullOrEmpty(sort) ? "head1_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var features = from f in db.Features select f;
            if (!string.IsNullOrEmpty(search))
            {
                features = features.Where(f => f.Head1.ToUpper().Contains(search.ToUpper()) ||
                  f.Head2.ToUpper().Contains(search.ToUpper()) ||
                  f.Head3.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head1_desc":
                    features = features.OrderByDescending(f => f.Head1);
                        break;
                default:
                    features = features.OrderBy(f => f.Head1);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(features.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.Features.Find(id);
            if (feature==null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Feature feature)
        {
            if (ModelState.IsValid)
            {
                db.Features.Add(feature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feature);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                return HttpNotFound();
            }
            return View(feature);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Feature feature)
        {
            try
            {
                db.Entry(feature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Feature)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The feature was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Feature)databaseEntry.ToObject();
                    if (databaseValues.Head1 != clientValues.Head1)
                        ModelState.AddModelError("Head1", "Current Value:" + databaseValues.Head1);
                    if (databaseValues.Head2 != clientValues.Head2)
                        ModelState.AddModelError("Head2", "Current Value" + databaseValues.Head2);
                    if (databaseValues.Icon != clientValues.Icon)
                        ModelState.AddModelError("Icon", "Current Value:" + databaseValues.Icon);
                    if (databaseValues.Head3 != clientValues.Head3)
                        ModelState.AddModelError("Head3", "Current Value:" + databaseValues.Head3);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit"
                        + " Was modified by another user after you got the original value."
                        + " The edit operation was canceled and the current values in the database have been displayed"
                        + " If you still want to edit click the save button again"
                        + " Otherwise click the back to list hyperlink.");
                    feature.RowVersion = databaseValues.RowVersion;

                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again, and if the problem persists contact your system administrator.");
            }
            return View(feature);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feature feature = db.Features.Find(id);
            if (feature == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                if (feature==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }

            }
            return View(feature);
        }

        //Post Delete
        [HttpPost]
        public ActionResult Delete(Feature feature)
        {
            try
            {
                db.Entry(feature).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = feature.FeatureID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(feature);
        }












    }
}