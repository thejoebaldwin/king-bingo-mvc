using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using KingBingo.Models;

namespace TestJSON
{
    public partial class frmTestJSON : Form
    {
        public string[] gamecardNumbers;
        public ArrayList drawnNumbers;
        public int gameSpeed = 0;
        public string profileImage = "";
        public UserProfile user;
        public frmTestJSON()
        {
            InitializeComponent();
        }

        private string getTargetUrl()
        {
            string targetURL = "";
            if (cbTarget.SelectedIndex == 0)
            {
                targetURL = "http://localhost:22986/service/v0";

            }
            else if (cbTarget.SelectedIndex == 1)
            {
                targetURL = "http://itweb.fvtc.edu/kingbingo/service/v0";
            }
            return targetURL;
        }


        private string PostDataWithOperation(string operation, string JSON)
        {
            //build request
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/{1}", getTargetUrl(), operation));
            //set http method
            request.Method = "POST";
            //content type
            request.ContentType = "text/html";
            //build json
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(JSON);
            //write to request body
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = request.GetResponse().GetResponseStream();
            //read response body
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            processResponse(response_json);
            return response_json;

        }

        private void btnGetAllGames_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ",\"page\":\"0\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allgames", post_json);
        }

        private void btnGetAllUsers_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            if (chkIncludeProfileImages.Checked)
            {
                post_json += ",\"include_profile_images\":\"true\"";
            }
            post_json += ",\"page\":\"0\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allusers", post_json); ;
        }


        public string ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds).ToString();
        }

        public DateTime FromUnixTime(string timestamp)
        {
            // Unix timestamp is seconds past epoch
            double timestampSeconds = Convert.ToDouble(timestamp);
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestampSeconds).ToLocalTime();
            return dtDateTime;
        }

        private void processResponse(string response_json)
        {

            try
            {
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response_json);

                if (data.operation == "auth")
                {
                    //var user = data.user;
                    user = UserProfile.FromData(data.user);
                    if (user != null)
                    {
                        byte[] buffer = user.ProfileImage; 
                        MemoryStream ms = new MemoryStream(buffer);
                        picProfileImage.Image = Image.FromStream(ms);

                        txtUpdateUser_Bio.Text = user.Bio;
                        txtUpdateUser_Name.Text = user.Name;
                        txtUpdateUser_Zip.Text = user.Zip;
                        if (user.Sex == Sex.Male)
                        {
                            rbUpdateUser_Male.Checked = true;
                            rbUpdateUser_Female.Checked = false;
                        }
                        else
                        {
                            rbUpdateUser_Male.Checked = false;
                            rbUpdateUser_Female.Checked = true;
                        }

                        DateTime birthdate =(DateTime) user.Birthdate;  //FromUnixTime((string) user.birthdate);
                        dateUpdateUser_Birthdate.Value = birthdate;
                        string location = user.Location;
                        if (location != "")
                        {
                            string[] splitString = { "," };
                            double latitude = Convert.ToDouble(location.Split(splitString, StringSplitOptions.None)[0]);
                            double longitude = Convert.ToDouble(location.Split(splitString, StringSplitOptions.None)[1]);
                            txtUpdateUser_Latitude.Text = latitude.ToString();
                            txtUpdateUser_Longitude.Text = longitude.ToString();
                        }
                    }

                }
                else if (data.operation == "allgames")
                {

                }
                else if (data.operation == "allfriends")
                {
                    var temp = data.friends;
                    List<Friend> friends = new List<Friend>();
                    foreach (dynamic d in temp)
                    {
                        var friend = Friend.FromData(d);
                        friends.Add(friend);
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("FriendID", typeof( Int32)));
                    dt.Columns.Add(new DataColumn("UserName", typeof(string)));
                    dt.Columns.Add(new DataColumn("Status", typeof(string)));
                    dt.Columns.Add(new DataColumn("Created", typeof(DateTime)));

                  foreach (Friend f in friends)
                    {
                        DataRow dr = dt.NewRow();
                        dr["FriendID"] = f.FriendID;
                        dr["UserName"] = f.FriendUser.UserName;
                        dr["Status"] = f.Status;
                        dr["Created"] = f.Created;
                        dt.Rows.Add(dr);

                    }

                    dgFriends.DataSource = dt;
                  
                }
                else if (data.operation == "joingame")
                {
                    var gamecard = data.game_card;
                    var gamespeed = data.game_speed;
                    string[] splitString = { "," };
                    gamecardNumbers = ((string)gamecard).Split(splitString, StringSplitOptions.None);
                    for (int i = 0; i < gamecardNumbers.Length; i++)
                    {
                        Label tempLabel = getNumberLabelByNumber(i);
                        tempLabel.Text = gamecardNumbers[i];
                    }

                    timerNumbers.Enabled = true;
                    double delay = ((100 - gamespeed) / 100.0) * 60.0;
                    timerNumbers.Interval = (int)delay * 1000;

                }
                else if (data.operation == "getprofileimage")
                {
                    string profileimage = data.profile_image;
                    byte[] buffer = Convert.FromBase64String(profileimage);
                    MemoryStream ms = new MemoryStream(buffer);
                    picProfileImage.Image = Image.FromStream(ms);
                }
                else if (data.operation == "getnumber")
                {
                    string bingoNumber = data.number;
                    if (bingoNumber != "-1")
                    {
                        int number = UnBingofyNumber(bingoNumber);
                        for (int i = 0; i < 25; i++)
                        {
                            drawnNumbers.Add(number);
                            Label lblTemp = getNumberLabelByNumber(i);
                            if (lblTemp.Text == number.ToString())
                            {
                                lblTemp.BackColor = Color.Yellow;
                                checkIfWin();
                            }
                        }
                    }
                    else
                    {
                        timerNumbers.Enabled = false;
                        timerNumbers.Stop();
                    }
                    lblBingoNumber.Text = bingoNumber;

                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }



        bool verifyWin(string bingo, string gamecard)
        {
            string[] splitString = { "," };
            string[] bingoNumbers = bingo.Split(splitString, StringSplitOptions.None);
            string[] userGameCard = gamecard.Split(splitString, StringSplitOptions.None);
            bool win = true;
            //first check that these numbers were drawn
            foreach (string number in bingoNumbers)
            {
                int n = System.Convert.ToInt32(number);
                if (!drawnNumbers.Contains(n))
                {
                    win = false;
                    break;
                }
                else
                {
                    for (int i = 0; i < userGameCard.Length; i++)
                    {
                        if (userGameCard[i] == number)
                        {
                            userGameCard[i] = "x";
                        }
                    }
                }
            }
            //if all the claimed winning numbers have been drawn, check for each winning scenario
            if (win)
            {
                win = false;
                for (int row = 0; row < 5; row++)
                {
                    //check all rows
                    if (userGameCard[0 + row] == "x" &&
                       userGameCard[5 + row] == "x" &&
                       userGameCard[10 + row] == "x" &&
                       userGameCard[15 + row] == "x" &&
                       userGameCard[20 + row] == "x")
                    {
                        win = true;
                        break;
                    }
                }
                if (!win)
                {
                    //check all columns
                    for (int column = 0; column < 5; column++)
                    {
                        int offsetColumn = column * 5;
                        if (
                          userGameCard[0 + offsetColumn] == "x" &&
                          userGameCard[1 + offsetColumn] == "x" &&
                          userGameCard[2 + offsetColumn] == "x" &&
                          userGameCard[3 + offsetColumn] == "x" &&
                          userGameCard[4 + offsetColumn] == "x")
                        {
                            win = true;
                            break;
                        }
                    }
                }
                if (!win)
                {
                    //backwards diagonal
                    if (
                        userGameCard[0] == "x" &&
                        userGameCard[6] == "x" &&
                        userGameCard[12] == "x" &&
                        userGameCard[18] == "x" &&
                        userGameCard[24] == "x")
                        {
                        win = true;
                        }
                }
                if (!win)
                {
                    //forwards diagonal
                    if (
                        userGameCard[4] == "x" &&
                        userGameCard[8] == "x" &&
                        userGameCard[12] == "x" &&
                        userGameCard[16] == "x" &&
                        userGameCard[20] == "x")
                        {
                            win = true;
                        }
                }
            }

    

            return win;
        }

        string stringifyGameCard()
        {

           return string.Join(",", gamecardNumbers);
        }

        string getWinningNumbers()
        {
            //check columns
            //check rows
            //check diagonals

            bool win = false;
            string bingo = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                bingo = rowWin(i);
                if (bingo != string.Empty) break;
            }
            if (bingo == string.Empty)
            {
                for (int i = 0; i < 5; i++)
                {
                    bingo = columnWin(i);
                    if (bingo != string.Empty) break;
                }
            }
            if (bingo == string.Empty)
            {
                bingo = forwardDiagonalWin();
            }
            if (bingo == string.Empty)
            {
                bingo = backwardDiagonalWin();
            }
            return bingo;
        }

        void checkIfWin()
        {
            //check columns
            //check rows
            //check diagonals

            bool win = false;
            string bingo = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                bingo = rowWin(i);
                if (bingo != string.Empty) break;
            }
            if (bingo == string.Empty)
            {
                for (int i = 0; i < 5; i++)
                {
                    bingo = columnWin(i);
                    if (bingo != string.Empty) break;
                }
            }
            if (bingo == string.Empty)
            {
                bingo = forwardDiagonalWin();
            }
            if (bingo == string.Empty)
            {
                bingo = backwardDiagonalWin();
            }
            if (bingo != string.Empty)
            {
                timerNumbers.Enabled = false;
                timerNumbers.Stop();

                if (verifyWin(bingo, string.Join(",", gamecardNumbers)))
                {
                    MessageBox.Show("BINGO!\n" + bingo);
                }
                else
                {
                    MessageBox.Show("false claim bingo\n" + bingo);
                }

            }

        }


        string rowWin(int row)
        {
            string bingo = string.Empty;
          
            if (getNumberLabelByNumber(0 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(5 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(10 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(15 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(20 + row).BackColor == Color.Yellow)
            {
                bingo = gamecardNumbers[0 + row].ToString();
                bingo += "," + gamecardNumbers[5 + row].ToString();
                bingo += "," + gamecardNumbers[10 + row].ToString();
                bingo += "," + gamecardNumbers[15 + row].ToString();
                bingo += "," + gamecardNumbers[20 + row].ToString();
               
            }
            return bingo;

        }

        string columnWin(int column)
        {
            string bingo = string.Empty;
            column = column * 5;
            bool win = false;
            if (getNumberLabelByNumber(0 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(1 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(2 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(3 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(4 + column).BackColor == Color.Yellow)
            {
                 bingo = gamecardNumbers[0 + column].ToString();
                bingo += "," + gamecardNumbers[1 + column].ToString();
                bingo += "," + gamecardNumbers[2 + column].ToString();
                bingo += "," + gamecardNumbers[3 + column].ToString();
                bingo += "," + gamecardNumbers[4 + column].ToString();
                win = true;
            }
            return bingo;
        }
        string forwardDiagonalWin()
        {
            //0, 6, 12, 18, 24
            string bingo = string.Empty;
            bool win = false;
            if (getNumberLabelByNumber(0).BackColor == Color.Yellow &&
                getNumberLabelByNumber(6).BackColor == Color.Yellow &&
                getNumberLabelByNumber(12).BackColor == Color.Yellow &&
                getNumberLabelByNumber(18).BackColor == Color.Yellow &&
                getNumberLabelByNumber(24).BackColor == Color.Yellow)
            {
                 bingo = gamecardNumbers[0].ToString();
                bingo += "," + gamecardNumbers[6].ToString();
                bingo += "," + gamecardNumbers[12].ToString();
                bingo += "," + gamecardNumbers[18].ToString();
                bingo += "," + gamecardNumbers[24].ToString();
                win = true;
            }
            return bingo;

        }
        string backwardDiagonalWin()
        {
            //4,8, 12, 16, 20

            string bingo = string.Empty;
            bool win = false;
            if (getNumberLabelByNumber(4).BackColor == Color.Yellow &&
                getNumberLabelByNumber(8).BackColor == Color.Yellow &&
                getNumberLabelByNumber(12).BackColor == Color.Yellow &&
                getNumberLabelByNumber(16).BackColor == Color.Yellow &&
                getNumberLabelByNumber(20).BackColor == Color.Yellow)
            {
                bingo = gamecardNumbers[4].ToString();
                bingo += "," + gamecardNumbers[8].ToString();
                bingo += "," + gamecardNumbers[12].ToString();
                bingo += "," + gamecardNumbers[16].ToString();
                bingo += "," + gamecardNumbers[20].ToString();
                win = true;
            }
            return bingo;
        }

        private int UnBingofyNumber(string bingoNumber)
        {
            int number = -1;
            bingoNumber = bingoNumber.Substring(1);
            number = System.Convert.ToInt32(bingoNumber);
            return number;
        }

        private string BingofyNumber(string number)
        {
            int n = System.Convert.ToInt32(number);
            string bingoNumber = number;

            if (n <= 15)
            {
                bingoNumber = "B" + bingoNumber;
            }
            else if (n <= 30)
            {
                bingoNumber = "I" + bingoNumber;
            }
            else if (n <= 45)
            {
                bingoNumber = "N" + bingoNumber;
            }
            else if (n <= 60)
            {
                bingoNumber = "G" + bingoNumber;
            }
            else if (n <= 70)
            {
                bingoNumber = "0" + bingoNumber;
            }
            return bingoNumber;
        }

        private void btnGetUser_Click(object sender, EventArgs e)
        {

            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\", \"query_user_id\":\"1\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("getuser", post_json);
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {

            string hash = createAuthHash(txtPassword.Text.Trim());
            string post_json = "{\"username\":\"" + txtUserName.Text.Trim() + "\", \"hash\":\"" + hash + "\"}";
            string response_json = PostDataWithOperation("auth", post_json);
            txtRequest.Text = post_json;
            txtResponse.Text = response_json;
            processResponse(response_json);
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {

            string post_json = "{\"username\":\"" + txtUserName.Text.Trim() + "\", \"password\":\"" + txtPassword.Text.Trim() + "\",\"email\":\"" + txtEmail.Text.Trim() + "\"}";
            txtRequest.Text = post_json;

            txtResponse.Text = PostDataWithOperation("createuser", post_json);

        }

        static private string SHA1(string cleartext)
        {
            byte[] p2 = System.Text.Encoding.Unicode.GetBytes(cleartext);
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] result = sha.ComputeHash(p2);
            string encodedText = Convert.ToBase64String(result);
            return encodedText;
        }


        static public string createAuthHash(string password)
        {
            string hash = SHA1(password);
            hash = hash + DateTime.Today.ToString("yyyy-MM-dd"); //YYYY-MM-dd
            hash = SHA1(hash);
            return hash;
        }


        private void btnHash_Click(object sender, EventArgs e)
        {

            lblHash.Text = createAuthHash(txtPassword.Text.Trim());
        }




        private void frmTestJSON_Load(object sender, EventArgs e)
        {
            cbTarget.SelectedIndex = 0;
            drawnNumbers = new ArrayList();

            foreach (Control c in tabBarOperations.TabPages["tabGame"].Controls)
            {
                if (c.GetType() == (new Label()).GetType())
                {
                    if (c.Name.Contains("lbl_"))
                    {
                        c.Click += new EventHandler(numberLabel_Click);
                    }

                }

            }


        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnCreateGame_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";

            post_json += ",\"win_limit\":\"" + txtWinLimit.Text + "\", \"user_limit\":\"" + txtUserLimit.Text + "\"";
            post_json += ",\"game_speed\":\"" + txtSpeed.Text + "\", \"name\":\"" + txtName.Text + "\"";
            post_json += ",\"description\":\"" + txtDescription.Text + "\"";

            post_json += ",\"player_ids\":\"" + txtPlayers.Text + "\", \"private\":\"" + chkPrivate.Checked.ToString() + "\"";
            post_json += "}";

            txtRequest.Text = post_json;

            txtResponse.Text = PostDataWithOperation("creategame", post_json);
        }

        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("joingame", post_json);


            for (int i = 0; i < 25; i++)
            {
                Label tempLabel = getNumberLabelByNumber(i);
                tempLabel.BackColor = Color.Silver;
            }

        }

        private void btnQuitGame_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("quitgame", post_json);
        }

        private void btnGetNextNumber_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("getnumber", post_json);
        }

        private void btnAddFriend_Click(object sender, EventArgs e)
        {
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);
            int friend_user_id = Convert.ToInt32(txtFriend_friend_user_id.Text);
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ", \"friend_id\":\"" + friend_user_id + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("addfriend", post_json);
        }

        public Label getNumberLabel(string letter, int row)
        {
            Label numberLabel = (Label)this.Controls.Find("lbl_" + letter + row.ToString(), true)[0];
            return numberLabel;
        }

        public Label getNumberLabelByNumber(int index)
        {
            string letter = "";
            int row = index % 5;
            if (index < 5)
            {
                letter = "b";
            }
            else if (index < 10)
            {
                letter = "i";

            }
            else if (index < 15)
            {
                letter = "n";
            }
            else if (index < 20)
            {
                letter = "g";
            }
            else if (index < 25)
            {
                letter = "o";
            }
            Label numberLabel = (Label)this.Controls.Find("lbl_" + letter + row.ToString(), true)[0];
            return numberLabel;
        }

        protected void numberLabel_Click(object sender, EventArgs e)
        {
            Label numberLabel = (Label)sender;
            numberLabel.BackColor = Color.Red;




        }


        private void getNewNumber()
        {
             int user_id = Convert.ToInt32(txtFriend_user_id.Text);
            int friend_user_id = Convert.ToInt32(txtFriend_friend_user_id.Text);
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("getnumber", post_json);

        }

        private void timerNumbers_Tick(object sender, EventArgs e)
        {
            getNewNumber();
        }

        private void btnGetProfileImage_Click(object sender, EventArgs e)
        {
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);

            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("getprofileimage", post_json);
        }

        private void btnUploadProfileImage_Click(object sender, EventArgs e)
        {
            Stream fileStream = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((fileStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (fileStream)
                        {
                            byte[] buffer;
                            using (var streamReader = new MemoryStream())
                            {
                                fileStream.CopyTo(streamReader);
                                buffer = streamReader.ToArray();
                            }
                            profileImage = Convert.ToBase64String(buffer);
                        
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += " ,\"user\": {";
            string sub_json = "";
            if (profileImage != "")
            {
                sub_json += " \"profile_image\":\"" + profileImage + "\"";
            }
            if (txtUpdateUser_Latitude.Text != "" && txtUpdateUser_Longitude.Text != "")
            {
                if (sub_json != "") sub_json += ",";
                sub_json += string.Format("\"location\":\"{0},{1}\"", txtUpdateUser_Latitude.Text, txtUpdateUser_Longitude.Text);
            }
           
            post_json += sub_json + "}}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("updateuser", post_json);
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            if (timerNumbers.Enabled)
            {
                timerNumbers.Enabled = false;
                btnAuto.Text = "Auto Off";
                timerNumbers.Stop();
            }
            else
            {
                timerNumbers.Enabled = false;
                timerNumbers.Start();
         
                btnAuto.Text = "Auto On";
            }
        }

        private void btnDrawNumber_Click(object sender, EventArgs e)
        {
            getNewNumber();
        }

        private void txtSpeed_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCheckWin_Click(object sender, EventArgs e)
        {
            //checkIfWin();

            string bingo = getWinningNumbers();
            string game_id = txtGameID.Text;
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);
            string gamecard = string.Join(",", gamecardNumbers);

            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"";
            post_json += ",\"game_id\":\"" + game_id + "\", \"winning_numbers\":\"" + bingo + "\", \"game_card\":\"" + gamecard + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("callbingo", post_json);

        }

        private void btnAllFriends_Click(object sender, EventArgs e)
        {
       
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);


            string post_json = "{\"user_id\":\"" + user.UserId.ToString() + "\", \"authentication_token\":\"" + user.AuthenticationToken + "\"}";
             txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allfriends", post_json);
        }
    }
}

