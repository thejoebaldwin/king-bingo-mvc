using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using System.Web.Security;
using WebMatrix.WebData;
using KingBingo.Models;

namespace KingBingo.Controllers
{
    public class SecureController : Controller
    {
        private KingBingoContext db = new KingBingoContext();
        //
        // GET: /Secure/

        [Authorize]
        public ActionResult Index()
        {

        //    var membership = (SimpleMembershipProvider)Membership.Provider;
          
          //  db.UserProfiles.SingleOrDefault(u => u.UserName == "test2");

            return View();
        }

        [Authorize]
        public ActionResult Profile()
        {

          

            return View();
        }

        [Authorize]
        public ActionResult Notifications()
        {

          

            return View();
        }

        [Authorize]
        public ActionResult Games()
        {

         

            return View();
        }
        
        [Authorize]
        public ActionResult Friends()
        {
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == this.User.Identity.Name);
            user.Friends = db.Friends.Where(f => f.User.UserName == user.UserName).ToList<Friend>();
            ViewData["User"] = user;
            return View();
        }

    }
}
