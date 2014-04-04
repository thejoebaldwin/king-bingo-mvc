using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using System.Web.Security;
using WebMatrix.WebData;
using KingBingo.Models;
using System.Data.Entity;
using System.IO;
using System.Drawing;

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

            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == this.User.Identity.Name);
            ViewData["user"] = user;
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
            DbSet<Game> games = db.Games;
            ViewData["Games"] = games;
            return View();
        }

        [Authorize]
        public ActionResult GameDetail(int id)
        {
            var game = db.Games.SingleOrDefault(g => g.GameID == id);
            ViewData["Game"] = game;
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

        [Authorize]
        public ActionResult UploadComplete()
        {

            return View();
        }

         [Authorize]
        [HttpPost]
        public ActionResult UploadImages(HttpPostedFileBase file, int user_id, int x0, int y0, int x1, int y1)
        {
           /*
             if (file.Count() <= 1)
            {
                return RedirectToAction("BrowseImages");
            }
             */
            //foreach (var image in uploadImages)
            //{
            if (file.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(file.ContentLength);
                    }
                    System.Drawing.Image img;

                    using (var ms = new System.IO.MemoryStream(imageData)) {
                        img = System.Drawing.Image.FromStream(ms);
                    }

                    img = cropImage(img, new Rectangle(x0, y0, x1-x0, y1-y0));
                    
                   MemoryStream newstream = new MemoryStream();
           
             
           
                    img.Save(newstream, System.Drawing.Imaging.ImageFormat.Png);
           
          
          

                 imageData = newstream.ToArray();

                    var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
                    user.ProfileImage = imageData;
                    db.SaveChanges();
                 
                }
           // }
           return RedirectToAction("Profile");

        }

         private static Image cropImage(Image img, Rectangle cropArea)
         {
             Bitmap bmpImage = new Bitmap(img);
             Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
             return (Image)(bmpCrop);
         }

    }


   
}
