using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using KingBingo.Models;


namespace KingBingo.DAL
{
    public class KingBingoInitializer
    {
        public void Seed(kingbingoEntities2 context)
        {

            //create a test user
            string username = "test1";
            string password = "test1";

            var user = context.UserProfiles.SingleOrDefault(u => u.UserName == username);
            if (user == null)
            {
                UserProfile.CreateUser(username, password);
                user = context.UserProfiles.SingleOrDefault(u => u.UserName == username);
                user.Bio = "This is my bio";
                user.Birthdate = DateTime.Now.AddYears(-30);
                context.SaveChanges();
            }


           



        }


    }
}