using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{
    public enum OutcomeType
    {
        Quit, Loss, Win
    }

    public class Result
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ResultID { get; set; }
        public OutcomeType? Outcome { get;  set; }
        public DateTime Created { get; set; }
        public int Points { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual Game Game { get; set; }
    }
}