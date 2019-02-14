using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Data;
using WeatherApplication.Models.Classes;
using PagedList;

namespace WeatherApplication.Controllers
{
    public class SlidesController : Controller
    {
        private WeatherContext db = new WeatherContext();
        // GET: Slides
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.BackgroundImage1 = string.IsNullOrEmpty(sort) ? "backgroundimage1_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var slides = from s in db.Slides select s;
            if (!string.IsNullOrEmpty(search))
            {
                slides = slides.Where(s => s.BackgroundImage1.ToUpper().Contains(search.ToUpper()) ||
                  s.BackgroundImage2.ToUpper().Contains(search.ToUpper()) ||
                  s.BackgroundImage3.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "backgroundimage1_desc":
                    slides = slides.OrderByDescending(s => s.BackgroundImage1);
                    break;
                default:
                    slides = slides.OrderBy(s => s.BackgroundImage1);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(slides.ToPagedList(PageNumber,PageSize));
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Slide slide)
        {
            if (ModelState.IsValid)
            {
                db.Slides.Add(slide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slide);
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide==null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Slide slide)
        {
            try
            {
                db.Entry(slide).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Slide)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The slide was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Slide)databaseEntry.ToObject();
                    if (databaseValues.BackgroundImage1 != clientValues.BackgroundImage1)
                        ModelState.AddModelError("BackgroundImage1", "Current Value:" + databaseValues.BackgroundImage1);
                    if (databaseValues.BackgroundImage2 != clientValues.BackgroundImage2)
                        ModelState.AddModelError("BackgroundImage2", "Current Value:" + databaseValues.BackgroundImage2);
                    if (databaseValues.BackgroundImage3 != clientValues.BackgroundImage3)
                        ModelState.AddModelError("BackgroundImage3", "Current Value:" + databaseValues.BackgroundImage3);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value"
                        + " The edit operation was cancelled and the current values in the database have been displayed"
                        + " If you still want to edit this record click the save button again"
                        + " Otherwise click the back to list hyperlink.");
                    slide.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again,and if the problem persists contact your system administrator.");

            }
            return View(slide);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (slide==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(slide);
        }
        //Post Delete
        [HttpPost]
       public ActionResult Delete(Slide slide)
        {
            try
            {
                db.Entry(slide).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                return RedirectToAction("Delete", new { concurrencyError = true, id = slide.SlideID });
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");


            }
            return View(slide);
        }
    }
}