using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApplication.Models.Classes;

namespace WeatherApplication.Controllers
{
    public class HomeController : Controller
    {
        private WeatherContext db = new WeatherContext();
        public ActionResult Index()
        {
            ViewBag.application = db.Applications.First();
            //////////////////////////////////////////////
            ViewBag.feature = db.Features.First();
            ViewBag.fe = db.Features.ToList();
            /////////////////////////////////////////////
            ViewBag.slide = db.Slides.First();
            ////////////////////////////////////////////
            ViewBag.description = db.Descriptions.First();
            ViewBag.de = db.Descriptions.ToList();
            //////////////////////////////////////////////
            ViewBag.review = db.Reviews.First();
            ViewBag.re = db.Reviews.ToList();
            ////////////////////////////////////////////////
            ViewBag.download = db.Downloads.First();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}