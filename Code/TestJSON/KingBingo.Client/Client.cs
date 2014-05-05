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
using KingBingo.Models;

namespace KingBingo
{
    public class Client
    {

        UserProfile User;
        List<UserProfile> Users;
        List<Friend> Friends;
        List<Game> Games;
        GameCard numbers;
        public string[] gamecardNumbers;
        Action _Completion;
        string _TargetURL;

        public Client(string url)
        {
            _TargetURL = url;
        }


        private string PostDataWithOperation(string operation, string JSON)
        {
            //build request
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/{1}", _TargetURL, operation));
            //set http method
            request.Method = "POST";
            //content type
            request.ContentType = "text/html";
            //build json
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(JSON);
            //write to request body
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = request.GetResponse().GetResponseStream();
            //read response body
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            processResponse(response_json);
            return response_json;
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
            string response_json = PostDataWithOperation("auth", post_json);
            processResponse(response_json);
        }

        public void GetAllGames(int page, Action completion)
        {
            _Completion = completion;
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("user_id", User.UserId.ToString());
            d.Add("authentication_token", User.AuthenticationToken);
            d.Add("page", page.ToString());
            string post_json = Newtonsoft.Json.JsonConvert.SerializeObject(d);
            string response =  PostDataWithOperation("allgames", post_json);
            processResponse(response);
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
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response_json);

                if (data.operation == "auth")
                {
                    //var user = data.user;
                    User = UserProfile.FromData(data.user);
                }
                else if (data.operation == "allusers")
                {
                    List<UserProfile> users = new List<UserProfile>();
                    foreach (dynamic u in data.users)
                    {
                        UserProfile user = UserProfile.FromData(u);
                        users.Add(user);
                    }
                }
                else if (data.operation == "allgames")
                {
                    List<Game> games = new List<Game>();
                    foreach (dynamic g in data.games)
                    {
                        Game game = Game.FromData(g);
                        games.Add(game);
                    }
                }

                else if (data.operation == "allfriends")
                {
                    var temp = data.friends;
                    List<Friend> friends = new List<Friend>();
                    foreach (dynamic d in temp)
                    {
                        var friend = Friend.FromData(d);
                        friends.Add(friend);
                    }
 
                }
                else if (data.operation == "joingame")
                {
                    var gamecard = data.game_card;
                    var gamespeed = data.game_speed;
                    string[] splitString = { "," };
                    gamecardNumbers = ((string)gamecard).Split(splitString, StringSplitOptions.None);
                }
                else if (data.operation == "getprofileimage")
                {
                    string profileimage = data.profile_image;
                    byte[] buffer = Convert.FromBase64String(profileimage);
                    MemoryStream ms = new MemoryStream(buffer);
                    User.ProfileImage = buffer;
                }
                else if (data.operation == "getnumber")
                {
                    string bingoNumber = data.number;
                    if (bingoNumber != "-1")
                    {
                        int number = UnBingofyNumber(bingoNumber);
                      
                    }
                    else
                    {
                    
                    }
                  

                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            _Completion();
        }



    }
}
