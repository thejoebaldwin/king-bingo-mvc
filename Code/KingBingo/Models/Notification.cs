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
            data.Add("friend_id", this.FriendID.ToString());
            return data;
        }

        public static Notification FromData(dynamic data)
        {
            Notification n = new Notification();
            n.Message = data.message;
            n.NotificationID = data.notification_id;
            n.Created = FromUnixTime((string)data.created);
            if (data.user_id != null)
            {
                n.UserId = data.user_id;
            }
            if (data.friend_id != null)
            {
                n.FriendID = data.friend_id;
            }
            if (data.game_id != null)
            {
                n.GameID = data.game_id;
            }
            if (data.result_id != null)
            {
                n.ResultID = data.result_id;
            }
            if (data.type != null)
            {
                if (data.type == "Friend") n.Type = NotificationType.Friend;
                else if (data.type == "Result") n.Type = NotificationType.Result;
                else if (data.type == "System") n.Type = NotificationType.System;
                else if (data.type == "Game") n.Type = NotificationType.Game;
            }

            return n;
        }
    }
}