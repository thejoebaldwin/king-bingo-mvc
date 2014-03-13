using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KingBingo.Models
{
    public class Support
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SupportID { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
        public bool Replied { get; set; }
        public bool Read { get; set; }
    }
}