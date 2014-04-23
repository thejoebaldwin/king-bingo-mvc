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
        Pending, Rejected, Accepted, Requested
    }


    public class Friend
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FriendID { get; set; }
        public DateTime Created { get; set; }
        public RequestStatus Status { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual UserProfile FriendUser { get; set; }

        public Friend()
        {
            Created = DateTime.Now;
        }

        public static Friend FromData(dynamic data)
        {
            Friend f = new Friend();
            UserProfile friendUser = new UserProfile();
            friendUser.UserName = data.username;
            friendUser.Name = data.name;
            friendUser.Bio = data.bio;
            friendUser.UserId = data.friend_user_id;
            f.FriendUser = friendUser;
            f.FriendID = data.friend_id;
            if (data.status == "Accepted")
            {
                f.Status = RequestStatus.Accepted;                                  
            }
            else if (data.status == "Pending")
            {
                f.Status = RequestStatus.Pending;
            }
            else if (data.status == "Requested")
            {
                f.Status = RequestStatus.Requested;
            }
            else if (data.status == "Rejected")
            {
                f.Status = RequestStatus.Rejected;
            }

            return f;
        }
    }
}