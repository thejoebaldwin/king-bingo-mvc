namespace TestJSON
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetAllGames = new System.Windows.Forms.Button();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.btnGetAllUsers = new System.Windows.Forms.Button();
            this.btnGetUser = new System.Windows.Forms.Button();
            this.txtGetUser = new System.Windows.Forms.TextBox();
            this.txtAuth = new System.Windows.Forms.TextBox();
            this.btnAuth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetAllGames
            // 
            this.btnGetAllGames.Location = new System.Drawing.Point(100, 78);
            this.btnGetAllGames.Name = "btnGetAllGames";
            this.btnGetAllGames.Size = new System.Drawing.Size(75, 23);
            this.btnGetAllGames.TabIndex = 0;
            this.btnGetAllGames.Text = "getallgames";
            this.btnGetAllGames.UseVisualStyleBackColor = true;
            this.btnGetAllGames.Click += new System.EventHandler(this.btnGetAllGames_Click);
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(62, 218);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(430, 255);
            this.txtResponse.TabIndex = 1;
            // 
            // btnGetAllUsers
            // 
            this.btnGetAllUsers.Location = new System.Drawing.Point(295, 78);
            this.btnGetAllUsers.Name = "btnGetAllUsers";
            this.btnGetAllUsers.Size = new System.Drawing.Size(75, 23);
            this.btnGetAllUsers.TabIndex = 2;
            this.btnGetAllUsers.Text = "getallusers";
            this.btnGetAllUsers.UseVisualStyleBackColor = true;
            this.btnGetAllUsers.Click += new System.EventHandler(this.btnGetAllUsers_Click);
            // 
            // btnGetUser
            // 
            this.btnGetUser.Location = new System.Drawing.Point(601, 170);
            this.btnGetUser.Name = "btnGetUser";
            this.btnGetUser.Size = new System.Drawing.Size(75, 23);
            this.btnGetUser.TabIndex = 3;
            this.btnGetUser.Text = "get user";
            this.btnGetUser.UseVisualStyleBackColor = true;
            this.btnGetUser.Click += new System.EventHandler(this.btnGetUser_Click);
            // 
            // txtGetUser
            // 
            this.txtGetUser.Location = new System.Drawing.Point(489, 11);
            this.txtGetUser.Multiline = true;
            this.txtGetUser.Name = "txtGetUser";
            this.txtGetUser.Size = new System.Drawing.Size(232, 142);
            this.txtGetUser.TabIndex = 4;
            this.txtGetUser.Text = "{\"user_id\":\"1\"}";
            // 
            // txtAuth
            // 
            this.txtAuth.Location = new System.Drawing.Point(509, 218);
            this.txtAuth.Multiline = true;
            this.txtAuth.Name = "txtAuth";
            this.txtAuth.Size = new System.Drawing.Size(232, 142);
            this.txtAuth.TabIndex = 6;
            // 
            // btnAuth
            // 
            this.btnAuth.Location = new System.Drawing.Point(621, 377);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(75, 23);
            this.btnAuth.TabIndex = 5;
            this.btnAuth.Text = "Auth";
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 608);
            this.Controls.Add(this.txtAuth);
            this.Controls.Add(this.btnAuth);
            this.Controls.Add(this.txtGetUser);
            this.Controls.Add(this.btnGetUser);
            this.Controls.Add(this.btnGetAllUsers);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.btnGetAllGames);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetAllGames;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Button btnGetAllUsers;
        private System.Windows.Forms.Button btnGetUser;
        private System.Windows.Forms.TextBox txtGetUser;
        private System.Windows.Forms.TextBox txtAuth;
        private System.Windows.Forms.Button btnAuth;
    }
}

