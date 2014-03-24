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

namespace TestJSON
{
    public partial class Form1 : Form
    {
        const string AUTH_POST_URL = "http://localhost:22986/service/v0";

        public Form1()
        {
            InitializeComponent();
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
            string post_json = "";
            //encode json
            byte[] buffer = Encoding.ASCII.GetBytes(post_json);
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
            string post_json = "";
            //encode json
            byte[] buffer = Encoding.ASCII.GetBytes(post_json);
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
            string post_json = txtGetUser.Text;
            //encode json
            byte[] buffer = Encoding.ASCII.GetBytes(post_json);
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
    }
}
