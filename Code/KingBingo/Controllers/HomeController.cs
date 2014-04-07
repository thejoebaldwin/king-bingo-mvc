using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;

namespace KingBingo.Controllers
{
    public class HomeController : Controller
    {


        private KingBingoContext db = new KingBingoContext();

        public ActionResult Index()
        {
            ViewBag.Title = "King Bingo";
            ViewBag.Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim";

            var users = db.UserProfiles;

            ViewData["users"] = users;

            return View();


        }

        public ActionResult Search()
        {
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == this.User.Identity.Name);
            ViewData["user"] = user;
            if (Request.QueryString["term"] != null)
            {
                var term = Request.QueryString["term"];
                var users = db.UserProfiles.Where(u => u.UserName.Contains(term));
                ViewData["users"] = users;
            }
            else
            {
                var users = db.UserProfiles;
                ViewData["users"] = users;
            }
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


        public ActionResult Ranking()
        {
            ViewBag.Title = "Ranking";
            ViewBag.Message = "";

            return View();
        }


    }
}
