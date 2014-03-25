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

                    if (operation == "auth")
                    {
                        //int user_id = data.user_id;
                        string username = data.username;
                        string hash = data.hash;

                        //byte[] raw = UserProfile.GetBytes(hash);
                      
                        //back to string
                       // hash = System.Text.Encoding.UTF8.GetString(raw);

                        var user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
                        ViewBag.Operation = "auth";
                        ViewData["Hash"] = user.AuthHash();
                        if (hash == user.AuthHash())
                        {

                            user.AuthenticationToken = Guid.NewGuid().ToString();
                            user.AuthenticationTokenExpires = DateTime.Now.AddDays(7);
                            db.SaveChanges();
                            ViewData["User"] = user;
                          
                            ViewBag.Message = "Successfully authenticated";
                        }
                        else
                        {
                            ViewBag.Status = "error";
                            ViewBag.Message = "Password hash did not match with given user_id";
                        }
                    }
                    else if (operation == "createuser")
                    {
                        ViewBag.Operation = "createuser";
                        string username = data.username;
                        string password = data.password;
                        string email = data.email;
                        //need to have check for if username already exists!!
                        var user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
                        if (user == null)
                        {
                            user = db.UserProfiles.SingleOrDefault(u => u.Email == email);
                            if (user == null)
                            {
                                createUser(username, password, false);
                                user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
                                //hash it once and store it
                                user.PasswordHash = UserProfile.SHA1(password);
                                user.Created = DateTime.Now;
                                user.Friends = new List<Friend>();
                                user.Results = new List<Result>();
                                user.Name = username;
                                user.Email = email;
                                user.Bio = "";
                             
                               
                                user.Created = DateTime.Now;
                                user.DeviceToken = "";
                                user.Zip = "";
                                user.Birthdate = new DateTime(1977, 10, 25);
                                user.ReceiveEmails = true;
                                user.AuthenticationToken = Guid.NewGuid().ToString();
                                user.AuthenticationTokenExpires = DateTime.Now.AddDays(7);
                                user.ProfileImage = null;
                                user.ConfirmationKey = "";
                                user.Active = true;
                                user.Sex = Sex.Female;
                                user.Confirmed = true;
                                user.Location = new Decimal?[2] { 88, -120 };
                                user.GameCard = null;
                                user.Badges = new List<Badge>(); ;




                                db.SaveChanges();
                                ViewData["User"] = user;
                                ViewBag.Message = "Successfully created user";
                            }
                            else
                            {
                                ViewBag.Status = "error";
                                ViewBag.Message = "email already exists";
                            }
                        }
                        else
                        {
                            ViewBag.Status = "error";
                            ViewBag.Message = "user already exists";
                        }
                    }
                    else
                    {
                        //NEED TO AUTHENTICATE
                        int user_id = data.user_id;
                        var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
                        if (user != null)
                        {
                           

                            string authenticationToken = data.authentication_token;
                           

                            if (user.AuthenticationToken != authenticationToken)
                            {

                                ViewBag.Message = "Authentication Token Expired";
                                ViewBag.Status = "Error";
                            }
                            else
                            {

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
                                    int query_user_id = data.user_id;
                                    ViewData["User"] = db.UserProfiles.SingleOrDefault(u => u.UserId == query_user_id);

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


                                    int friend_id = data.friend_id;

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

                                    int game_id = data.game_id;

                                    //create game here 
                                }
                                else if (operation == "quitgame")
                                {
                                    ViewBag.Operation = "quitgame";
                                    ViewBag.Message = "Successfully quit game";
                                }

                            }
                        }
                        else
                        {
                            ViewBag.Operation = operation;
                            ViewBag.Status = "error";
                            ViewBag.Message = "missing authentication data";
                        }

                    } // before this
                  

              
                return View();
            }
            else
            {
                return View("Help");
            }
        }
    }
}
