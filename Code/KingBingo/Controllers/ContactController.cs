using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using KingBingo.Models;

using System.Data.Entity;

namespace KingBingo.Controllers
{
    public class ContactController : Controller
    {
        private KingBingoContext db = new KingBingoContext();
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            ViewBag.Title = "King Bingo - Contact";
            return View();
        }


        public ActionResult All()
        {
            ViewBag.Title = "King Bingo - Support";
            DbSet<Support> supports = db.Supports;
            ViewData["supports"] = supports;
            return View();
        }

        public ActionResult Create(string name, string email, string comments)
        {
            try
            {
                Support support = new Support();
                support.Email = email;
                support.Name = name;
                support.Comments = comments;
                support.Created = DateTime.Now;
                db.Supports.Add(support);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ThankYou");

        }

        public ActionResult ThankYou()
        {
            ViewBag.Title = "King Bingo - Contact";
            return View();
        }

    }
}
