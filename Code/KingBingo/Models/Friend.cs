using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{
    public class Friend
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FriendID { get; set; }
        public DateTime Created { get; set; }

        public virtual UserProfile FriendA { get; set; }
        public virtual UserProfile FriendB { get; set; }
    }
}