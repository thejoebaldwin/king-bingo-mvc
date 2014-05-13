using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingBingo;
using KingBingo.Models;

namespace Bot
{
    class Program
    {
        static KingBingo.Client client;
        static Mode _mode;
        //static string username = "test1";
        //static string password = "test1";
        static string username = "silvercooper";
        static string password = "test!234";
        //static string url =  "http://localhost:22986/service/v0";
        static string url = "http://itweb.fvtc.edu/kingbingo/service/v0";

        enum Mode
        {
            Auth,
            AllGames,
            JoinGame,
            CreateGame,
            PlayGame,
            Wait,
            QuitGame,
            CallBingo,
            AcceptFriend,
            RequestFriend,
            Done
        }


        static void Main(string[] args)
        {
            if (args.Length >= 3)
            {
                username = args[0];
                password = args[1];
                url = args[2];
            }
            _mode = Mode.Auth;
            Control();
        }


        public static void AuthenticationComplete()
        {
            _mode = Mode.AllGames;
            Control();
        }

   

        public static void CallBingoComplete()
        {
            _mode = Mode.PlayGame;
            if (client.Status == "ok")
            {
                client.Card.Clear();
            }
            Control();
        }

        public static void GetAllGamesComplete()
        {
           
                _mode = Mode.JoinGame;
                if (client.Games == null || client.Games.Count == 0)
                {
                    _mode = Mode.CreateGame;
                }
          
            Control();
        }

        public static void JoinGameComplete()
        {
            _mode = Mode.PlayGame;
            Control();
        }

        public static void CreateGameComplete()
        {
            _mode = Mode.JoinGame;
            if (client.User.Game == null )
            {
                _mode = Mode.CreateGame;
            }
            else
            {
                _mode = Mode.PlayGame;
            }
            Control();
        }

        public static void QuitGameComplete()
        {
            _mode = Mode.JoinGame;
            Control();
        }

        public static void AcceptFriendComplete()
        {
            _mode = Mode.PlayGame;
            Control();
        }

        public static void GetAllFriendsComplete()
        {
            if (client.Friends != null && client.Friends.Count > 0)
            {
                Friend temp = client.Friends.ElementAt(0);
                client.AcceptFriend(temp.FriendUser.UserId, AcceptFriendComplete);
            }
            else
            {
                _mode = Mode.PlayGame;
                Control();
            }
        }

        public static void AddFriendComplete()
        {
            _mode = Mode.PlayGame;
            Control();
        }

        public static void GetAllUsersComplete()
        {
            if (client.Users.Count > 0)
            {
                Random r = new Random();
                int num = r.Next(client.Users.Count);
                UserProfile temp = client.Users.ElementAt(num);
                client.AddFriend(temp.UserId, AddFriendComplete);
            }
            else
            {
                _mode = Mode.PlayGame;
                Control();
            }
        }

        public static void GetNumberComplete()
        {
            if (client.Status == "ok")
            {
                string bingo = client.Card.GetBingo();

                if (bingo == string.Empty)
                {
                    double delay = ((100 - client.GameSpeed) / 100.0) * 60.0;
                    System.Threading.Thread.Sleep((int) delay * 1000);
                }
                else
                {
                    _mode = Mode.CallBingo;
                }
            }
            else
            {
                Console.WriteLine("Game closed. Joining another");
                _mode = Mode.AllGames;
            }
            Control();
        }


        public static void Control()
        {
            
            if (client != null)
            {
                Console.WriteLine(client.Request);
                Console.WriteLine(client.Response);
            }
            string local = "http://localhost:22986/service/v0";
            string production = "http://itweb.fvtc.edu/kingbingo/service/v0";


            //GENERATE RANDOM
            //IF LESS THAN SOME NUMBER THEN DO THE FOLLOWING:
            //ACCEPT A FRIEND
            //OR
            //GET ALL USERS AND SEND A FRIEND REQUEST
            if (client != null && client.User != null)
            {
                Random r = new Random();
                int num = r.Next(50);
                if (num < 3)
                {
                    _mode = Mode.RequestFriend;
                }
                else if (num < 5)
                {
                    _mode = Mode.AcceptFriend;
                }
            }

            switch (_mode)
                {
                case Mode.Auth:
                      client = new Client(url);
                      client.Authenticate(username,password, AuthenticationComplete);
                      _mode = Mode.AllGames;
                      break;
                case Mode.AllGames:
                      client.GetAllGames(0, GetAllGamesComplete);
                      break;
                case Mode.JoinGame:
                      Game target = client.Games.ElementAt(0);
                      client.JoinGame(target.GameID, JoinGameComplete);
                      break;
                case Mode.CreateGame:
                      client.CreateGame(3, 10, 70, client.User.UserName + " Big Game", "Created by " + client.User.Name, null, false, CreateGameComplete);
                      break;
                case Mode.PlayGame:
                      client.GetNewNumber(GetNumberComplete);
                      break;
                case Mode.CallBingo:
                      string bingo = client.Card.GetBingo();
                      client.CallBingo(bingo, CallBingoComplete);
                      break;
                case Mode.QuitGame:
                      client.QuitGame(client.User.Game.GameID, QuitGameComplete);
                      break;
                case Mode.AcceptFriend:
                      client.GetAllFriends(0, RequestStatus.Pending, GetAllFriendsComplete);
                      break;
                case Mode.RequestFriend:
                      client.GetAllUsers(0, false, GetAllUsersComplete);
                      break;
                }

            Console.WriteLine("All Done");
            Console.ReadKey();
        }
    }
}
