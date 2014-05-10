using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Linq;
using WebMatrix.WebData;
using System.Threading.Tasks;
using KingBingo.DAL;

namespace KingBingo.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }


    public enum Sex
    {
        Male, Female
    }



    [Table("UserProfile")]
    public class UserProfile
    {
         private KingBingoContext db = new KingBingoContext();

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? Created { get; set; }
        public string DeviceToken { get; set; }
        public string Zip { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool? ReceiveEmails { get; set; }
        public string AuthenticationToken { get; set; }
        public DateTime? AuthenticationTokenExpires { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ConfirmationKey { get; set; }
        public bool? Active { get; set; }
        public Sex? Sex { get; set; }
        public string Bio { get; set; }
        public bool? Confirmed { get; set; }
        public string Location { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual ICollection<Result> Players { get; set; }
        public virtual GameCard GameCard { get; set; }
        public virtual Game Game { get; set; }


        public int? FriendCount { get; set; }
        public int? WinCount { get; set; }
        public int? Rank { get; set; }
        public int? GameCount { get; set; }

      


        public string AuthHash()
        {
            return UserProfile.createAuthHashFromHash(this.PasswordHash);
        }

        public void GenerateAuthenticationToken()
        {
               AuthenticationToken = Guid.NewGuid().ToString();
               AuthenticationTokenExpires = DateTime.Now.AddDays(7);

        }


        public virtual ICollection<Badge> Badges { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Result> Results { get; set; }

        private string ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds).ToString();
        }


        public Dictionary<string, string> ToData(bool includeImages, bool verbose)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("user_id", this.UserId.ToString());
            dict.Add("username", this.UserName);
            dict.Add("name", this.Name);
            dict.Add("bio", this.Bio);
            dict.Add("win_count", WinCount.ToString());
            dict.Add("friend_count", FriendCount.ToString());
            dict.Add("rank", this.Rank.ToString());
            dict.Add("game_count", GameCount.ToString());
            
            
            if (includeImages || verbose)
            {
                dict.Add("profile_image", Convert.ToBase64String(this.ProfileImage));
            }        
            if (this.Location != null)
            {
                dict.Add("location", string.Format("{0},{1}", this.Location[0], this.Location[1]));
            }
            if (verbose)
            {
             dict.Add("email", this.Email);
             dict.Add("confirmed", this.Confirmed.ToString());
             dict.Add("created", ToUnixTime((DateTime)this.Created));
             dict.Add("device_token", this.DeviceToken);
             dict.Add("zip", this.Zip);
             dict.Add("birthdate", ToUnixTime((DateTime)this.Birthdate));
             dict.Add("receive_emails", this.ReceiveEmails.ToString());
             dict.Add("authentication_token", this.AuthenticationToken);
             dict.Add("authentication_token_expires", ToUnixTime((DateTime)this.AuthenticationTokenExpires));
             dict.Add("birth_date", ToUnixTime((DateTime)this.Birthdate));
            }
            return dict;
        }


        static public UserProfile FromData(Dictionary<string, string> data, bool verbose, bool includeProfileImages)
        {
            var user = new UserProfile();
            
            user.UserId = System.Convert.ToInt32(data["user_id"]);
            user.UserName = data["username"];
            user.Name = data["name"];
            user.WinCount = System.Convert.ToInt32(data["win_count"]);
            user.Bio = data["bio"];
            user.Rank = System.Convert.ToInt32(data["rank"]);
            user.FriendCount = System.Convert.ToInt32(data["friend_count"]);
            user.FriendCount = System.Convert.ToInt32(data["game_count"]);
            if (includeProfileImages)
            {
                user.ProfileImage = Convert.FromBase64String(data["profile_image"]);
            }
            if (verbose)
            {
                user.AuthenticationToken = data["authentication_token"];
                user.AuthenticationTokenExpires = FromUnixTime((string)data["authentication_token_expires"]);
                user.Birthdate = FromUnixTime((string)data["birth_date"]);
                user.Confirmed = System.Convert.ToBoolean(data["confirmed"]);
                user.Created = UserProfile.FromUnixTime((string)data["created"]);
                user.DeviceToken = data["device_token"];
                user.Email = data["email"];
            }
            
            return user;
        }

        private static DateTime FromUnixTime(string timestamp)
        {
            // Unix timestamp is seconds past epoch
            double timestampSeconds = Convert.ToDouble(timestamp);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestampSeconds).ToLocalTime();
            return dtDateTime;
        }

        static public void GenerateRandomUsers(int count)
        {
            Random r = new Random();

            string[] firstNames = {
                                      "john",
                                      "rick",
                                      "fred",
                                      "tom",
                                      "jake",
                                      "suzy",
                                      "tanya",
                                      "beth",
                                      "jeff",
                                      "tony",
                                      "paul",
                                      "aaron",
                                      "mr",
                                      "ms",
                                      "mrs",
                                      "professor",
                                      "dr",
                                      "king",
                                      "silver",
                                      "golden",
                                      "liquid",
                                      "solid",
                                      "laser",
                                      "atomic",
                                      "dark"

                                  };

            string[] lastNames = {
                                      "paulson",
                                      "smith",
                                      "richards",
                                      "johnson",
                                      "thompson",
                                      "cooper",
                                      "mcdonald",
                                      "anderson",
                                      "lee",
                                      "robot",
                                      "ninja",
                                      "pirate",
                                      "wizard",
                                      "warlock",
                                      "unicorn",
                                      "volcano",
                                      "tornado",
                                      "earthquake",
                                      "skeleton",
                                      "phoenix",
                                      "baldwin"
                                  };

            string[] emails = {
                                  "fakemail.com",
                                  "emailcity.com",
                                  "downtownemails.com",
                                  "emailpallooza.com"
                              };

            string[] bioFirsts = {
                                    "lifelong",
                                    "the best",
                                    "the one and only",
                                    "super",
                                    "cold",
                                    "fire",
                                    "poison",
                                    "wind",
                                    "laser",
                                    "unflappable",
                                    "experienced",
                                    "long lost",
                                    "curious",
                                    "precocious",
                                    "atomic"
                                };

            string[] bioLasts = {
                                    "dog owner",
                                    "knitter",
                                    "sailor",
                                    "dinosaur",
                                    "engineer",
                                    "shaman",
                                    "doctor",
                                    "student",
                                    "biologist",
                                    "physicist",
                                    "teacher",
                                    "geologist",
                                    "mechanic",
                                    "antagonist",
                                    "protagonist",
                                    "secret agent"

                                   
                                };
            List<string> profileImages = new List<string>();
            string[] splitString = { "\\" };
            foreach (string file in Directory.EnumerateFiles(System.Web.HttpContext.Current.Server.MapPath("~/Images/profiles")))
            {
                string[] temp = file.Split(splitString, StringSplitOptions.None);
                profileImages.Add(temp.Last());
            }
            int counter = 0;
            KingBingoContext db = new KingBingoContext();
            while (counter < count)
            {

                string name = firstNames[r.Next(firstNames.Length - 1)] + " " + lastNames[r.Next(lastNames.Length - 1)];
                string username = name.Replace(" ", "");
                string email = name + emails[r.Next(emails.Length - 1)];
                string bio = bioFirsts[r.Next(bioFirsts.Length - 1)] + " " + bioLasts[r.Next(bioLasts.Length - 1)];
                string password = "test!234";

                var user = db.UserProfiles.Where(u => u.UserName == username).FirstOrDefault();
                if (user == null)
                {
                    user = db.UserProfiles.Where(u => u.UserName == username).FirstOrDefault();
                    if (user == null)
                    {
                        UserProfile.CreateUser(username, password, false);
                        user = db.UserProfiles.SingleOrDefault(u => u.UserName == username);
                        user.Name = name;
                        user.Email = email;
                        user.Bio = bio;
                        user.PasswordHash = UserProfile.SHA1(password);
                        user.Created = DateTime.Now;
                        user.Birthdate = DateTime.Now.AddYears((r.Next(20) + 10) * -1);
                        user.ProfileImage = UserProfile.GetProfileImage(profileImages[r.Next(profileImages.Count())]);
                        user.Friends = new List<Friend>();
                        user.FriendCount = 0;
                        user.Zip = "";
                        user.GameCount = 0;
                        user.ReceiveEmails = false;
                        user.Active = false;
                        user.Confirmed = false;
                        user.DeviceToken = "";
                        user.Sex = Models.Sex.Male;
                        user.Location = "0,0";
                        //user.Results = new List<Result>();
                        user.Badges = new List<Badge>();
                        user.WinCount = 0;
                        user.Rank = 0;
                        db.SaveChanges();
                    }
                   
                }
                counter++;
            }
         
            

        }

        static public string SHA1(string cleartext)
        {
            byte[] p2 = System.Text.Encoding.Unicode.GetBytes(cleartext);
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(p2);
            string encodedText = Convert.ToBase64String(result);
            return encodedText;
        }

        //already hashed once and stored in database
        static public string createAuthHashFromHash(string hash)
        {
            hash = hash + DateTime.Today.ToString("yyyy-MM-dd"); //YYYY-MM-dd
            hash = SHA1(hash);
            return hash;
        }

        static public string createAuthHash(string password)
        {
            string hash = SHA1(password);
            hash = hash + DateTime.Today.ToString("yyyy-MM-dd"); //YYYY-MM-dd
            hash = SHA1(hash);
            return hash;
        }

        public static void CreateUser(string username, string password, bool admin)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (admin)
            {
                if (!roles.RoleExists("Admin"))
                {
                    roles.CreateRole("Admin");
                }
            }

            if (membership.GetUser(username, false) == null)
            {
                membership.CreateUserAndAccount(username, password);
            }
            if (!roles.GetRolesForUser(username).Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { username }, new[] { "admin" });
            }

        }


        public static byte[] GetProfileImage(string name)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Images/Profiles/" + name));
            MemoryStream ms = new MemoryStream();
            if (name.ToLower().Contains(".jpg") || name.ToLower().Contains(".jpeg"))
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else  if (name.ToLower().Contains(".png"))
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }
          
          

           return ms.ToArray();
        }

    }
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
