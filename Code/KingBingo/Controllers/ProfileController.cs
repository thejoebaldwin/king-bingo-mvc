using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KingBingo.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index(string username)
        {
            var Username = username;

            ViewData["Username"] = username;

            return View();

           
        }

    }
}
