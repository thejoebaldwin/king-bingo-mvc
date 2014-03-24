using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using KingBingo.Models;
using System.Web.Security;
using WebMatrix.WebData;

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

        protected void createUser(string username, string password, bool admin)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (admin)
            {
                if (!roles.RoleExists("Admin"))
                {
                    roles.CreateRole("Admin");
                }
            }

            if (membership.GetUser(username, false) == null)
            {
                membership.CreateUserAndAccount(username, password);
            }
            if (!roles.GetRolesForUser(username).Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { username }, new[] { "admin" });
            }

        }

       

        public ActionResult V0(string operation)
        {
            ViewBag.Status = "ok";
            ViewBag.Command = "";
            ViewBag.Message = "";
            if (Request.HttpMethod == "POST")
            {
                
                   
                    string json = getPostBody(Request);
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    if (operation == "allgames")
                    {
                        ViewData["Games"] = db.Games;
                        ViewBag.Operation = "allgames";
                        ViewBag.Message = "Successfully retrieved list of all games";
                    }
                    if (operation == "allusers")
                    {
                        ViewData["Users"] = db.UserProfiles;
                        ViewBag.Operation = "allusers";
                        ViewBag.Message = "Successfully retrieved list of all users";
                    }
                    else if (operation == "getuser")
                    {
                        ViewBag.Operation = "getuser";
                        ViewBag.Message = "Successfully retrieved user";
                        int user_id = data.user_id;
                        ViewData["User"] = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
                    }
                    else if (operation == "createuser")
                    {
                        ViewBag.Operation = "createuser";
                        ViewBag.Message = "Successfully created user";
                        int timestamp = data.timestamp;
                        string username = data.username;
                        string password = data.password;
                        //need to have check for if username already exists!!
                        createUser(username, password, false);
                        ViewData["User"] = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
                    }
                    else if (operation == "creategame")
                    {
                        ViewBag.Operation = "creategame";
                        ViewBag.Message = "Successfully created game";
                    }
                    else if (operation == "inviteusers")
                    {
                        ViewBag.Operation = "inviteusers";
                        ViewBag.Message = "Successfully invited users";
                    }
                    else if (operation == "addfriend")
                    {
                        ViewBag.Operation = "addfriend";
                        ViewBag.Message = "Successfully added friend";
                        int timestamp = data.timestamp;
                        int user_id = data.user_id;
                        int friend_id = data.friend_id;
                        var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
                        var friend_user = db.UserProfiles.SingleOrDefault(u => u.UserId == friend_id);
                        var friend1 = new Friend { User = user, FriendUser = friend_user, Created = DateTime.Now, Status = RequestStatus.Requested };
                        var friend2 = new Friend { User = friend_user, FriendUser = user, Created = DateTime.Now, Status = RequestStatus.Pending };
                        db.Friends.Add(friend1);
                        db.Friends.Add(friend2);
                        db.SaveChanges();
                        user.Friends.Add(friend1);
                        friend_user.Friends.Add(friend2);
                        db.SaveChanges();
                    }
                    else if (operation == "acceptfriend")
                    {
                        ViewBag.Operation = "acceptfriend";
                        ViewBag.Message = "Successfully accepted friend";
                    }
                    else if (operation == "rejectfriend")
                    {
                        ViewBag.Operation = "rejectfriend";
                        ViewBag.Message = "Successfully added friend";
                    }
                    else if (operation == "getnumber")
                    {
                        ViewBag.Operation = "getnumber";
                        ViewBag.Message = "Successfully retrieved number";
                    }
                    else if (operation == "joingame")
                    {
                        ViewBag.Operation = "joingame";
                        ViewBag.Message = "Successfully joined game";
                        int timestamp = data.timestamp;
                        int game_id = data.game_id;
                        int user_id = data.user_id;
                        //create game here 
                    }
                    else if (operation == "quitgame")
                    {
                        ViewBag.Operation = "quitgame";
                        ViewBag.Message = "Successfully quit game";
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
