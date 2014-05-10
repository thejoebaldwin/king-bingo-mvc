using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Cryptography;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KingBingo.Models;

namespace KingBingo
{
    public class Client
    {

        public UserProfile User;
        public List<UserProfile> Users;
        public List<Friend> Friends;
        public List<Game> Games;
        public List<Notification> Notifications;
        public GameCard Card;

        public string Request;
        public string Response;
        public string Status;
        public int Number;
        public string Message;
        private int _GameID;
        private System.Net.HttpWebRequest _request;

        //public string[] GamecardNumbers;
        public bool[] _GameCard;
        
        Action _Completion;
        string _TargetURL;
        public int GameSpeed;
        

        public Client(string url)
        {
            _TargetURL = url;
            _GameCard = new bool[25];
        }



        private void EndResponse(IAsyncResult result)
        {
            //_request.EndGetResponse(result);
            Stream response = (result.AsyncState as System.Net.HttpWebRequest).EndGetResponse(result).GetResponseStream();
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            processResponse(response_json);
            _Completion();
        }
   

        private void PostDataWithOperation(string operation, string JSON)
        {
            //build request
 
            //System.Net.HttpWebRequest
            _request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/{1}", _TargetURL, operation));
            //set http method
            _request.Method = "POST";
            //content type
            _request.ContentType = "text/html";
            //build json
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(JSON);
            //write to request body
            Stream PostData = _request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = _request.GetResponse().GetResponseStream();

            Request = JSON;

            _request.BeginGetResponse(EndResponse, _request);


           

            //read response body
            //StreamReader response_reader = new StreamReader(response);
            //string response_json = response_reader.ReadToEnd();
            //processResponse(response_json);
            //return response_json;
        }

        static public string createAuthHash(string password)
        {
            string hash = SHA1(password);
            hash = hash + DateTime.Today.ToString("yyyy-MM-dd"); //YYYY-MM-dd
            hash = SHA1(hash);
            return hash;
        }


        static private string SHA1(string cleartext)
        {
            byte[] p2 = System.Text.Encoding.Unicode.GetBytes(cleartext);
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(p2);
            string encodedText = Convert.ToBase64String(result);
            return encodedText;
        }

        public void Authenticate(string username, string password, Action completion)
        {
            _Completion = completion;
            string hash = createAuthHash(password);
            string post_json = "{\"username\":\"" + username + "\", \"hash\":\"" + hash + "\"}";
            PostDataWithOperation("auth", post_json);

        }

        public void AddFriend(int friend_user_id, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("friend_user_id", friend_user_id.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("addfriend", post_json);
           
        }

        public void AcceptFriend(int friend_user_id, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("friend_user_id", friend_user_id.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("acceptfriend", post_json);
           
        }

        public void RejectFriend(int friend_user_id, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("friend_user_id", friend_user_id.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("rejectfriend", post_json);
          
        }

        public void GetNewNumber(Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("game_id", _GameID.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("getnumber", post_json);
        }

        private string ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds).ToString();
        }


        public void UpdateUser(string name, string bio, string email, string zip, Models.Sex sex, DateTime birthdate, string profileImage, double latitude, double longitude, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> u = new Dictionary<string, string>();
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            u.Add("name", name);
            u.Add("bio", bio);
            u.Add("email", email);
            u.Add("zip", zip);
            u.Add("sex", sex.ToString());
            u.Add("birthdate", ToUnixTime(birthdate));
            //if (profileImage != null)
            //{
            u.Add("profile_image", profileImage);
            //}
            //add a placeholder to keep serializer from wrapping user json in quotes
            d.Add("user", "@@@");
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            string userJson = Newtonsoft.Json.JsonConvert.SerializeObject(u);
            //swap out placeholder with correct user json
            post_json = post_json.Replace("\"@@@\"", userJson);
            PostDataWithOperation("updateuser", post_json);
        }

        public void CreateGame(int winLimit, int userLimit, int gameSpeed, string name, string description, string playerIDs, bool privateGame, Action completion)
        {

            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("win_limit", winLimit.ToString());
            d.Add("user_limit", userLimit.ToString());
            d.Add("game_speed", gameSpeed.ToString());
            d.Add("name", name);
            d.Add("description", description);
            d.Add("player_ids", playerIDs);
            d.Add("private", privateGame.ToString());


            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("creategame", post_json);
        

        }

        public void Register(string username, string password, string email, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("username", username);
            d.Add("password", password);
            d.Add("email", email);
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("createuser", post_json);
        
        }

        public void GetUser(int query_user_id, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("query_user_id", User.AuthenticationToken);

            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("getuser", post_json);
          
        }

        public void JoinGame(int gameID, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("game_id", gameID.ToString());
            _GameID = gameID;
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("joingame", post_json);
           
        }


        public void CallBingo(string winningNumbers, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("winning_numbers", winningNumbers);
            d.Add("game_card", Card.ToString());
            d.Add("game_id", _GameID.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("callbingo", post_json);
        }

        public void QuitGame(int gameID, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("game_id", gameID.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("quitgame", post_json);
          
        }

        public void GetAllNotifications(int page, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("allnotifications", post_json);
        
        }

        public void GetAllGames(int page, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("page", page.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("allgames", post_json);
         
        }

        public void GetAllUsers(int page, bool includeProfileImages, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("page", page.ToString());
            d.Add("include_profile_images", includeProfileImages.ToString());

            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
          PostDataWithOperation("allusers", post_json);
          

        }

        public void GetAllFriends(int page, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("page", page.ToString());

            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            PostDataWithOperation("allfriends", post_json);
     

        }

        private int UnBingofyNumber(string bingoNumber)
        {
            int number = -1;
            bingoNumber = bingoNumber.Substring(1);
            number = System.Convert.ToInt32(bingoNumber);
            return number;
        }

        private void processResponse(string response_json)
        {
      
            try
            {
                 Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                 response_json,
                 new JsonSerializerSettings
                 {
                       TypeNameHandling = TypeNameHandling.All,
                       TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                 }
                );

                Message = (string) data["message"];
                Status = (string)data["status"];
                if ((string)data["status"] == "ok")
                {
                    if ((string)data["operation"] == "auth")
                    {
                        JObject user = (JObject)data["user"];
                        Dictionary<string, string> userDictionary = user.ToObject<Dictionary<string, string>>();
                        User = UserProfile.FromData(userDictionary, true, true);
                        Games = new List<Game>();
                        if (data.ContainsKey("games"))
                        {
                            JArray games = (JArray)data["games"];
                            foreach (JObject g in games)
                            {
                                Game game = Game.FromData(g.ToObject<Dictionary<string, string>>());
                                Games.Add(game);
                            }
                        }
                        if (data.ContainsKey("game_card"))
                        {
                                   var gamecard = data["game_card"];
                        GameSpeed = System.Convert.ToInt32(data["game_speed"]);
                        Card = new GameCard((string)data["game_card"]);
                        }
                        /*
                 
                        */
                    }
                    else if ((string)data["operation"] == "allusers")
                    {
                        Users = new List<UserProfile>();
                        JArray users = (JArray)data["users"];
                        foreach (JObject u in users)
                        {
                            UserProfile user = UserProfile.FromData(u.ToObject<Dictionary<string, string>>(), false, false);
                            Users.Add(user);
                        }
                    }
                    else if ((string)data["operation"] == "allnotifications")
                    {
                        Notifications = new List<Notification>();
                        JArray notifications = (JArray)data["notifications"];
                        foreach (JObject n in notifications)
                        {
                            Notification notification = Notification.FromData(n.ToObject<Dictionary<string, string>>());
                            Notifications.Add(notification);
                        }
                    }
                    else if ((string)data["operation"] == "allgames")
                    {
                        Games = new List<Game>();
                        JArray games = (JArray)data["games"];
                        foreach (JObject g in games)
                        {
                            Game game = Game.FromData(g.ToObject<Dictionary<string, string>>());
                            Games.Add(game);
                        }

                    }
                    else if ((string)data["operation"] == "creategame")
                    {
                        Games = new List<Game>();
                        JArray games = (JArray)data["games"];
                        Game game = Game.FromData(games[0].ToObject<Dictionary<string, string>>());
                        User.Game = game;
                        Card = new GameCard((string)data["game_card"]);
                    }

                    else if ((string)data["operation"] == "allfriends")
                    {
                        var temp = data["friends"];
                        Friends = new List<Friend>();
                        JArray friends = (JArray)data["friends"];
                        foreach (JObject f in friends)
                        {
                            Friend friend = Friend.FromData(f.ToObject<Dictionary<string, string>>());
                            Friends.Add(friend);
                        }
                    }
                    else if ((string)data["operation"] == "addfriend" || (string)data["operation"] == "acceptfriend" || (string)data["operation"] == "rejectfriend")
                    {
                        //should refresh all friends data here
                    }
                    else if ((string)data["operation"] == "joingame")
                    {
                        var gamecard = data["game_card"];
                        GameSpeed = System.Convert.ToInt32(data["game_speed"]);
                        Card = new GameCard((string)data["game_card"]);
                        //GamecardNumbers = ((string)gamecard).Split(',');
                    }
                    else if ((string)data["operation"] == "quitgame")
                    {
                        Card = null;
                        GameSpeed = -1;
                    }
                    else if ((string)data["operation"] == "getprofileimage")
                    {
                        string profileimage = (string)data["profile_image"];
                        byte[] buffer = Convert.FromBase64String(profileimage);
                        MemoryStream ms = new MemoryStream(buffer);
                        User.ProfileImage = buffer;
                    }
                    else if ((string)data["operation"] == "callbingo")
                    {
                        //?
                    }
                    else if ((string)data["operation"] == "getnumber")
                    {
                        string bingoNumber = (string)data["number"];
                        Number = -1;
                        if (bingoNumber != "-1")
                        {
                            Number = UnBingofyNumber(bingoNumber);
                            Card.PlayNumber(Number);
                        }
                    }
                }
                else
                {

                }
                Response = response_json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }



    }
}
