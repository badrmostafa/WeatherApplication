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
    public class DownloadsController : Controller
    {
        private WeatherContext db = new WeatherContext();
        // GET: Downloads
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Head = string.IsNullOrEmpty(sort) ? "head_desc" : "";
            var downloads = from d in db.Downloads select d;
            if (!string.IsNullOrEmpty(search))
            {
                downloads = downloads.Where(d => d.Head.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    downloads = downloads.OrderByDescending(d => d.Head);
                    break;
                default:
                    downloads = downloads.OrderBy(d => d.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(downloads.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Find(id);
            if (download==null)
            {
                return HttpNotFound();
            }
            return View(download);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Download download)
        {
            if (ModelState.IsValid)
            {
                db.Downloads.Add(download);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(download);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Find(id);
            if (download == null)
            {
                return HttpNotFound();
            }
            return View(download);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Download download)
        {
            try
            {

                db.Entry(download).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Download)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The review was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Download)databaseEntry.ToObject();
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    if (databaseValues.Image1 != clientValues.Image1)
                        ModelState.AddModelError("Image1", "Current Value:" + databaseValues.Image1);
                    if (databaseValues.Image2 != clientValues.Image2)
                        ModelState.AddModelError("Image2", "Current Value:" + databaseValues.Image2);
                    if (databaseValues.Image3 != clientValues.Image3)
                        ModelState.AddModelError("Image3", "Current Value:" + databaseValues.Image3);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value"
                  + " The edit operation was cancelled and the current values in the database have been displayed"
                  + " If you still want to edit this record click the save button again"
                  + " Otherwise click the back to list hyperlink.");
                    download.RowVersion = databaseValues.RowVersion;


                }


            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again, and if the problem persists contact your system administrator.");
            }
            return View(download);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Download download = db.Downloads.Find(id);
            if (download == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (download==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(download);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Download download)
        {
            try
            {
                db.Entry(download).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = download.DownloadID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again, and if the problem persists contact your system administrator.");
            }
            return View(download);
        }
    }
}