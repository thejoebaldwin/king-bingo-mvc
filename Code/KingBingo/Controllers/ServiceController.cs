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


        public string getPostBody(HttpRequestBase request)
        {
            /*
            System.IO.Stream str; 
            String strmContents;
            Int32 counter, strLen, strRead;
            // Create a Stream object.
            str = request.InputStream;
            // Find number of bytes in stream.
            strLen = Convert.ToInt32(str.Length);
            // Create a byte array.
            byte[] strArr = new byte[strLen];
            // Read stream into byte array.
            strRead = str.Read(strArr, 0, strLen);

            // Convert byte array to a text string.
            strmContents = "";
            for (counter = 0; counter < strLen; counter++)
            {
                strmContents = strmContents + strArr[counter].ToString();
            }
            */
            string data = new System.IO.StreamReader(Request.InputStream).ReadToEnd();

            return data;
        }
      
        public ActionResult V0()
        {
            ViewBag.Status = "ok";
            ViewBag.Command = "";
            ViewBag.Message = "";
            if (Request.HttpMethod == "POST")
            {
                if (Request.QueryString["cmd"] != null)
                {
                    var cmd = Request.QueryString["cmd"];
                    switch (cmd)
                    {
                        case "allgames":
                            ViewData["Games"] = db.Games;
                            ViewBag.Command = "allgames";
                            ViewBag.Message = "Successfully retrieved list of all games";
                            break;
                        case "allusers":
                            ViewData["Users"] = db.UserProfiles;
                            ViewBag.Command = "allusers";
                            ViewBag.Message = "Successfully retrieved list of all users";
                            break;
                        case "getuser":
                            string json = getPostBody(Request);
                            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                            ViewBag.Command = "getuser";
                            int user_id = data.user_id;
                            ViewData["User"] = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
                            break;
                        case "createuser":
                            ViewBag.Command = "createuser";
                            break;
                        case "getnumber":
                            ViewBag.Command = "getnumber";
                            break;
                        case "joingame":
                            ViewBag.Command = "joingame";
                            break;
                        case "quitgame":
                            ViewBag.Command = "quitgame";
                            break;

                    }
                }
                return View();
            }
            else
            {
                return View("Help");
            }
        }

       

    }
}
