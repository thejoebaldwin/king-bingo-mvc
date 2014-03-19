using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using KingBingo.Models;

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
            int friendCount = user.Friends.Count;
            int gameCount = user.Results.Count;
            int winCount = 0;
            foreach(Result r in user.Results)
            {
                if (r.Outcome == OutcomeType.Win) winCount++;
            }
            ViewData["friendCount"] = friendCount;
            ViewData["gameCount"] = gameCount;
            ViewData["winCount"] = winCount;
            ViewData["User"] = user;
            return View();
        }

        public ActionResult Friends(string username)
        {

            var Username = username;
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
            int friendCount = user.Friends.Count;
            user.Friends = db.Friends.Where(f => f.User.UserName == user.UserName).ToList<Friend>();
            int gameCount = user.Results.Count;
            int winCount = 0;
            foreach (Result r in user.Results)
            {
                if (r.Outcome == OutcomeType.Win) winCount++;
            }
            ViewData["friendCount"] = friendCount;
            ViewData["gameCount"] = gameCount;
            ViewData["winCount"] = winCount;
            ViewData["User"] = user;
            return View();
        }


    }
}
