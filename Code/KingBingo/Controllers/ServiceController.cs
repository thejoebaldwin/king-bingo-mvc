using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KingBingo.DAL;
using KingBingo.Models;
using System.Web.Security;
using WebMatrix.WebData;
using System.IO;
using MarkdownSharp;
using System.Collections;

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
            ViewBag.status = "ok";
            ViewBag.command = "";
            ViewBag.message = "";
            if (Request.HttpMethod == "POST")
            {
                try
                {
                    string json = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    if (operation == "auth")
                    {
                        string username = data.username;
                        string hash = data.hash;

                        var user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
                        ViewBag.operation = "auth";
                        ViewData["hash"] = user.AuthHash();
                        if (hash == user.AuthHash())
                        {

                            user.GenerateAuthenticationToken();

                            db.SaveChanges();
                            ViewData["user"] = user;

                            ViewBag.message = "Successfully authenticated";
                        }
                        else
                        {
                            ViewBag.status = "error";
                            ViewBag.message = "Password hash did not match with given user_id";
                        }
                    }
                    else if (operation == "createuser")
                    {
                        ViewBag.operation = "createuser";
                        string username = data.username;
                        string password = data.password;
                        string email = data.email;
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

                                user.WinCount = 0;
                                user.FriendCount = 0;
                                user.Rank = 0;
                                user.GameCount = 0;

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
                                ViewData["user"] = user;
                                ViewBag.message = "Successfully created user";
                            }
                            else
                            {
                                ViewBag.status = "error";
                                ViewBag.message = "email already exists";
                            }
                        }
                        else
                        {
                            ViewBag.status = "error";
                            ViewBag.message = "user already exists";
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
                                ViewBag.message = "Authentication Token Invalid";
                                ViewBag.status = "error";
                            }
                            else
                            {
                                if (operation == "allgames")
                                {
                                    int resultSize = 25;
                                    int page = 0;
                                    if (data.page != null)
                                    {
                                        page = data.page;
                                    }
                                    ViewData["games"] = db.Games.OrderBy(g => g.Created).Skip(page * resultSize).Take(resultSize);
                                    ViewBag.operation = "allgames";
                                    ViewBag.message = "Successfully retrieved list of all games";
                                }
                                if (operation == "allusers")
                                {
                                    int resultSize = 25;
                                    int page = 0;
                                    if (data.page != null)
                                    {
                                        page = data.page;
                                    }
                                    ViewData["users"] = db.UserProfiles.OrderBy(u => u.UserName).Skip(page * resultSize).Take(resultSize);
                                    ViewBag.operation = "allusers";
                                    ViewBag.message = "Successfully retrieved list of all users";
                                }
                                else if (operation == "getuser")
                                {
                                    ViewBag.operation = "getuser";
                                    ViewBag.message = "Successfully retrieved user";
                                    int query_user_id = data.query_user_id;
                                    var users = new List<UserProfile>();
                                    users.Add(db.UserProfiles.SingleOrDefault(u => u.UserId == query_user_id));
                                    ViewData["users"] = users;
                                }
                                else if (operation == "creategame")
                                {
                                    createGame(data);
                                }
                                else if (operation == "inviteusers")
                                {
                                    ViewBag.operation = "inviteusers";
                                    ViewBag.message = "Successfully invited users";
                                    //CREATE NOTIFICATIONS

                                }
                                else if (operation == "addfriend")
                                {
                                    ViewBag.operation = "addfriend";
                                    ViewBag.message = "Successfully added friend";
                                    int friend_id = data.friend_id;

                                    var friend_user = db.UserProfiles.SingleOrDefault(u => u.UserId == friend_id);
                                    var friend1 = new Friend { User = user, FriendUser = friend_user, Status = RequestStatus.Requested };
                                    var friend2 = new Friend { User = friend_user, FriendUser = user, Status = RequestStatus.Pending };
                                    db.Friends.Add(friend1);
                                    db.Friends.Add(friend2);
                                    db.SaveChanges();
                                    user.Friends.Add(friend1);
                                    friend_user.Friends.Add(friend2);
                                    db.SaveChanges();
                                }
                                else if (operation == "acceptfriend")
                                {
                                    ViewBag.operation = "acceptfriend";
                                    ViewBag.message = "Successfully accepted friend";
                                }
                                else if (operation == "rejectfriend")
                                {
                                    ViewBag.operation = "rejectfriend";
                                    ViewBag.message = "Successfully added friend";
                                }
                                else if (operation == "getnumber")
                                {
                                    getNumber(data);
                                }
                                else if (operation == "joingame")
                                {
                                    joinGame(data);
                                }
                                else if (operation == "quitgame")
                                {
                                    quitGame(data);
                                }
                                else if (operation == "updateuser")
                                {
                                    ViewBag.operation = "updateuser";
                                    ViewBag.message = "Successfully updated user";
                                }
                                else if (operation == "getfriends")
                                {
                                    ViewBag.operation = "getfriends";
                                    ViewBag.message = "Successfully retrieved list of friends";
                                    //get all friendship requests
                                    var friends = db.Friends.Include("FriendUser").Where(f => f.User == user);
                                    ViewData["friends"] = friends;
                                }
                            }
                        }
                        else
                        {
                            ViewBag.operation = operation;
                            ViewBag.status = "error";
                            ViewBag.message = "missing authentication data";
                        }


                    } // before this
                 
                }
                catch (Exception ex)
                {
                    ViewBag.status = "error";
                    ViewBag.message = "check your json and documentation for proper format";
                }
                return View();
            }
            else
            {
                ViewBag.markdown = MarkdownForOperation(operation);
                return View("Help");
            }
        }

        void getNumber(dynamic data)
        {
            ViewBag.operation = "getnumber";
            ViewBag.message = "Successfully retrieved number";
            int game_id = data.game_id;


            int user_id = data.user_id;
         
            var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
            //var game = db.Games.SingleOrDefault(g => g.GameID == game_id);
            var game = db.Games.Include("Players").Where(g => g.GameID == game_id).FirstOrDefault();
            if (!game.Players.Contains(user))
            {
                ViewBag.Status = "error";
                ViewBag.message = "User not joined to game.";
            }
            else
            {
                ViewBag.message = "Successfully retrieved number";
                int number = game.GetNextNumber();
                ViewBag.nextnumbertime = game.NextNumberTime;
                if (number > -1)
                {
                    ViewBag.number = Game.BingofyNumber(number);
                }
                else
                {
                    ViewBag.status = "error";
                    ViewBag.message = "All Numbers have been drawn for this game";
                    ViewBag.number = -1;
                }
                db.SaveChanges();
            }
         
        }

        //public string ToUnixTime(DateTime date)
        //{
        //    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //    return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds).ToString();
        //}

        void quitGame(dynamic data)
        {
            ViewBag.operation = "quitgame";
           
            int user_id = data.user_id;
            int game_id = data.game_id;
            var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
            var game = db.Games.SingleOrDefault(g => g.GameID == game_id);
            if (game.Players != null && game.Players.Contains(user))
            {
                game.Players.Remove(user);
                //is game empty then close it
                user.Game = null;
                db.SaveChanges();
                ViewBag.message = "Successfully quit game";
            }
            else
            {
                ViewBag.message = "User was not joined to the game";
                ViewBag.Status = "error";
            }
            
        }

   
        void joinGame(dynamic data)
        {
            ViewBag.operation = "joingame";
            int user_id = data.user_id;
            int game_id = data.game_id;
            var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
            var game = db.Games.SingleOrDefault(g => g.GameID == game_id);
          
            if (game.Players != null && game.Players.Contains(user))
            {
                ViewBag.status = "error";
                ViewBag.message = "User is already joined to game";
            }
            else
            {
                ViewBag.message = "Successfully joined game";
                if (user.Game != null)
                {
                    user.Game.Players.Remove(user);
                }
                ViewData["game"] = game;
                GameCard gamecard = game.GetNextGameCard();
                gamecard = db.GameCards.SingleOrDefault(gc => gc.GameCardID == gamecard.GameCardID);
                ViewData["gamecard"] = gamecard;
                user.GameCard = gamecard;
                user.Game = game;
                if (game.Players == null) game.Players = new List<UserProfile>();
                game.Players.Add(user);
                db.SaveChanges();
            }
        }

        void createGame(dynamic data)
        {
            ViewBag.operation = "creategame";
            ViewBag.message = "Successfully created game";
            Game game = new Game();
            game.Name = data.name;
            game.Description = data.description;
            game.WinLimit = data.win_limit;
            game.UserLimit = data.user_limit;
            game.Speed = data.speed;
            game.Created = DateTime.Now;
            game.Private = data["private"];
            game.NumbersIndex = 0;
            //game.Numbers = new List<int>();
            int user_id = data.user_id;
            game.Players = new List<UserProfile>();

            UserProfile createdByUser = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
            createdByUser.Game = game;
            game.Players.Add(createdByUser);
        

            //generate unique gamecards
            game.GenerateGameCards(game.UserLimit);
            foreach (GameCard gc in game.GameCards)
            {
                db.GameCards.Add(gc);
            }

            string[] splitString = { "," };
            string temp = data.player_ids;
            string[] playerIDs = temp.Split(splitString, StringSplitOptions.None);
            //loop through request users
            foreach (string player_id in playerIDs)
            {
                int tempUser_id = System.Convert.ToInt32(player_id);
                var playerUser = db.UserProfiles.SingleOrDefault(u => u.UserId == tempUser_id);
              
                
                //create an invite notification for each player                                        
            }
            createdByUser.GameCard = game.GetNextGameCard();
          
          
        

            ViewData["gamecard"] = createdByUser.GameCard;
           
            game.Results = new List<Result>();
            db.Games.Add(game);
            db.SaveChanges();
            List<Game> games = new List<Game>();
            games.Add(game);
            ViewData["games"] = games;
        }


        string MarkdownForOperation(string operation)
        {
            if (operation == "" || operation == null) operation = "main";
            string path = string.Concat(HttpContext.Request.PhysicalApplicationPath, "\\Views\\Service\\docs\\", operation, ".md");
            string contents;

            using (StreamReader sr = new StreamReader(path))
            {
                Markdown md = new Markdown();
                contents = md.Transform(sr.ReadToEnd());
            }
            return contents;


        }

    }
}
