using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace KingBingo.Models
{
    public class Game
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WinLimit { get; set; }
        public int UserLimit { get; set; }
        public int GameSpeed { get; set; }
        public DateTime Created { get; set; }
        public bool Private { get; set; }
        public int NumbersIndex { get; set; }
        public DateTime NextNumberTime { get; set; }
        public bool Closed { get; set; }
        public int WinCount { get; set; }
        public int UserCount { get; set; }


        public string InternalData { get; set; }

        public ICollection<int> Numbers
        {
            get
            {
                return Array.ConvertAll(InternalData.Split(','), Int32.Parse);
            }
            set
            {
                InternalData = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }

        public int[] DrawnNumbers()
        {
           //CRASHING HERE
           int[] arr = Array.ConvertAll(InternalData.Split(','), Int32.Parse);
           Array.Copy(arr, arr, NumbersIndex);
           return arr;
        }

        public ICollection<UserProfile> Players { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<GameCard> GameCards { get; set; }

     
    

        public Game()
        {
            //randomize order in which numbers will be drawn
            int[] numbers = new int[70];
            int[] num_ = { 1, 2, 3, 4, 5, 6, 7, 8, 8, 10, 11, 12, 13, 14, 15,
                            16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                            31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45,
                            46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60,
                            61, 62, 63, 64, 65, 66, 67, 68, 69, 70
                         };
            ArrayList num = new ArrayList(num_);
            Random r = new Random();
            for (int i = 0; i < 70; i++)
            {
                int numberIndex = r.Next(num.Count);
                int number = (int)num[numberIndex];
                num.RemoveAt(numberIndex);
                numbers[i] = number;
            }
            Numbers = numbers;
            NumbersIndex = -1;
            Closed = false;
            NextNumberTime = DateTime.Now;
            WinCount = 0;
            UserCount = 0;
        }

        private static DateTime FromUnixTime(string timestamp)
        {
            // Unix timestamp is seconds past epoch
            double timestampSeconds = Convert.ToDouble(timestamp);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestampSeconds).ToLocalTime();
            return dtDateTime;
        }

        private string ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds).ToString();
        }


        public dynamic ToData()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("game_id", this.GameID.ToString());
            dict.Add("name", this.Name);
            dict.Add("description", this.Description);
            dict.Add("win_limit", this.WinLimit.ToString());
            dict.Add("user_limit", this.UserLimit.ToString());
            dict.Add("game_speed", this.GameSpeed.ToString());
            dict.Add("created", ToUnixTime(this.Created));
            dict.Add("private", this.Private.ToString());
            dict.Add("closed", this.Closed.ToString());
            dict.Add("win_count", this.WinCount.ToString());
            dict.Add("user_count", this.UserCount.ToString());
            string players = string.Empty;
            if (this.Players != null)
            {
                foreach (var user in this.Players)
                {
                    if (players != string.Empty)
                    {
                        players += ",";
                    }
                    players += user.UserId;
                }
            }
            dict.Add("players",players);
            return dict;
        }

        public static Game FromData(dynamic data)
        {
            Game game = new Game();
            if (data.numbers != null)
            {
                string numbers = data.numbers;
                game.Numbers = numbers.Split(',').Select(x => int.Parse(x)).ToArray();
            }
            game.Name = data.name;
            game.Description = data.description;
            game.Created = FromUnixTime((string) data.created);
            game.WinCount = data.win_count;
            game.UserCount = data.user_count;
            return game;
        }

        public void GenerateGameCards()
        {
            int count = 0;
            Hashtable hash = new Hashtable();
            int max_cards = UserLimit * 2;
            if (max_cards < 20) max_cards = 20;
            while (count < UserLimit * 2)
            {
                GameCard gc = GameCard.GenerateGameCard();
                if (!hash.ContainsKey(gc.Hash()))
                {
                    hash.Add(gc.Hash(), gc);
                    count++;
                }
            }
            List<GameCard> gamecards = new List<GameCard>();
            foreach (DictionaryEntry pair in hash)
            {
                gamecards.Add((GameCard)pair.Value);
            }
            GameCards = gamecards;
        }


        public bool VerifyWin(string winningNumbers, string gamecard)
        {
            string[] splitString = { "," };
            string[] userNumbers = winningNumbers.Split(splitString, StringSplitOptions.None);
            string[] userGameCard = gamecard.Split(splitString, StringSplitOptions.None);
            bool win = true;
            int[] drawnNumbers = (int[]) DrawnNumbers();
            //first check that these numbers were drawn
            foreach (string number in userNumbers)
            {
                int n = System.Convert.ToInt32(number);
                if (!drawnNumbers.Contains(n))
                {
                    win = false;
                    break;
                }
                else
                {
                    for (int i = 0; i < userGameCard.Length; i++)
                    {
                        if (userGameCard[i] == number)
                        {
                            userGameCard[i] = "x";
                        }
                    }
                }
            }
            //if all the claimed winning numbers have been drawn, check for each winning scenario
            if (win)
            {
                win = false;
                for (int row = 0; row < 5; row++)
                {
                    //check all rows
                    if (userGameCard[0 + row] == "x" &&
                       userGameCard[5 + row] == "x" &&
                       userGameCard[10 + row] == "x" &&
                       userGameCard[15 + row] == "x" &&
                       userGameCard[20 + row] == "x")
                    {
                        win = true;
                        break;
                    }
                }
                if (!win)
                {
                    //check all columns
                    for (int column = 0; column < 5; column++)
                    {
                        int offsetColumn = column * 5;
                        if (
                          userGameCard[0 + offsetColumn] == "x" &&
                          userGameCard[1 + offsetColumn] == "x" &&
                          userGameCard[2 + offsetColumn] == "x" &&
                          userGameCard[3 + offsetColumn] == "x" &&
                          userGameCard[4 + offsetColumn] == "x")
                        {
                            win = true;
                            break;
                        }
                    }
                }
                if (!win)
                {
                    //backwards diagonal
                    if (
                        userGameCard[0] == "x" &&
                        userGameCard[6] == "x" &&
                        userGameCard[12] == "x" &&
                        userGameCard[18] == "x" &&
                        userGameCard[24] == "x")
                    {
                        win = true;
                    }
                }
                if (!win)
                {
                    //forwards diagonal
                    if (
                        userGameCard[4] == "x" &&
                        userGameCard[8] == "x" &&
                        userGameCard[12] == "x" &&
                        userGameCard[16] == "x" &&
                        userGameCard[20] == "x")
                    {
                        win = true;
                    }
                }
            }
            return win;
        }

        public static string BingofyNumber(int number)
        {
            string prefix = "";
            if (number <= 15)
            {
                prefix = "B";
            }
            else if (number <= 30)
            {
                prefix = "I";
            }
            else if (number <= 45)
            {
                prefix = "N";
            }
            else if (number <= 60)
            {
                prefix = "G";
            }
            else if (number <= 70)
            {
                prefix = "O";
            }
            return prefix + number.ToString();
        }


        public GameCard GetNextGameCard()
        {
            GameCard gc = null;
            if (GameCards.Count > 0)
            {
               gc = GameCards.Last();
                GameCards.Remove(gc);
                return gc;
            }
            return gc;
        }

        public int GetNextNumber()
        {
            int number = -1;
            if (NumbersIndex < 70)
            {
                if (DateTime.Now > NextNumberTime)
                {
                    NumbersIndex++;
                    double delay = 100 - GameSpeed;
                    NextNumberTime = DateTime.Now.AddSeconds(60 * (delay/100.0));
                }
                if (NumbersIndex < 70)
                {
                    number = Numbers.ElementAt(NumbersIndex);
                }
            }
            return number;
        }
       
    }


}