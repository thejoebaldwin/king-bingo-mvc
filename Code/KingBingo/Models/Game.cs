﻿using System;
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
        public int Speed { get; set; }
        public DateTime Created { get; set; }
        public bool Private { get; set; }
        public int NumbersIndex { get; set; }
        public DateTime NextNumberTime { get; set; }


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


        public ICollection<UserProfile> Players { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<GameCard> GameCards { get; set; }

        public int WinCount()
        {
            return 0;
        }
        public int UserCount()
        {
            if (Players == null)
            {
                return 0;
            }
            else
            {
                return Players.Count;
            }
        }

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
            NextNumberTime = DateTime.Now;
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
                    double delay = 100 - Speed;
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