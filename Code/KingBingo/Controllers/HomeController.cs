using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KingBingo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "King Bingo";
            ViewBag.Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Title = "King Bingo - Terms";
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Title = "King Bingo - Privacy";
            ViewBag.Message = "Your app description page.";

            return View();
        }
    }
}
