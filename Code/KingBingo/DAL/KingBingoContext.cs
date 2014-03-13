using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using KingBingo.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Security;
using WebMatrix.WebData;

namespace KingBingo.DAL
{
    public class KingBingoContext : DbContext
    {

        public KingBingoContext() : base("DefaultConnection")
        {
       
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameCard> GameCards { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<SystemMessage> SystemMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

     
    }
}