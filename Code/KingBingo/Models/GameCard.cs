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

        public string Hash()
        {
            return string.Join(",", Numbers);
        }

    }
}