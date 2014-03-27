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
            return response_json;

        }

        private void btnGetAllGames_Click(object sender, EventArgs e)
        {
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allgames", post_json);
        }

        private void btnGetAllUsers_Click(object sender, EventArgs e)
        {
         
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token +  "\"}";
            txtRequest.Text = post_json;
            txtResponse.Text = PostDataWithOperation("allusers", post_json); ;
        }

        private void processResponse(string response_json)
        {
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response_json);
            var user = data.user;
            try
            {
                user_id = user.user_id;

                authentication_token = user.authentication_token;
            }
            catch (Exception ex)
            {

            }

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
        }
    }
}
