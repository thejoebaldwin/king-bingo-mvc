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
        public int Number;
        public string Message;
        private int _GameID;
        private System.Net.HttpWebRequest _request;

        public string[] GamecardNumbers;
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
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("winning_numbers", winningNumbers);
            d.Add("game_card", string.Join(",", GamecardNumbers));
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

        string RowWin(int row)
        {
            string bingo = string.Empty;

            if (_GameCard[0 + row] &&
                _GameCard[5 + row] &&
                _GameCard[10 + row] &&
                _GameCard[15 + row] &&
                _GameCard[20 + row])
            {
                bingo = GamecardNumbers[0 + row].ToString();
                bingo += "," + GamecardNumbers[5 + row].ToString();
                bingo += "," + GamecardNumbers[10 + row].ToString();
                bingo += "," + GamecardNumbers[15 + row].ToString();
                bingo += "," + GamecardNumbers[20 + row].ToString();
            }
            return bingo;
        }

        string ColumnWin(int column)
        {
            string bingo = string.Empty;
            column = column * 5;
            if (_GameCard[0 + column]&&
                _GameCard[1 + column] &&
                _GameCard[2 + column]  &&
                _GameCard[3 + column] &&
                _GameCard[4 + column])
            {
                bingo = GamecardNumbers[0 + column].ToString();
                bingo += "," + GamecardNumbers[1 + column].ToString();
                bingo += "," + GamecardNumbers[2 + column].ToString();
                bingo += "," + GamecardNumbers[3 + column].ToString();
                bingo += "," + GamecardNumbers[4 + column].ToString();
            }
            return bingo;
        }

        string forwardDiagonalWin()
        {
            //0, 6, 12, 18, 24
            string bingo = string.Empty;
            if (_GameCard[0] &&
                _GameCard[6] &&
                _GameCard[12] &&
                _GameCard[18] &&
                _GameCard[24])
            {
                bingo = GamecardNumbers[0].ToString();
                bingo += "," + GamecardNumbers[6].ToString();
                bingo += "," + GamecardNumbers[12].ToString();
                bingo += "," + GamecardNumbers[18].ToString();
                bingo += "," + GamecardNumbers[24].ToString();
            }
            return bingo;

        }
        string backwardDiagonalWin()
        {
            string bingo = string.Empty;
            if (_GameCard[4] &&
                _GameCard[8] &&
                _GameCard[12] &&
                _GameCard[16] &&
                _GameCard[20])
            {
                bingo = GamecardNumbers[4].ToString();
                bingo += "," + GamecardNumbers[8].ToString();
                bingo += "," + GamecardNumbers[12].ToString();
                bingo += "," + GamecardNumbers[16].ToString();
                bingo += "," + GamecardNumbers[20].ToString();
             }
            return bingo;
        }

        private void processResponse(string response_json)
        {
      
            try
            {
              //   Dictionary<string, string> data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(response_json);
                 Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                 response_json,
                 new JsonSerializerSettings
                 {
                       TypeNameHandling = TypeNameHandling.All,
                       TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
                 }
                );

               

                //System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                //Dictionary<string, string> data = js.Deserialize<Dictionary<string, string>>(response_json);   

                Message = (string) data["message"];
                if ((string) data["operation"] == "auth")
                {
                    JObject user =    (JObject) data["user"];
                    Dictionary<string, string> userDictionary = user.ToObject<Dictionary<string, string>>();
                    User = UserProfile.FromData(userDictionary, true);
                }
                else if ((string) data["operation"] == "allusers")
                {
                   Users = new List<UserProfile>();
                   JArray users = (JArray) data["users"];
                   foreach (JObject u in users)
                   {
                       Dictionary<string, string> userDictionary = u.ToObject<Dictionary<string, string>>();
                       UserProfile user = UserProfile.FromData(userDictionary, false);
                       Users.Add(user);
                   }
                }
                else if ((string) data["operation"] == "allnotifications")
                {
                    Notifications = new List<Notification>();
                    /*
                    foreach (dynamic n in data["notifications"])
                    {
                        Notification notification = Notification.FromData(n);
                        Notifications.Add(notification);
                    }*/
                }
                else if ((string) data["operation"] == "allgames")
                {
                    Games = new List<Game>();
                    /*
                    foreach (dynamic g in data["games"])
                    {
                        Game game = Game.FromData(g);
                        Games.Add(game);
                    }
                     */
                }

                else if ((string) data["operation"] == "allfriends")
                {
                    var temp = data["friends"];
                    Friends = new List<Friend>();
                     /*
                    foreach (dynamic d in temp)
                    {
                        var friend = Friend.FromData(d);
                        Friends.Add(friend);
                    }
                     * */

                }
                else if ((string)data["operation"] == "addfriend" || (string)data["operation"] == "acceptfriend" || (string) data["operation"] == "rejectfriend")
                {
                    //should refresh all friends data here
                }
                else if ((string) data["operation"] == "joingame")
                {
                    var gamecard = data["game_card"];
                    GameSpeed = System.Convert.ToInt32(data["game_speed"]);
                    string[] splitString = { "," };
                    GamecardNumbers = ((string)gamecard).Split(',');
                }
                else if ((string) data["operation"] == "quitgame")
                {
                    GamecardNumbers = null;
                    GameSpeed = -1;
                }
                else if ((string) data["operation"] == "getprofileimage")
                {
                    string profileimage = (string) data["profile_image"];
                    byte[] buffer = Convert.FromBase64String(profileimage);
                    MemoryStream ms = new MemoryStream(buffer);
                    User.ProfileImage = buffer;
                }
                else if ((string) data["operation"] == "callbingo")
                {
                   //?
                }
                else if ((string) data["operation"] == "getnumber")
                {
                    string bingoNumber = (string) data["number"];
                    Number = -1;
                    if (bingoNumber != "-1")
                    {
                        Number = UnBingofyNumber(bingoNumber);
                        for (int i = 0; i < GamecardNumbers.Length; i++ )
                        {
                            if (Number.ToString() == GamecardNumbers[i])
                            {
                                _GameCard[i] = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }



    }
}
