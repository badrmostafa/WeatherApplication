using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using WeatherApplication.Models.Classes;
using System.Data;
using PagedList;

namespace WeatherApplication.Controllers
{
    public class DescriptionsController : Controller
    {
        private WeatherContext db = new WeatherContext();
        // GET: Descriptions
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
            var descriptions = from d in db.Descriptions select d;
            if (!string.IsNullOrEmpty(search))
            {
                descriptions = descriptions.Where(d => d.Text1.ToUpper().Contains(search.ToUpper()) ||
                  d.Text2.ToUpper().Contains(search.ToUpper()) ||
                  d.Text3.ToUpper().Contains(search.ToUpper()) ||
                  d.Head1.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head1_desc":
                    descriptions = descriptions.OrderByDescending(d => d.Head1);
                    break;
                default:
                    descriptions = descriptions.OrderBy(d => d.Head1);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 2;
            return View(descriptions.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Description description = db.Descriptions.Find(id);
            if (description==null)
            {
                return HttpNotFound();
            }
            return View(description);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Description description)
        {
            if (ModelState.IsValid)
            {
                db.Descriptions.Add(description);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(description);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Description description = db.Descriptions.Find(id);
            if (description == null)
            {
                return HttpNotFound();
            }
            return View(description);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Description description)
        {
            try
            {
                db.Entry(description).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Description)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The description was deleted by another user.");
                }
                else
                {
                   var databaseValues = (Description)databaseEntry.ToObject();
                    if (databaseValues.Title1 != clientValues.Title1)
                        ModelState.AddModelError("Title1", "Current Value:" + databaseValues.Title1);
                    if (databaseValues.Title2 != clientValues.Title2)
                        ModelState.AddModelError("Title2", "Current Value:" + databaseValues.Title2);
                    if (databaseValues.Text1 != clientValues.Text1)
                        ModelState.AddModelError("Text1", "Current Value:" + databaseValues.Text1);
                    if (databaseValues.Text2 != clientValues.Text2)
                        ModelState.AddModelError("Text2", "Current Value:" + databaseValues.Text2);
                    if (databaseValues.Text3 != clientValues.Text3)
                        ModelState.AddModelError("Text3", "Current Value:" + databaseValues.Text3);
                    if (databaseValues.Degree1 != clientValues.Degree1)
                        ModelState.AddModelError("Degree1", "Current Value:" + databaseValues.Degree1);
                    if (databaseValues.Degree2 != clientValues.Degree2)
                        ModelState.AddModelError("Degree2", "Current Value:" + databaseValues.Degree2);
                    if (databaseValues.Image1 != clientValues.Image1)
                        ModelState.AddModelError("Image1", "Current Value:" + databaseValues.Image1);
                    if (databaseValues.Image2 != clientValues.Image2)
                        ModelState.AddModelError("Image2", "Current Value:" + databaseValues.Image2);
                    if (databaseValues.Head1 != clientValues.Head1)
                        ModelState.AddModelError("Head1", "Current Value:" + databaseValues.Head1);
                    if (databaseValues.Description1 != clientValues.Description1)
                        ModelState.AddModelError("Description1", "Current Value:" + databaseValues.Description1);
                    if (databaseValues.Humidity != clientValues.Humidity)
                        ModelState.AddModelError("Humidity", "Current Value:" + databaseValues.Humidity);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value"
                        + " The edit operation was cancelled and the current values in the database have been displayed"
                        + " If you still want to edit this record click the save button again"
                        + " Otherwise click the back to list hyperlink.");
                    description.RowVersion = databaseValues.RowVersion;

                }

            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again,and if the problem persists contact your system administrator.");
            }
            return View(description);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Description description = db.Descriptions.Find(id);
            if (description == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (description==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";


                }
            }
            return View(description);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Description description)
        {
            try
            {
                db.Entry(description).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = description.DescriptionID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(description);
        }
        
    }
}