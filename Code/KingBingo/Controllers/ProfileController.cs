using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;

namespace KingBingo.Controllers
{
    public class ProfileController : Controller
    {
        private KingBingoContext db = new KingBingoContext();
        //
        // GET: /Profile/

        public ActionResult Index(string username)
        {
            var Username = username;
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
            ViewData["User"] = user;
            return View();
        }

    }
}
