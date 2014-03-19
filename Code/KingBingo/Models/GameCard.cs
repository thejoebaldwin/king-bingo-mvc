using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{
    public class GameCard
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GameCardID { get; set; }
        public string Hash { get; set; }
        public ICollection<int> Numbers { get; set; }

    }
}