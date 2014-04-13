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

namespace TestJSON
{
    public partial class frmTestJSON : Form
    {


        public string authentication_token = "";
        public int user_id = -1;
        public int gameSpeed = 0;
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
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
            post_json += ",\"page\":\"0\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allgames", post_json);
        }

        private void btnGetAllUsers_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
            if (chkIncludeProfileImages.Checked)
            {
                post_json += ",\"include_profile_images\":\"true\"";
            }
            post_json += ",\"page\":\"0\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allusers", post_json); ;
        }

        private void processResponse(string response_json)
        {

            try
            {
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response_json);

                if (data.operation == "auth")
                {
                    var user = data.user;
                    if (user != null)
                    {
                        user_id = user.user_id;
                        authentication_token = user.authentication_token;

                        string profileimage = user.profile_image;
                        byte[] buffer = Convert.FromBase64String(profileimage);
                        MemoryStream ms = new MemoryStream(buffer);
                        picProfileImage.Image = Image.FromStream(ms);


                    }

                }
                else if (data.operation == "joingame")
                {
                    var gamecard = data.game_card;
                    var gamespeed = data.game_speed;
                    string[] splitString = { "," };
                    string[] numbers = ((string)gamecard).Split(splitString, StringSplitOptions.None);
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        Label tempLabel = getNumberLabelByNumber(i);
                        tempLabel.Text = numbers[i];
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

                            Label lblTemp = getNumberLabelByNumber(i);
                            if (lblTemp.Text == number.ToString())
                            {
                                lblTemp.BackColor = Color.Yellow;
                                verifyWin();
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

            }
        }


        void verifyWin()
        {
            //check columns
            //check rows
            //check diagonals

            bool win = false;
            for (int i = 0; i < 5; i++)
            {
                win = rowWin(i);
                if (win) break;
            }
            if (!win)
            {
                for (int i = 0; i < 5; i++)
                {
                    win = columnWin(i);
                    if (win) break;
                }
            }
            if (!win)
            {
                win = forwardDiagonalWin();
            }
            if (!win)
            {
                win = backwardDiagonalWin();
            }
            if (win)
            {
                timerNumbers.Enabled = false;
                timerNumbers.Stop();
                MessageBox.Show("BINGO!");

            }

        }


        bool rowWin(int row)
        {
            bool win = false;
            if (getNumberLabelByNumber(0 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(5 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(10 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(15 + row).BackColor == Color.Yellow &&
                getNumberLabelByNumber(20 + row).BackColor == Color.Yellow)
            {

                win = true;
            }
            return win;

        }

        bool columnWin(int column)
        {
            column = column * 5;
            bool win = false;
            if (getNumberLabelByNumber(0 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(1 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(2 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(3 + column).BackColor == Color.Yellow &&
                getNumberLabelByNumber(4 + column).BackColor == Color.Yellow)
            {

                win = true;
            }
            return win;
        }
        bool forwardDiagonalWin()
        {
            //0, 6, 12, 18, 24

            bool win = false;
            if (getNumberLabelByNumber(0).BackColor == Color.Yellow &&
                getNumberLabelByNumber(6).BackColor == Color.Yellow &&
                getNumberLabelByNumber(12).BackColor == Color.Yellow &&
                getNumberLabelByNumber(18).BackColor == Color.Yellow &&
                getNumberLabelByNumber(24).BackColor == Color.Yellow)
            {

                win = true;
            }
            return win;

        }
        bool backwardDiagonalWin()
        {
            //4,8, 12, 16, 20


            bool win = false;
            if (getNumberLabelByNumber(4).BackColor == Color.Yellow &&
                getNumberLabelByNumber(8).BackColor == Color.Yellow &&
                getNumberLabelByNumber(12).BackColor == Color.Yellow &&
                getNumberLabelByNumber(16).BackColor == Color.Yellow &&
                getNumberLabelByNumber(20).BackColor == Color.Yellow)
            {

                win = true;
            }
            return win;
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

            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\", \"query_user_id\":\"1\"}";
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
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";

            post_json += ",\"win_limit\":\"" + txtWinLimit.Text + "\", \"user_limit\":\"" + txtUserLimit.Text + "\"";
            post_json += ",\"speed\":\"" + txtWinLimit.Text + "\", \"name\":\"" + txtName.Text + "\"";
            post_json += ",\"description\":\"" + txtDescription.Text + "\"";
            post_json += ",\"player_ids\":\"" + txtPlayers.Text + "\", \"private\":\"" + chkPrivate.Checked.ToString() + "\"";
            post_json += "}";

            txtRequest.Text = post_json;

            txtResponse.Text = PostDataWithOperation("creategame", post_json);
        }

        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
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
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("quitgame", post_json);
        }

        private void btnGetNextNumber_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("getnumber", post_json);
        }

        private void btnAddFriend_Click(object sender, EventArgs e)
        {
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);
            int friend_user_id = Convert.ToInt32(txtFriend_friend_user_id.Text);
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
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

        private void timerNumbers_Tick(object sender, EventArgs e)
        {
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);
            int friend_user_id = Convert.ToInt32(txtFriend_friend_user_id.Text);
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
            post_json += ", \"game_id\":\"" + txtGameID.Text + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("getnumber", post_json);
        }

        private void btnGetProfileImage_Click(object sender, EventArgs e)
        {
            int user_id = Convert.ToInt32(txtFriend_user_id.Text);

            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"}";
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
                            string profileimage = Convert.ToBase64String(buffer);
                            int user_id = Convert.ToInt32(txtFriend_user_id.Text);
                            int friend_user_id = Convert.ToInt32(txtFriend_friend_user_id.Text);
                            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"";
                            post_json += ", \"profile_image\":\"" + profileimage + "\"}";
                            txtRequest.Text = post_json;
                            txtResponse.Text = PostDataWithOperation("updateprofileimage", post_json);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}

