using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KingBingo.DAL;

namespace KingBingo.Models
{
   

    public enum OutcomeType
    {
        Join, Quit, Loss, Win
    }

    public class Result
    {

        private KingBingoContext db = new KingBingoContext();

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResultID { get; set; }
        public OutcomeType? Outcome { get;  set; }
        public DateTime Created { get; set; }
        public int Points { get; set; }
        public int UserId { get; set; }
        public int GameID { get; set; }
        [ForeignKey("UserId")] 
        public virtual UserProfile User { get; set; }
        [ForeignKey("GameID")] 
        public virtual Game Game { get; set; }

        public Result()
        {
            Created = DateTime.Now;
            UserId = -1;
            GameID = -1;
        }


        private string ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds).ToString();
        }


        public string ToString()
        {
            string message = string.Empty;
            if (UserId > -1) Game = db.Games.Where(g => g.GameID == this.GameID).FirstOrDefault();
            if (GameID > -1) User = db.UserProfiles.Where(u => u.UserId == this.UserId).FirstOrDefault();

            if (this.Outcome == KingBingo.Models.OutcomeType.Win)
            {
                message = String.Format("{0} won in {1}", User.UserName, Game.Name);
            }
            else if (this.Outcome == KingBingo.Models.OutcomeType.Loss)
            {
                message = String.Format("{0} lost in {1}", User.UserName, Game.Name);
            }
            else if (this.Outcome == KingBingo.Models.OutcomeType.Quit)
            {
                message = String.Format("{0} quit {1}", User.UserName, Game.Name);
            }
            else if (this.Outcome == KingBingo.Models.OutcomeType.Join)
            {
                message = String.Format("{0} joined {1}", User.UserName, Game.Name);
            }
            return message;
        }

        public string ToString(string format)
        {
            string message = string.Empty;
            if (format == "html")
            {
                if (UserId > -1) Game = db.Games.Where(g => g.GameID == this.GameID).FirstOrDefault();
                if (GameID > -1) User = db.UserProfiles.Where(u => u.UserId == this.UserId).FirstOrDefault();

                if (this.Outcome == KingBingo.Models.OutcomeType.Win)
                {
                    message = String.Format("{0} won in {1}", User.UserName, Game.Name);
                }
                else if (this.Outcome == KingBingo.Models.OutcomeType.Loss)
                {
                    message = String.Format("{0} lost in {1}", User.UserName, Game.Name);
                }
                else if (this.Outcome == KingBingo.Models.OutcomeType.Quit)
                {
                    message = String.Format("{0} quit {1}", User.UserName, Game.Name);
                }
                else if (this.Outcome == KingBingo.Models.OutcomeType.Join)
                {
                    message = String.Format("{0} joined {1}", User.UserName, Game.Name);
                }
            }
            return message;
        }

        public Dictionary<string, string> ToData()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("message", this.ToString());
            dict.Add("result_id", ResultID.ToString());
            dict.Add("game_id", GameID.ToString());
            dict.Add("user_id", UserId.ToString());
            dict.Add("created", ToUnixTime(Created));
            dict.Add("type", Outcome.ToString());
            return dict;
        }

    }
}