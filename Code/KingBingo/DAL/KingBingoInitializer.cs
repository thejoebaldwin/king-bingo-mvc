using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using KingBingo.Models;
using System.Web.Security;

using WebMatrix.WebData;

using System.IO;

namespace KingBingo.DAL
{

    
  
   public class DropCreateIfChangeInitializer : System.Data.Entity.DropCreateDatabaseAlways<KingBingoContext>
    //public class DropCreateIfChangeInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<KingBingoContext>
    {


        

        protected override void Seed(KingBingoContext context)
        {

          WebSecurity.InitializeDatabaseConnection("DefaultConnection",
             "UserProfile", "UserId", "UserName", autoCreateTables: true);


            var badgesUser1 = new List<Badge>
            {

                new Badge{Name = "Badge1",  Description = "This is a description for badge1"},
                new Badge{Name = "Badge2", Description = "This is a description for badge2"}

            };


            foreach (Badge b in badgesUser1)
            {
                b.Image = Badge.GetImage(b.Name + ".png");
                context.Badges.Add(b);
            }

         
            context.SaveChanges();

            var badgesUser2 = new List<Badge>
            {

                new Badge{Name = "Badge1", Description = "This is a description for badge1"},
                new Badge{Name = "Badge2", Description = "This is a description for badge2"}

            };

            foreach (Badge b in badgesUser2)
            {
                b.Image = Badge.GetImage(b.Name + ".png");
                context.Badges.Add(b);
            }

        
            context.SaveChanges();

            //GAMECARD
            var gameCards = new List<GameCard>
            {
               GameCard.GenerateGameCard(),
               GameCard.GenerateGameCard()
            };
            gameCards.ForEach(gc => context.GameCards.Add(gc));


            context.SaveChanges();

            //USERS
            UserProfile.CreateUser("test1", "test1", true);
            var user1 = context.UserProfiles.SingleOrDefault(u => u.UserName == "test1");
            user1.Name = "Test User 1";
            user1.Email = "test@test.com";
            //hash it once and store it
            user1.PasswordHash = UserProfile.SHA1("test1");
            user1.Bio = "This is the Bio for test user 1";
            user1.Created = DateTime.Now;
            user1.DeviceToken = "0123456789ABCDEF";
            user1.Zip = "54915";
            user1.WinCount = 0;
            user1.FriendCount = 0;
            user1.Rank = 0;
            user1.GameCount = 0;
            user1.Birthdate = new DateTime(1977, 10, 25);
            user1.ReceiveEmails = true;
            user1.AuthenticationToken = "0123456789ABCDEF";
            user1.AuthenticationTokenExpires = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));
            user1.ProfileImage = UserProfile.GetProfileImage("crown.png"); ;
            user1.ConfirmationKey = "0123456789ABCDEF";
            user1.Active = true;
            user1.Sex = Sex.Male;
            user1.Confirmed = true;
            user1.Location = "88,-120";
            user1.GameCard = gameCards[0];
            user1.Badges = badgesUser1;



            

         
            //
            UserProfile.CreateUser("test2", "test2", true);
            var user2 = context.UserProfiles.SingleOrDefault(u => u.UserName == "test2");
            user2.Name = "Test User 2";
            user2.Bio = "This is the Bio for test user 2";
            user2.Email = "test2@test.com";
            //hash it once and store it
            user2.PasswordHash = UserProfile.SHA1("test2");
            user2.Created = DateTime.Now;
            user2.DeviceToken = "0123456789ABCDEF";
            user2.Zip = "54915";
      
            user2.WinCount = 0;
            user2.FriendCount = 0;
            user2.Rank = 0;
            user2.GameCount = 0;
            user2.Birthdate = new DateTime(1977, 10, 25);
            user2.ReceiveEmails = true;
            user2.AuthenticationToken = "0123456789ABCDEF";
            user2.AuthenticationTokenExpires = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0));
            user2.ProfileImage =  UserProfile.GetProfileImage("crown.png"); ;
            user2.ConfirmationKey = "0123456789ABCDEF";
            user2.Active = true;
            user2.Sex = Sex.Female;
            user2.Confirmed = true;
            user2.Location = "88,-120";
            user2.GameCard = gameCards[1];
            user2.Badges = badgesUser2;

            var players = new List<UserProfile>
            {
              user1,
              user2
            };
      
            context.SaveChanges();

            var friend1 = new Friend { User = user1, FriendUser = user2, Status = RequestStatus.Accepted };
            var friend2 = new Friend { User = user2, FriendUser = user1, Status = RequestStatus.Accepted };

            context.Friends.Add(friend1);
            context.Friends.Add(friend2);

            context.SaveChanges();

            user1.Friends = new List<Friend>();
            user2.Friends = new List<Friend>();

            user1.Friends.Add(friend1);
            user2.Friends.Add(friend2);
            user1.FriendCount++;
            user2.FriendCount++;
            context.SaveChanges();

            UserProfile.GenerateRandomUsers(50);

            //GAMES
            var games = new List<Game>
            {
                new Game{Name = "Test Game 1", Description="Test Game Description", WinLimit=1,UserLimit=10,   GameSpeed=99, Created=DateTime.Now, Private=false, Players = players}
            };
            user1.GameCount++;
            user2.GameCount++;
            games[0].GenerateGameCards();
            foreach (GameCard gc in games[0].GameCards)
            {
                context.GameCards.Add(gc);
            }
            //NEED TO GIVE USERS GAMECARDS HERE!!!
            context.SaveChanges();
            games[0].Results = new List<Result>();
            games.ForEach(g => context.Games.Add(g));
            context.SaveChanges();

        
            var results = new List<Result>
            {
                new Result{Outcome = OutcomeType.Win, Points = 5, User = user1, Game = games[0], Created = DateTime.Now},
                new Result{Outcome = OutcomeType.Loss, Points = -5, User = user2, Game = games[0], Created = DateTime.Now}
            };

            results.ForEach(r => context.Results.Add(r));
            games[0].Results.Add(results[0]);
            games[0].Results.Add(results[1]);
            user1.Results = new List<Result>();
           user2.Results = new List<Result>();
            user1.Results.Add(results[0]);
            user2.Results.Add(results[1]);
            context.SaveChanges();
        }
    
    
    }


 
}