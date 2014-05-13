using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//added these two
using System.Web.Security;
using WebMatrix.WebData;

namespace KingBingo.Models
{
    public partial class UserProfile
    {

        public enum UserCreateStatus
        {
            Success,
            UserExists,
            Error
        };

       


        public static UserCreateStatus CreateUser(string username, string password)
        {
            //get the membership provider  
            try
            {
                var membership = (SimpleMembershipProvider)Membership.Provider;

                //check if user already exists
                if (membership.GetUser(username, false) == null)
                {
                    //create the user
                    membership.CreateUserAndAccount(username, password);
                    return UserCreateStatus.Success;
                }
                else
                {
                    return UserCreateStatus.UserExists;
                }
            }
            catch (Exception ex)
            {
                return UserCreateStatus.Error;
            }
        }


    }
}