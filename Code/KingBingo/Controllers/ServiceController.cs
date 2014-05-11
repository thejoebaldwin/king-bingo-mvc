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


        public DateTime FromUnixTime(string timestamp)
        {
            // Unix timestamp is seconds past epoch
            double timestampSeconds = Convert.ToDouble(timestamp);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestampSeconds).ToLocalTime();
            return dtDateTime;
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
                        var user = db.UserProfiles.Include("Game").Include("GameCard").SingleOrDefault(u => u.UserName == username);
                        if (user != null)
                        {
                            ViewBag.operation = "auth";
                            ViewData["hash"] = user.AuthHash();
                            if (hash == user.AuthHash())
                            {
                                user.GenerateAuthenticationToken();
                                db.SaveChanges();

                                if (user.Game != null)
                                {
                                    var game = db.Games.Where(g => g.GameID == user.Game.GameID).FirstOrDefault();
                                    List<Game> games = new List<Game>();
                                    games.Add(game);
                                    ViewData["games"] = games;
                                    ViewBag.gamespeed = user.Game.GameSpeed;
                                    if (user.GameCard != null)
                                    {
                                        ViewData["gamecard"] = user.GameCard;
                                    }
                                }
                              
                          
                                ViewData["user"] = user;
                                ViewBag.message = "successfully authenticated";
                                ViewBag.includeprofileimages =true;
                            }
                            else
                            {
                                ViewBag.status = "error";
                                ViewBag.message = "password hash did not match with given username";
                            }
                        }
                        else
                        {
                            ViewBag.status = "error";
                            ViewBag.message = "password hash did not match with given username";
                        }
                    }
                    else if (operation == "allresults")
                    {
                        AllResults();
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
                                user.Location = "88,-120";
                                user.GameCard = null;
                                user.Badges = new List<Badge>(); ;
                                db.SaveChanges();
                                ViewData["user"] = user;
                                ViewBag.message = "successfully created user";
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
                                ViewBag.message = "authentication token invalid";
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
                                    ViewData["games"] = db.Games.Include("Players").Where(g => g.Closed == false).OrderByDescending(g => g.Created).Skip(page * resultSize).Take(resultSize);
                                    ViewBag.operation = "allgames";
                                    ViewBag.message = "successfully retrieved list of all games";
                                }
                                else if (operation == "allnotifications")
                                {
                                    int resultSize = 25;
                                    int page = 0;
                                    if (data.page != null)
                                    {
                                        page = data.page;
                                    }
                                    ViewData["notifications"] = db.Notifications.Where(n => n.User.UserId == user.UserId).OrderByDescending(g => g.Created).Skip(page * resultSize).Take(resultSize);
                                    ViewBag.operation = "allnotifications";
                                    ViewBag.message = "successfully retrieved list of all notifications";
                                }
                                else if (operation == "allusers")
                                {
                                    int resultSize = 25;
                                    int page = 0;
                                    if (data.page != null)
                                    {
                                        page = data.page;
                                    }
                                    ViewData["users"] = db.UserProfiles.OrderBy(u => u.UserName).Skip(page * resultSize).Take(resultSize);
                                    ViewBag.operation = "allusers";
                                    if (data.include_profile_images != null)
                                    {
                                        if (data.include_profile_images == "true")
                                        {
                                            ViewBag.includeprofileimages = true;
                                        }
                                        else
                                        {
                                            ViewBag.includeprofileimages = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.includeprofileimages = false;
                                    }
                                    ViewBag.message = "successfully retrieved list of all users";
                                }
                                else if (operation == "updateuser")
                                {
                                    //need more error messages
                                    ViewBag.operation = "updateuser";
                                    var updateUser = data.user;
                                    if (updateUser != null)
                                    {
                                        if (updateUser.profile_image != null && updateUser.profile_image != string.Empty)
                                        {
                                            string profileimage = updateUser.profile_image;
                                            byte[] buffer = Convert.FromBase64String(profileimage);
                                            MemoryStream ms = new MemoryStream(buffer);
                                            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                                            /*
                                            if (img.Width > 200 || img.Height > 200)
                                            {
                                                ViewBag.status = "error";
                                                ViewBag.message = "profile image cannot be larger than 200 x 200";
                                            }
                                            else if (img.Width != img.Height)
                                            {
                                                ViewBag.status = "error";
                                                ViewBag.message = "profile image must have equal width and height";
                                            }
                                            else
                                            {
                                             */
                                            ViewBag.status = "ok";
                                            user.ProfileImage = buffer;
                                            //ViewBag.message = "profile image updated successfully with " + img.Width.ToString() + "x" + img.Height.ToString() + ",";
                                            // }
                                        }
                                        else
                                        {
                                            ViewBag.message += "profile image is null";
                                        }
                                        if (updateUser.birthdate != null)
                                        {

                                            string timestamp = (string)updateUser.birthdate;
                                            user.Birthdate = FromUnixTime(timestamp);
                                        }
                                        if (updateUser.name != null)
                                        {
                                            user.Name = updateUser.name;
                                        }
                                        if (updateUser.bio != null)
                                        {
                                            user.Bio = updateUser.bio;
                                        }
                                        if (updateUser.zip != null)
                                        {
                                            user.Zip = updateUser.zip;
                                        }
                                        if (updateUser.sex != null)
                                        {
                                            if (updateUser.sex == "Male" || updateUser.sex == "male")
                                            {
                                                updateUser.Sex = Sex.Male;
                                            }
                                            else
                                            {
                                                updateUser.Sex = Sex.Female;
                                            }
                                        }
                                        if (updateUser.location != null)
                                        {
                                            user.Location = updateUser.location;
                                        }
                                        ViewBag.message += "successfully updated user profile";
                                        db.SaveChanges();
                                    }

                                }
                                else if (operation == "getuser")
                                {
                                    ViewBag.operation = "getuser";
                                    ViewBag.message = "auccessfully retrieved user";
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
                                    ViewBag.message = "successfully invited users";
                                    //CREATE NOTIFICATIONS
                                }
                                else if (operation == "addfriend")
                                {
                                    ViewBag.operation = "addfriend";
                                    int friend_user_id = data.friend_user_id;
                                    var friendship = db.Friends.Include("User").Where(f => f.User.UserId == user.UserId && f.FriendUser.UserId == friend_user_id).FirstOrDefault();
                                    if (friendship == null)
                                    {
                                        ViewBag.message = "successfully added friend";
                                        var friend_user = db.UserProfiles.SingleOrDefault(u => u.UserId == friend_user_id);
                                        var friend1 = new Friend { User = user, FriendUser = friend_user, Status = RequestStatus.Requested };
                                        var friend2 = new Friend { User = friend_user, FriendUser = user, Status = RequestStatus.Pending };
                                        db.Friends.Add(friend1);
                                        db.Friends.Add(friend2);

                                        Notification n1 = new Notification();
                                        n1.User = friend_user;
                                        n1.FriendID = friend2.FriendID;
                                        n1.Message = user.UserName + " Sent you a friend request";
                                        n1.Created = DateTime.Now;
                                        n1.Expires = n1.Created.AddDays(14);
                                        n1.Type = NotificationType.Friend;
                                        db.Notifications.Add(n1);

                                        db.SaveChanges();
                                        user.Friends.Add(friend1);
                                        friend_user.Friends.Add(friend2);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        ViewBag.message = "friend request already has been added";
                                        ViewBag.status = "error";
                                    }
                                }
                                else if (operation == "acceptfriend")
                                {
                                    ViewBag.operation = "acceptfriend";
                                    ViewBag.message = "successfully accepted friend";

                                    int friend_user_id = data.friend_user_id;

                                    var friendshipA = db.Friends.Include("User").Where(f => f.User.UserId == user.UserId && f.FriendUser.UserId == friend_user_id).FirstOrDefault();
                                    var friendshipB = db.Friends.Include("User").Where(f => f.User.UserId == friend_user_id && f.FriendUser.UserId == user.UserId).FirstOrDefault();
                                    friendshipA.Status = RequestStatus.Accepted;
                                    friendshipB.Status = RequestStatus.Accepted;
                                    friendshipA.User.FriendCount++;
                                    friendshipB.User.FriendCount++;

                                    Notification n1 = new Notification();
                                    n1.User = friendshipB.User;
                                    n1.FriendID = friendshipB.FriendID;
                                    n1.Message = friendshipB.FriendUser.UserName + " accepted your friend request";
                                    n1.Created = DateTime.Now;
                                    n1.Expires = n1.Created.AddDays(14);
                                    n1.Type = NotificationType.Friend;
                                    db.Notifications.Add(n1);

                                    db.SaveChanges();
                                    //send out notifications

                                }
                                else if (operation == "rejectfriend")
                                {
                                    ViewBag.operation = "rejectfriend";
                                    ViewBag.message = "successfully rejected friend";
                                    int friend_user_id = data.friend_user_id;
                                    var friendshipA = db.Friends.Include("User").Where(f => f.User.UserId == user.UserId && f.FriendUser.UserId == friend_user_id).FirstOrDefault();
                                    var friendshipB = db.Friends.Include("User").Where(f => f.User.UserId == friend_user_id && f.FriendUser.UserId == user.UserId).FirstOrDefault();
                                    friendshipA.Status = RequestStatus.Rejected;
                                    friendshipB.Status = RequestStatus.Rejected;

                                    Notification n1 = new Notification();
                                    n1.User = friendshipB.User;
                                    n1.FriendID = friendshipB.FriendID;
                                    n1.Message = friendshipB.FriendUser.UserName + " rejected your friend request";
                                    n1.Created = DateTime.Now;
                                    n1.Expires = n1.Created.AddDays(14);
                                    n1.Type = NotificationType.Friend;
                                    db.Notifications.Add(n1);

                                    db.SaveChanges();
                                    //send out notifications
                                }
                                else if (operation == "getnumber")
                                {
                                    getNumber(data);
                                }
                                else if (operation == "callbingo")
                                {
                                    ViewBag.operation = "callbingo";
                                    string gamecard = data.game_card;

                                    if (gamecard == user.GameCard.Hash())
                                    {
                                        string winningNumbers = data.winning_numbers;
                                        int game_id = data.game_id;
                                        var game = db.Games.Where(g => g.GameID == game_id).FirstOrDefault();
                                        if (game != user.Game)
                                        {
                                            ViewBag.status = "error";
                                            ViewBag.message = "user is not joined to that game";
                                        }
                                        bool win = game.VerifyWin(winningNumbers, gamecard);
                                        if (win)
                                        {
                                            //add to result
                                            //add to user win count
                                            Result r = new Result();
                                            r.Game = user.Game;
                                            r.Outcome = OutcomeType.Win;
                                            r.Points = 0;
                                            r.User = user;
                                            db.Results.Add(r);
                                            user.Results.Add(r);
                                            r.Game.Results.Add(r);
                                            user.WinCount++;
                                            db.SaveChanges();

                                            r.Game.WinCount++;
                                            if (r.Game.WinCount >= r.Game.WinLimit)
                                            {
                                                r.Game.Closed = true;
                                                //generate loss for each user still in game
                                            }

                                            ViewBag.status = "ok";
                                            ViewBag.message = "you won a game!";
                                        }
                                        else
                                        {
                                            ViewBag.status = "error";
                                            ViewBag.message = "invalid winning sequence";
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.status = "error";
                                        ViewBag.message = "submitted game card does not match user game card at server";
                                    }

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
                                    ViewBag.message = "successfully updated user";
                                }
                                else if (operation == "allfriends")
                                {
                                    ViewBag.operation = "allfriends";
                                    ViewBag.message = "successfully retrieved list of friends";
                                    //get all friendship requests
                                    var friends = db.Friends.Where(f => f.User.UserName == user.UserName).ToList<Friend>();
                                    ViewData["friends"] = friends;
                                }
                                else
                                {
                                    ViewBag.status = "error";
                                    ViewBag.operation = "unknown";
                                    ViewBag.message = "the operation you specified does not exist. Check your documentation.";
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
                if (operation == "allresults")
                {
                    AllResults();
                    return View();
                }
                else
                {
                    ViewBag.markdown = MarkdownForOperation(operation);
                    ViewBag.title = operation;
                    return View("Help");
                }
            }
        }


      

        void getNumber(dynamic data)
        {
            ViewBag.operation = "getnumber";
            ViewBag.message = "successfully retrieved number";
            int game_id = data.game_id;


            int user_id = data.user_id;
         
            var user = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);
            //var game = db.Games.SingleOrDefault(g => g.GameID == game_id);
            var game = db.Games.Include("Players").Where(g => g.GameID == game_id).FirstOrDefault();
            if (!game.Players.Contains(user))
            {
                ViewBag.Status = "error";
                ViewBag.message = "user not joined to game.";
            }
            else if (game.Closed)
            {
                if (game.NumbersIndex >= 25)
                {
                    ViewBag.message = "game closed, all numbers have been drawn";
                }
                else if (game.WinCount >= game.WinLimit)
                {
                    ViewBag.message = "game closed, win limit has been reached";
                }
                ViewBag.status = "error";
              
            }
            else
            {
                ViewBag.message = "successfully retrieved number";
                int number = game.GetNextNumber();
                ViewBag.nextnumbertime = game.NextNumberTime;
                if (number > -1)
                {
                    ViewBag.number = Game.BingofyNumber(number);
                }
                else
                {
                    game.Closed = true;
                    ViewBag.status = "error";
                    ViewBag.message = "all numbers have been drawn for this game";
                    ViewBag.number = -1;
                    //give a loss to all users who didn't win
                    foreach (UserProfile u in game.Players)
                    {
                        bool hadWin = false;
                        foreach (Result r in game.Results)
                        {
                            if (r.User == u && r.Outcome == OutcomeType.Win)
                            {
                                hadWin = true;
                            }
                        }
                        if (!hadWin)
                        {
                            Result loss = new Result();
                            loss.Game = game;
                            loss.User = u;
                            loss.Outcome = OutcomeType.Loss;
                            db.Results.Add(loss);
                            u.Results.Add(loss);
                            game.Results.Add(loss);
                        }
                    }
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
            if (user.Game != null)
            {
                if (game.Players != null && game.Players.Contains(user))
                {
                    game.UserCount--;
                    game.Players.Remove(user);
                    //is game empty then close it
                    Result r = new Result();
                    r.Game = user.Game;
                    r.Outcome = OutcomeType.Quit;
                    r.Points = 0;
                    r.User = user;
                    db.Results.Add(r);
                    user.Results.Add(r);
                    r.Game.Results.Add(r);
                
                    user.Game = null;
                    //db.SaveChanges();
                    ViewBag.message = "successfully quit game";
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.message = "user was not joined to the game";
                    ViewBag.Status = "error";
                }
            }
            else
            {
                ViewBag.message = "user was not joined to the game";
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

            if (game != null)
            {
                if (game.Closed)
                {
                    ViewBag.status = "error";
                    if (game.NumbersIndex >= 25)
                    {
                        ViewBag.message = "game closed, all numbers have been drawn";
                    }
                    else if (game.WinCount >= game.WinLimit)
                    {
                        ViewBag.message = "game closed, win limit has been reached";
                    }
                }
                else if (game.Players != null && game.Players.Contains(user))
                {
                    ViewBag.status = "error";
                    ViewBag.message = "user is already joined to game";
                }
                else if (game.Players != null && game.Players.Count == game.UserLimit)
                {
                    ViewBag.status = "error";
                    ViewBag.message = "game is full, try again shortly";
                }
                else
                {
                    ViewBag.message = "successfully joined game";

                    if (user.Game != null)
                    {
                        user.Game.UserCount--;
                        user.Game.Players.Remove(user);
                        Result r = new Result();
                        r.Game = user.Game;
                        r.Outcome = OutcomeType.Quit;
                        r.Points = 0;
                        r.User = user;
                        db.Results.Add(r);
                        user.Results.Add(r);
                        r.Game.Results.Add(r);
                       
                        user.Game = null;

                    }
                    game.UserCount++;
                    user.GameCount++;
                    Result join = new Result();
                    join.Outcome = OutcomeType.Join;
                    join.Game = game;
                    join.User = user;
                    db.Results.Add(join);
                    game.Results.Add(join);
                    user.Results.Add(join);

                    ViewData["game"] = game;
                    GameCard gamecard = game.GetNextGameCard();
                    if (gamecard == null)
                    {
                        ViewBag.message = "no gamecards left to play";
                        ViewBag.status = "error";
                    }
                    else
                    {
                        gamecard = db.GameCards.SingleOrDefault(gc => gc.GameCardID == gamecard.GameCardID);
                        ViewData["gamecard"] = gamecard;
                        user.GameCard = gamecard;
                        ViewBag.gamespeed = game.GameSpeed;
                        user.Game = game;
                        if (game.Players == null) game.Players = new List<UserProfile>();
                        game.Players.Add(user);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                ViewBag.message = "no game was found with that id";
                ViewBag.status = "error";
            }
        }

        void createGame(dynamic data)
        {
            ViewBag.operation = "creategame";
            ViewBag.message = "successfully created game";
            Game game = new Game();
            game.Name = data.name;
            game.Description = data.description;
            game.WinLimit = data.win_limit;
            game.UserLimit = data.user_limit;
            game.GameSpeed = data.game_speed;
            game.Created = DateTime.Now;
            game.Private = data["private"];
            game.NumbersIndex = 0;
            //game.Numbers = new List<int>();
            int user_id = data.user_id;
            game.Players = new List<UserProfile>();
            game.UserCount++;
            UserProfile createdByUser = db.UserProfiles.SingleOrDefault(u => u.UserId == user_id);

            
            if (createdByUser.Game != null)
            {
                //quit the user from any other game
                //create result that the user quit the game
                createdByUser.Game.Players.Remove(createdByUser);
                Result r = new Result();
                r.Game = createdByUser.Game;
                r.Outcome = OutcomeType.Quit;
                r.Points = 0;
                r.User = createdByUser;
                db.Results.Add(r);
                db.SaveChanges();
                createdByUser.Results.Add(r);
                r.Game.Results.Add(r);
                r.Game.UserCount--;

                createdByUser.Game = null;
            }
            createdByUser.GameCount++;
            createdByUser.Game = game;
            game.Players.Add(createdByUser);
            
            Result join = new Result();
            join.Outcome = OutcomeType.Join;
            join.Game = game;
            join.User = createdByUser;
            db.Results.Add(join);
            game.Results.Add(join);
            createdByUser.Results.Add(join);

            //generate unique gamecards
            game.GenerateGameCards();
            foreach (GameCard gc in game.GameCards)
            {
                db.GameCards.Add(gc);
            }

            string[] splitString = { "," };
            string temp = data.player_ids;
            if (temp != null)
            {
                string[] playerIDs = temp.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
                //loop through request users
                if (playerIDs != null)
                {
                    foreach (string player_id in playerIDs)
                    {
                        int tempUser_id = System.Convert.ToInt32(player_id);
                        var playerUser = db.UserProfiles.SingleOrDefault(u => u.UserId == tempUser_id);


                        //create an invite notification for each player                                        
                    }
                }
            }


            



            createdByUser.GameCard = game.GetNextGameCard();
          
          
        

            ViewData["gamecard"] = createdByUser.GameCard;
           
            //game.Results = new List<Result>();
            db.Games.Add(game);
            db.SaveChanges();
            List<Game> games = new List<Game>();
            games.Add(game);
            ViewData["games"] = games;
        }

        public void AllResults()
        {
            ViewBag.operation = "allresults";
            ViewData["results"] = db.Results.OrderByDescending(g => g.Created).Take(10);
            ViewBag.message = "successfully retrieved list of results";
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
