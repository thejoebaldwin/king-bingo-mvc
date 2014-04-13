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
        Email, Push, Web
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
    }
}