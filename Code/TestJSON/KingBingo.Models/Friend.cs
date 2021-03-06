﻿using System;
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


        public dynamic ToData()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("friend_id", this.FriendID.ToString());
            dict.Add("friend_user_id", this.FriendUser.UserId.ToString());
            dict.Add("username", this.FriendUser.UserName);
            dict.Add("name", this.FriendUser.Name);
            dict.Add("status", this.Status.ToString());
            dict.Add("bio", this.FriendUser.Bio);
            return dict;
        }

        public static Friend FromData(Dictionary<string, string> data)
        {
            Friend f = new Friend();
            UserProfile friendUser = new UserProfile();
            friendUser.UserName = data["username"];
            friendUser.Name = data["name"];
            friendUser.Bio = data["bio"];
            friendUser.UserId = System.Convert.ToInt32( data["friend_user_id"]);
            f.FriendUser = friendUser;
            f.FriendID = System.Convert.ToInt32( data["friend_id"]);
            if (data["status"] == "Accepted")
            {
                f.Status = RequestStatus.Accepted;                                  
            }
            else if (data["status"] == "Pending")
            {
                f.Status = RequestStatus.Pending;
            }
            else if (data["status"] == "Requested")
            {
                f.Status = RequestStatus.Requested;
            }
            else if (data["status"] == "Rejected")
            {
                f.Status = RequestStatus.Rejected;
            }

            return f;
        }
    }
}