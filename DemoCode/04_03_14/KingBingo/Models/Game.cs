//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KingBingo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Game
    {
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> WinCount { get; set; }
        public Nullable<int> WinLimit { get; set; }
        public Nullable<int> UserCount { get; set; }
        public Nullable<int> UserLimit { get; set; }
    }
}