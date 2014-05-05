using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{

    public enum NotificationMethodType
    {
        Email, Push, Web, Default
    }

    public enum NotificationType
    {
        Friend, Game, Result, System
    }

    public class Notification
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
        public bool Sent { get; set; }
        public DateTime Expires { get; set; }
        public NotificationMethodType Method { get; set; }
        public NotificationType Type { get; set; }
        public virtual UserProfile User { get; set; }
        
        //foreign keys to objects that caused the notification
        //should be objects instead?
        //should be an enumeration
        public int UserId { get; set; }
        public int GameID { get; set; }
        public int FriendID { get; set; }
        public int ResultID { get; set; }

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
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("notification_id", this.NotificationID.ToString());
            data.Add("created", ToUnixTime((DateTime) this.Created));
            data.Add("message", this.Message);
            data.Add("type", this.Type.ToString());
            data.Add("user_id", this.User.UserId.ToString());
            data.Add("game_id", this.GameID.ToString());
            data.Add("result_id", this.ResultID.ToString());
       
            

            return data;
        }
    }
}