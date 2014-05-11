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

namespace KingBingo
{
    public partial class frmTestJSON : Form
    {
        public TestJSON.frmDebug debugForm;
        public ArrayList drawnNumbers;
        public int gameSpeed = 0;
        public string profileImage = "";
        public UserProfile user;
        public bool debugOn = false;

        public KingBingo.Client client;

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

        private void btnGetAllGames_Click(object sender, EventArgs e)
        {
            client.GetAllGames(0, GetAllGamesComplete);
        }

        void checkIfWin()
        {
            string bingo = client.Card.GetBingo();

            if (bingo != string.Empty)
            {
                timerNumbers.Enabled = false;
                timerNumbers.Stop();
                MessageBox.Show("BINGO!\n" + bingo);
            }

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

        public void UpdateDebug()
        {
            debugForm.txtRequest.Text = client.Request;
            debugForm.txtResponse.Text = client.Response;
        }

        private void GetNumberComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.Number != -1)
            {
                for (int i = 0; i < 25; i++)
                {
                    drawnNumbers.Add(client.Number);
                    Label lblTemp = getNumberLabelByNumber(i);
                    if (lblTemp.Text == client.Number.ToString())
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
            lblBingoNumber.Text = BingofyNumber(client.Number.ToString());
        }

        private void CallBingoComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void RegisterComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void CreateGameComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        public void UpdateUserComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void JoinGameComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.Card != null)
            {

                for (int i = 0; i < 25; i++)
                {
                    Label tempLabel = getNumberLabelByNumber(i);
                    tempLabel.Text =  client.Card.Numbers.ElementAt(i).ToString();
                }

                timerNumbers.Enabled = true;
                double delay = ((100 - client.GameSpeed) / 100.0) * 60.0;
                timerNumbers.Interval = (int)(delay * 1000);
            }
        }

        private void GetUserComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
        }


        private void GetAllResultsComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void AddFriendComplete()
        {
            //do nothing?
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void AcceptFriendComplete()
        {
            //do nothing?
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void RejectFriendComplete()
        {
            //do nothing?
            UpdateDebug();
            lblMessage.Text = client.Message;
        }

        private void GetAllNotificationsComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.Notifications != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("NotificationID", typeof(Int32)));

                dt.Columns.Add(new DataColumn("Message", typeof(string)));

                foreach (Notification n in client.Notifications)
                {
                    DataRow dr = dt.NewRow();
                    dr["NotificationID"] = n.NotificationID;
                    dr["Message"] = n.Message;
        
                    dt.Rows.Add(dr);

                }
                dgNotifications.DataSource = dt;
            }
        }

        private void GetAllFriendsComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.Friends != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("FriendId", typeof(Int32)));

                dt.Columns.Add(new DataColumn("FriendUserId", typeof(Int32)));
                dt.Columns.Add(new DataColumn("UserName", typeof(string)));
                dt.Columns.Add(new DataColumn("Status", typeof(string)));
                dt.Columns.Add(new DataColumn("Created", typeof(DateTime)));

                foreach (Friend f in client.Friends)
                {
                    DataRow dr = dt.NewRow();
                    dr["FriendID"] = f.FriendID;

                    dr["FriendUserId"] = f.FriendUser.UserId;
                    dr["UserName"] = f.FriendUser.UserName;
                    dr["Status"] = f.Status;
                    dr["Created"] = f.Created;
                    dt.Rows.Add(dr);

                }

                dgFriends.DataSource = dt;
            }

        }

        private void GetAllUsersComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.Users != null)
            {
              
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("UserId", typeof(Int32)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("UserName", typeof(string)));


                foreach (UserProfile u in client.Users)
                {
                    DataRow dr = dt.NewRow();
                    dr["UserId"] = u.UserId;
                    dr["Name"] = u.Name;
                    dr["UserName"] = u.UserName;

                    dt.Rows.Add(dr);
                }
                dgUsers.DataSource = dt;
            }
        }

        private void QuitGameComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            for (int i = 0; i < 25; i++)
            {
                Label tempLabel = getNumberLabelByNumber(i);
                tempLabel.Text = tempLabel.Name;
            }
            timerNumbers.Enabled = false;
        }

        private void GetAllGamesComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.Games != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("GameID", typeof(Int32)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Description", typeof(string)));
                dt.Columns.Add(new DataColumn("Created", typeof(DateTime)));

                foreach (Game g in client.Games)
                {
                    DataRow dr = dt.NewRow();
                    dr["GameID"] = g.GameID;
                    dr["Name"] = g.Name;
                    dr["Description"] = g.Description;
                    dr["Created"] = g.Created;
                    dt.Rows.Add(dr);
                }

                dgGames.DataSource = dt;
            }

        }

        private void AuthenticateComplete()
        {
            UpdateDebug();
            lblMessage.Text = client.Message;
            if (client.User != null)
            {
                byte[] buffer = client.User.ProfileImage;
                MemoryStream ms = new MemoryStream(buffer);
                picProfileImage.Image = Image.FromStream(ms);
                profileImage = Convert.ToBase64String(client.User.ProfileImage);
                txtUpdateUser_Email.Text = client.User.Email;
                txtUpdateUser_Bio.Text = client.User.Bio;
                txtUpdateUser_Name.Text = client.User.Name;
                txtUpdateUser_Zip.Text = client.User.Zip;
                if (client.User.Sex == Sex.Male)
                {
                    rbUpdateUser_Male.Checked = true;
                    rbUpdateUser_Female.Checked = false;
                }
                else
                {
                    rbUpdateUser_Male.Checked = false;
                    rbUpdateUser_Female.Checked = true;
                }

                DateTime birthdate = (DateTime)client.User.Birthdate;  //FromUnixTime((string) user.birthdate);
                dateUpdateUser_Birthdate.Value = birthdate;
                string location = client.User.Location;
                if (location != null && location != "")
                {
                    string[] splitString = { "," };
                    double latitude = Convert.ToDouble(location.Split(splitString, StringSplitOptions.None)[0]);
                    double longitude = Convert.ToDouble(location.Split(splitString, StringSplitOptions.None)[1]);
                    txtUpdateUser_Latitude.Text = latitude.ToString();
                    txtUpdateUser_Longitude.Text = longitude.ToString();
                }
                tabBarOperations.Enabled = true;
                btnAuth.Text = "Sign Out";
            }

        }



        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            client.Register(txtUserName.Text.Trim(), txtPassword.Text.Trim(), txtEmail.Text.Trim(), RegisterComplete);
        }

   

        private void frmTestJSON_Load(object sender, EventArgs e)
        {
            cbTarget.SelectedIndex = 0;
            drawnNumbers = new ArrayList();

            debugForm = new TestJSON.frmDebug();

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

    

        private void btnCreateGame_Click(object sender, EventArgs e)
        {
            int winLimit = 0;
            int userLimit = 0;
            int gameSpeed = 0;
            Int32.TryParse(txtWinLimit.Text, out winLimit);
            Int32.TryParse(txtUserLimit.Text, out userLimit);
            Int32.TryParse(txtSpeed.Text, out gameSpeed);
            client.CreateGame(winLimit, userLimit, gameSpeed, txtName.Text, txtDescription.Text, "", false, CreateGameComplete);
        }

        private void btnJoinGame_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
            {
                Label tempLabel = getNumberLabelByNumber(i);
                tempLabel.BackColor = Color.Silver;
            }

            if (dgGames.SelectedRows != null && dgGames.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgGames.SelectedRows[0];
                int game_id = (int)dr.Cells[0].Value;
                client.JoinGame(game_id, JoinGameComplete);
            }
        }

        private void btnQuitGame_Click(object sender, EventArgs e)
        {
            if (dgGames.SelectedRows != null && dgGames.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgGames.SelectedRows[0];
                int game_id = (int)dr.Cells[0].Value;
                client.QuitGame(game_id, QuitGameComplete);
            }
        }

        private void btnGetNextNumber_Click(object sender, EventArgs e)
        {
            client.GetNewNumber(GetNumberComplete);
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
            client.GetNewNumber(GetNumberComplete);
        }

        private void timerNumbers_Tick(object sender, EventArgs e)
        {
            getNewNumber();
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
            string name = txtUpdateUser_Name.Text;
            string bio = txtUpdateUser_Bio.Text;
            string email = txtUpdateUser_Email.Text;
            double latitude = 0;
            double longitude = 0;
            string zip = txtUpdateUser_Zip.Text;
            DateTime birthdate = DateTime.Now;
            KingBingo.Models.Sex sex = KingBingo.Models.Sex.Male;
            if (rbUpdateUser_Female.Checked)  sex = KingBingo.Models.Sex.Female;
            client.UpdateUser(name, bio, email, zip, sex, birthdate, profileImage, latitude, longitude, UpdateUserComplete);
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

        private void btnCheckWin_Click(object sender, EventArgs e)
        {
            string bingo = client.Card.GetBingo();
            client.CallBingo(bingo, CallBingoComplete);
        }

        private void btnAllFriends_Click(object sender, EventArgs e)
        {
            client.GetAllFriends(0, GetAllFriendsComplete);
        }

  

        private void btnGetAllUsers_Click(object sender, EventArgs e)
        {
            client.GetAllUsers(0, chkIncludeProfileImages.Checked, GetAllUsersComplete);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add friend
            if (dgUsers.SelectedRows != null && dgUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgUsers.SelectedRows[0];
                int friend_user_id = (int)dr.Cells[0].Value;
                client.AddFriend(friend_user_id, AddFriendComplete);
            }
       
        }

        private void btnAcceptFriend_Click(object sender, EventArgs e)
        {
            //accept friend
            if (dgFriends.SelectedRows != null && dgFriends.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgFriends.SelectedRows[0];
                int friend_user_id = (int)dr.Cells[1].Value;
                client.AcceptFriend(friend_user_id, AcceptFriendComplete);
                if ((string)dr.Cells[3].Value == "Pending")
                {
                  client.AcceptFriend(friend_user_id, AcceptFriendComplete);
                }
             }
        }

        private void btnRejectFriend_Click(object sender, EventArgs e)
        {
            if (dgFriends.SelectedRows != null && dgFriends.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgFriends.SelectedRows[0];
                int friend_user_id = (int)dr.Cells[1].Value;
                if ((string)dr.Cells[3].Value == "Pending")
                {
                    client.RejectFriend(friend_user_id, RejectFriendComplete);
                }
 
            }
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            client.GetAllNotifications(0, GetAllNotificationsComplete);
        }

        private void tabUser_Click(object sender, EventArgs e)
        {

        }


        private void btnGetUser_Click_1(object sender, EventArgs e)
        {
            if (dgUsers.SelectedRows != null && dgUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgUsers.SelectedRows[0];
                int user_id = (int)dr.Cells[0].Value;
                client.GetUser(user_id, GetUserComplete);
            }
        }

        private void btnAuth_Click_1(object sender, EventArgs e)
        {
            if (btnAuth.Text == "Sign In")
            {
                string url = getTargetUrl();
                client = new KingBingo.Client(url);
                client.Authenticate(txtUserName.Text.Trim(), txtPassword.Text.Trim(), AuthenticateComplete);
            }
            else
            {
                tabBarOperations.Enabled = false;
                dgFriends.Rows.Clear();
                dgUsers.Rows.Clear();
                dgGames.Rows.Clear();
                picProfileImage.Image = null;
                txtUpdateUser_Name.Text = string.Empty;
                txtUpdateUser_Bio.Text = string.Empty;
                txtUpdateUser_Email.Text = string.Empty;
                btnAuth.Text = "Sign In";
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            debugForm.Show();
        }

        private void btnAllResults_Click(object sender, EventArgs e)
        {
            client.GetAllResults(0, GetAllResultsComplete);
        }
    }
}

