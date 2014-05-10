using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;

namespace KingBingo.Models
{
    public class GameCard
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GameCardID { get; set; }
        public virtual bool[] Played { get; set; }
        public string InternalData { get; set; }

        public List<int> Numbers
        {
            get
            {
                if (InternalData != null && InternalData != string.Empty)
                {
                    int[] temp = Array.ConvertAll(InternalData.Split(','), Int32.Parse);

                    List<int> numbers = new List<int>();
                    for (int i = 0; i < 25; i++)
                    {
                        numbers.Add(temp[i]);
                    }
                    return numbers;
                }
                else
                {
                    return new List<int>();
                }
               
            }
            set
            {
                InternalData = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }


        public GameCard()
        {
            Played = new bool[25];
            Numbers = new List<int>();
        }

        public GameCard(string gamecard)
        {
            Played = new bool[25];
            string[] temp = gamecard.Split(',');
            List<int> numbers = new List<int>();
            for (int i = 0; i < 25; i++)
            {
                numbers.Add(System.Convert.ToInt32(temp[i]));
            }
            Numbers = numbers;
        }


        static public GameCard GenerateGameCard()
        {
            //int[] numbers = new int[25];
            List<int> numbers = new List<int>();
            

            int[] b_ = { 1, 2, 3, 4, 5, 6, 7, 8, 8, 10, 11, 12, 13, 14, 15 };
            int[] i_ = { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            int[] n_ = { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45 };
            int[] g_ = { 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60 };
            int[] o_ = { 61, 62, 63, 64, 65, 66, 67, 68, 69, 70 };

            //create arraylists
            ArrayList b = new ArrayList(b_);
            ArrayList i = new ArrayList(i_);
            ArrayList n = new ArrayList(n_);
            ArrayList g = new ArrayList(g_);
            ArrayList o = new ArrayList(o_);
            Random r = new Random();
            for (int index = 0; index < 25; index++)
            {
                if (index < 5)
                {
                    int numberIndex = r.Next(b.Count);
                    int number = (int) b[numberIndex];
                    b.RemoveAt(numberIndex);
                    numbers.Add(number);
                }
                else if (index < 10)
                {
                    int numberIndex = r.Next(i.Count);
                    int number = (int)i[numberIndex];
                    i.RemoveAt(numberIndex);
                    numbers.Add(number);
                }
                else if (index < 15)
                {
                    int numberIndex = r.Next(n.Count);
                    int number = (int)n[numberIndex];
                    n.RemoveAt(numberIndex);
                    numbers.Add(number);
                }
                else if (index < 20)
                {
                    int numberIndex = r.Next(g.Count);
                    int number = (int)g[numberIndex];
                    g.RemoveAt(numberIndex);
                    numbers.Add(number);
                }
                else if (index < 25)
                {
                    int numberIndex = r.Next(o.Count);
                    int number = (int)o[numberIndex];
                    o.RemoveAt(numberIndex);
                    numbers.Add(number);
                }
            }
            GameCard gc = new GameCard();
            gc.Numbers = numbers;
            return gc;
        }



        public dynamic ToData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            return data;
        }

        

        public GameCard FromData(dynamic data)
        {
            GameCard gamecard = new GameCard();
            return gamecard;
        }

        public string ToString()
        {
            return string.Join(",", Numbers);
        }

        public string Hash()
        {
            return string.Join(",", Numbers);
        }

        public bool PlayNumber(int number)
        {
            bool found = false;
            List<int> numbers = (List<int>) this.Numbers;
            for (int i = 0; i < 25; i++)
            {
                if (numbers.ElementAt(i) == number)
                {
                    found = true;
                    Played[i] = true;
                    break;
                }
            }
            return found;
        }

        public void Clear()
        {
            Played = new bool[25];
        }

        string RowWin(int row)
        {
            string bingo = string.Empty;

            if (Played[0 + row] &&
                Played[5 + row] &&
                Played[10 + row] &&
                Played[15 + row] &&
                Played[20 + row])
            {
                List<int> numbers = (List<int>) this.Numbers;
                bingo = numbers[0 + row].ToString();
                bingo += "," + numbers[5 + row].ToString();
                bingo += "," + numbers[10 + row].ToString();
                bingo += "," + numbers[15 + row].ToString();
                bingo += "," + numbers[20 + row].ToString();
            }
            return bingo;
        }

        string ColumnWin(int column)
        {
            string bingo = string.Empty;
            column = column * 5;
            if (Played[0 + column] &&
                Played[1 + column] &&
                Played[2 + column] &&
                Played[3 + column] &&
                Played[4 + column])
            {
                List<int> numbers = (List<int>)this.Numbers;
                bingo = numbers[0 + column].ToString();
                bingo += "," + numbers[1 + column].ToString();
                bingo += "," + numbers[2 + column].ToString();
                bingo += "," + numbers[3 + column].ToString();
                bingo += "," + numbers[4 + column].ToString();
            }
            return bingo;
        }

        public string GetBingo()
        {
            //check columns
            //check rows
            //check diagonals
            string bingo = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                bingo = RowWin(i);
                if (bingo != string.Empty) break;
            }
            if (bingo == string.Empty)
            {
                for (int i = 0; i < 5; i++)
                {
                    bingo = ColumnWin(i);
                    if (bingo != string.Empty) break;
                }
            }
            if (bingo == string.Empty)
            {
                bingo = ForwardDiagonalWin();
            }
            if (bingo == string.Empty)
            {
                bingo = BackwardDiagonalWin();
            }
            return bingo;
        }

        string ForwardDiagonalWin()
        {
            //0, 6, 12, 18, 24
            string bingo = string.Empty;
            if (Played[0] &&
                Played[6] &&
                Played[12] &&
                Played[18] &&
                Played[24])
            {
                List<int> numbers = (List<int>)this.Numbers;
                bingo = numbers[0].ToString();
                bingo += "," + numbers[6].ToString();
                bingo += "," + numbers[12].ToString();
                bingo += "," + numbers[18].ToString();
                bingo += "," + numbers[24].ToString();
            }
            return bingo;
        }

        string BackwardDiagonalWin()
        {
            string bingo = string.Empty;
            if (Played[4] &&
                Played[8] &&
                Played[12] &&
                Played[16] &&
                Played[20])
            {
                List<int> numbers = (List<int>)this.Numbers;
                bingo = numbers[4].ToString();
                bingo += "," + numbers[8].ToString();
                bingo += "," + numbers[12].ToString();
                bingo += "," + numbers[16].ToString();
                bingo += "," + numbers[20].ToString();
            }
            return bingo;
        }



    }
}