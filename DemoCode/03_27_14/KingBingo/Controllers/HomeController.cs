using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

using System.Data.Entity;
using KingBingo.Models;

namespace KingBingo.Controllers
{
    public class HomeController : Controller
    {

        KingBingo.Models.kingbingoEntities db = new KingBingo.Models.kingbingoEntities();


        public ActionResult Index()
        {
            ViewBag.Message = "Hello World!";
            var gameID = 1;
            ViewBag.gameID = gameID;


            DbSet<Game> games = db.Games;
            ViewData["games"] = games;

            

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
    }
}
