using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KingBingo.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            ViewBag.Title = "King Bingo - Contact";
            return View();
        }

        public void Create(string name, string email, string comments)
        {
            RedirectToAction("ThankYou");
        }

        public ActionResult ThankYou()
        {
            ViewBag.Title = "King Bingo - Contact";
            return View();
        }

    }
}
