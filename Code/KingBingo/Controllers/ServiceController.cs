using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using KingBingo.Models;

namespace KingBingo.Controllers
{
    public class ServiceController : Controller
    {
        private KingBingoContext db = new KingBingoContext();
        //
        // GET: /Service/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult V0()
        {

            if (Request.QueryString["cmd"] != null)
            {
                var cmd = Request.QueryString["cmd"];
                switch (cmd)
                {
                    case "allgames":
                        ViewData["Games"] = db.Games;
                        break;
                }
            }
            return View();
        }

       

    }
}
