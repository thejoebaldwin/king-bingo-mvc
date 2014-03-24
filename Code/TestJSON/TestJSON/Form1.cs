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
    public partial class Form1 : Form
    {
        //const string AUTH_POST_URL = "http://localhost:22986/service/v0";
        const string AUTH_POST_URL = "http://itweb.fvtc.edu/kingbingo/service/v0";

        public string authentication_token = "";
        public int user_id = -1;
        public Form1()
        {
            InitializeComponent();
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static public string createPasswordHash(string password)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            string hash = password;
            byte[] data;
            byte[] result;
            //hash raw password once
            data = GetBytes(hash);
            result = sha.ComputeHash(data);
            //back to string
            hash = System.Text.Encoding.UTF8.GetString(result);
           // hash = BitConverter.ToString(result);
            //add today's date
            hash = password + DateTime.Today.ToString("yyyy-MM-dd"); //YYYY-MM-dd
            //hash it again
            data = GetBytes(hash);
            result = sha.ComputeHash(data);
            //back to string
            //hash = System.Text.Encoding.Default.GetString(result);
            hash = System.Text.Encoding.UTF8.GetString(result);
            //hash = BitConverter.ToString(result);
            return hash;
        }
      

        private void btnGetAllGames_Click(object sender, EventArgs e)
        {
            string host = AUTH_POST_URL;
            //build request
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/allgames", host));
            //set http method
            request.Method = "POST";
            //content type
            request.ContentType = "text/html";
            //build json
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\"}";
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(post_json);
            //write to request body
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = request.GetResponse().GetResponseStream();
            //read response body
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            txtResponse.Text = response_json;
        }

        private void btnGetAllUsers_Click(object sender, EventArgs e)
        {
            string host = AUTH_POST_URL;
            //build request
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/allusers", host));
            //set http method
            request.Method = "POST";
            //content type
            request.ContentType = "text/html";
            //build json
       
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token +  "\"}";
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(post_json);
            //write to request body
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = request.GetResponse().GetResponseStream();
            //read response body
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            txtResponse.Text = response_json;
        }

        private void processResponse(string response_json)
        {
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response_json);
            var user = data.user;
            user_id = user.user_id;

            authentication_token = user.authentication_token;

        }

        private void btnGetUser_Click(object sender, EventArgs e)
        {
            string host = AUTH_POST_URL;
            //build request
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/getuser", host));
            //set http method
            request.Method = "POST";
            //content type
            request.ContentType = "text/html";
            //build json
            string post_json = "{\"user_id\":\"" + user_id.ToString() + "\", \"authentication_token\":\"" + authentication_token + "\", \"query_user_id\":\"1\"}";
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(post_json);
            //write to request body
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = request.GetResponse().GetResponseStream();
            //read response body
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            txtResponse.Text = response_json;
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            string host = AUTH_POST_URL;
            //build request
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format("{0}/auth", host));
            //set http method
            request.Method = "POST";
            //content type
            request.ContentType = "text/html";
            //build json
            string hash = createPasswordHash("test1");
            string post_json = "{\"username\":\"test1\", \"hash\":\"" + hash + "\"}";
            //encode json
            byte[] buffer = Encoding.UTF8.GetBytes(post_json);
            //write to request body
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            //get response
            Stream response = request.GetResponse().GetResponseStream();
            //read response body
            StreamReader response_reader = new StreamReader(response);
            string response_json = response_reader.ReadToEnd();
            txtResponse.Text = response_json;
            processResponse(response_json);
        }
    }
}
