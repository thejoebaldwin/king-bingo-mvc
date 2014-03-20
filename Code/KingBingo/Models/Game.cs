using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{
    public class Game
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int WinLimit { get; set; }
        public int UserLimit { get; set; }
        public int Speed { get; set; }
        public DateTime Created { get; set; }
        public bool Private { get; set; }
        public int NumbersIndex { get; set; }
        public ICollection<int> Numbers { get; set; }
        
        public virtual ICollection<UserProfile> Players { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }


}