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
        public virtual UserProfile User { get; set; }
        
        //foreign keys to objects that caused the notification
        //should be objects instead?
        //should be an enumeration
        public int UserId { get; set; }
        public int GameID { get; set; }
        public int FriendID { get; set; }
        public int ResultID { get; set; }

    }
}