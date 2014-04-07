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
            var profileuser = db.UserProfiles.Where(u => u.UserName == username).FirstOrDefault();

            //var game = db.Games.Include("Players").Where(g => g.GameID == game_id).FirstOrDefault();

            int friendCount = profileuser.Friends.Count;
            int gameCount = profileuser.Results.Count;
            int winCount = 0;
            foreach (Result r in profileuser.Results)
            {
                if (r.Outcome == OutcomeType.Win) winCount++;
            }
            ViewData["friendCount"] = friendCount;
            ViewData["gameCount"] = gameCount;
            ViewData["winCount"] = winCount;
            ViewData["profileuser"] = profileuser;

            if (this.User != null)
            {
                var user = db.UserProfiles.SingleOrDefault(u => u.UserName == this.User.Identity.Name);
                ViewData["user"] = user;
            }

            return View();
        }

        public ActionResult Friends(string username)
        {
            //someone else's friend page, create a hash table with userid's and friend request statuses
            //    --if it doesn't exist then show add friend
            //    --if it is pending/requested/accepted show pending/requested/accepted
            //    --if it is rejected then don't display a button




            if (this.User != null)
            {
                var user = db.UserProfiles.SingleOrDefault(u => u.UserName == this.User.Identity.Name);
                ViewData["user"] = user;
            }

            var Username = username;
            var profileuser = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
            int friendCount = profileuser.Friends.Count;
            profileuser.Friends = db.Friends.Where(f => f.User.UserName == profileuser.UserName).ToList<Friend>();

            //var user = db.UserProfiles.Include("Friends").Where(g => g.GameID == game_id).FirstOrDefault();

            int gameCount = profileuser.Results.Count;
            int winCount = 0;
            foreach (Result r in profileuser.Results)
            {
                if (r.Outcome == OutcomeType.Win) winCount++;
            }
            ViewData["friendCount"] = friendCount;
            ViewData["gameCount"] = gameCount;
            ViewData["winCount"] = winCount;
            ViewData["profileuser"] = profileuser;
            return View();
        }

       


    }
}
