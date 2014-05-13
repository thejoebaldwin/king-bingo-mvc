using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KingBingo.Models
{
    public class SystemMessage
    {
        public int SystemMessageID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public DateTime PublishOn { get; set; }
        
    }
}