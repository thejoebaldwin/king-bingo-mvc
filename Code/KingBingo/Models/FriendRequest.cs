using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{
    public enum RequestStatus
    {
        Pending, Rejected, Accepted
    }

    public class FriendRequest
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FriendRequestID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public RequestStatus? Status { get; set; }

        public virtual UserProfile RequestUser { get; set; }
        public virtual UserProfile TargetUser { get; set; }
    }
}