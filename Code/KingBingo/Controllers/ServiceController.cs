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
            ViewBag.Status = "ok";
            ViewBag.Command = "allgames";
            ViewBag.Message = "successfully retrieved list of active games";
            if (Request.QueryString["cmd"] != null)
            {
                var cmd = Request.QueryString["cmd"];
                switch (cmd)
                {
                    case "allgames":
                        ViewData["Games"] = db.Games;
                        break;
                    case "allusers":
                        ViewData["Users"] = db.UserProfiles;
                        break;                      
                    case "getuser":
                        var user_id = System.Convert.ToInt32(Request.QueryString["user_id"]);
                        ViewData["User"] = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
                        break;
                    case "createuser":
                        break;
                    case "getnumber":
                        break;
                    case "joingame":
                        break;
                    case "quitgame":
                        break;

                }
            }
            return View();
        }

       

    }
}
