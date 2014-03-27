namespace TestJSON
{
    partial class frmTestJSON
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
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.btnAuth = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnHash = new System.Windows.Forms.Button();
            this.lblHash = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbTarget = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCreateGame = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.chkPrivate = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPlayers = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtWinLimit = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtUserLimit = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetAllGames
            // 
            this.btnGetAllGames.Location = new System.Drawing.Point(64, 489);
            this.btnGetAllGames.Name = "btnGetAllGames";
            this.btnGetAllGames.Size = new System.Drawing.Size(75, 23);
            this.btnGetAllGames.TabIndex = 0;
            this.btnGetAllGames.Text = "getallgames";
            this.btnGetAllGames.UseVisualStyleBackColor = true;
            this.btnGetAllGames.Click += new System.EventHandler(this.btnGetAllGames_Click);
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(376, 210);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(381, 255);
            this.txtResponse.TabIndex = 1;
            // 
            // btnGetAllUsers
            // 
            this.btnGetAllUsers.Location = new System.Drawing.Point(163, 489);
            this.btnGetAllUsers.Name = "btnGetAllUsers";
            this.btnGetAllUsers.Size = new System.Drawing.Size(75, 23);
            this.btnGetAllUsers.TabIndex = 2;
            this.btnGetAllUsers.Text = "getallusers";
            this.btnGetAllUsers.UseVisualStyleBackColor = true;
            this.btnGetAllUsers.Click += new System.EventHandler(this.btnGetAllUsers_Click);
            // 
            // btnGetUser
            // 
            this.btnGetUser.Location = new System.Drawing.Point(258, 489);
            this.btnGetUser.Name = "btnGetUser";
            this.btnGetUser.Size = new System.Drawing.Size(75, 23);
            this.btnGetUser.TabIndex = 3;
            this.btnGetUser.Text = "getuser";
            this.btnGetUser.UseVisualStyleBackColor = true;
            this.btnGetUser.Click += new System.EventHandler(this.btnGetUser_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(33, 210);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.Size = new System.Drawing.Size(334, 255);
            this.txtRequest.TabIndex = 6;
            // 
            // btnAuth
            // 
            this.btnAuth.Location = new System.Drawing.Point(72, 68);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(75, 23);
            this.btnAuth.TabIndex = 5;
            this.btnAuth.Text = "Auth";
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(90, 27);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(100, 20);
            this.txtUserName.TabIndex = 7;
            this.txtUserName.Text = "test1";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(271, 27);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.Text = "test1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Password";
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.Location = new System.Drawing.Point(213, 68);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(75, 23);
            this.btnCreateUser.TabIndex = 11;
            this.btnCreateUser.Text = "Create User";
            this.btnCreateUser.UseVisualStyleBackColor = true;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(394, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(455, 31);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(100, 20);
            this.txtEmail.TabIndex = 12;
            this.txtEmail.Text = "test@test.com";
            // 
            // btnHash
            // 
            this.btnHash.Location = new System.Drawing.Point(395, 489);
            this.btnHash.Name = "btnHash";
            this.btnHash.Size = new System.Drawing.Size(144, 23);
            this.btnHash.TabIndex = 14;
            this.btnHash.Text = "Test Auth Hash";
            this.btnHash.UseVisualStyleBackColor = true;
            this.btnHash.Click += new System.EventHandler(this.btnHash_Click);
            // 
            // lblHash
            // 
            this.lblHash.AutoSize = true;
            this.lblHash.Location = new System.Drawing.Point(573, 494);
            this.lblHash.Name = "lblHash";
            this.lblHash.Size = new System.Drawing.Size(103, 13);
            this.lblHash.TabIndex = 15;
            this.lblHash.Text = "AUTH HASH HERE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(392, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Response";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Request";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.btnCreateUser);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnAuth);
            this.groupBox1.Location = new System.Drawing.Point(28, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(725, 115);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User";
            // 
            // cbTarget
            // 
            this.cbTarget.FormattingEnabled = true;
            this.cbTarget.Items.AddRange(new object[] {
            "Development",
            "Production"});
            this.cbTarget.Location = new System.Drawing.Point(64, 13);
            this.cbTarget.Name = "cbTarget";
            this.cbTarget.Size = new System.Drawing.Size(121, 21);
            this.cbTarget.TabIndex = 19;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtUserLimit);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtWinLimit);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtSpeed);
            this.groupBox2.Controls.Add(this.txtPlayers);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.chkPrivate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtDescription);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.btnCreateGame);
            this.groupBox2.Location = new System.Drawing.Point(33, 537);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(720, 185);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Game";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnCreateGame
            // 
            this.btnCreateGame.Location = new System.Drawing.Point(31, 145);
            this.btnCreateGame.Name = "btnCreateGame";
            this.btnCreateGame.Size = new System.Drawing.Size(75, 23);
            this.btnCreateGame.TabIndex = 0;
            this.btnCreateGame.Text = "Create Game";
            this.btnCreateGame.UseVisualStyleBackColor = true;
            this.btnCreateGame.Click += new System.EventHandler(this.btnCreateGame_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(85, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 20);
            this.txtName.TabIndex = 1;
            this.txtName.Text = "Game Test A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(251, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(321, 32);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(203, 20);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.Text = "This is a description";
            // 
            // chkPrivate
            // 
            this.chkPrivate.AutoSize = true;
            this.chkPrivate.Location = new System.Drawing.Point(83, 57);
            this.chkPrivate.Name = "chkPrivate";
            this.chkPrivate.Size = new System.Drawing.Size(59, 17);
            this.chkPrivate.TabIndex = 5;
            this.chkPrivate.Text = "Private";
            this.chkPrivate.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(262, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Player IDs";
            // 
            // txtPlayers
            // 
            this.txtPlayers.Location = new System.Drawing.Point(321, 58);
            this.txtPlayers.Name = "txtPlayers";
            this.txtPlayers.Size = new System.Drawing.Size(150, 20);
            this.txtPlayers.TabIndex = 7;
            this.txtPlayers.Text = "2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(273, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Speed";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(321, 85);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(150, 20);
            this.txtSpeed.TabIndex = 8;
            this.txtSpeed.Text = "5";
            this.txtSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(35, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Win Limit";
            // 
            // txtWinLimit
            // 
            this.txtWinLimit.Location = new System.Drawing.Point(90, 85);
            this.txtWinLimit.Name = "txtWinLimit";
            this.txtWinLimit.Size = new System.Drawing.Size(150, 20);
            this.txtWinLimit.TabIndex = 10;
            this.txtWinLimit.Text = "3";
            this.txtWinLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(33, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "User Limit";
            // 
            // txtUserLimit
            // 
            this.txtUserLimit.Location = new System.Drawing.Point(88, 111);
            this.txtUserLimit.Name = "txtUserLimit";
            this.txtUserLimit.Size = new System.Drawing.Size(150, 20);
            this.txtUserLimit.TabIndex = 12;
            this.txtUserLimit.Text = "3";
            this.txtUserLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmTestJSON
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 807);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbTarget);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblHash);
            this.Controls.Add(this.btnHash);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.btnGetUser);
            this.Controls.Add(this.btnGetAllUsers);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.btnGetAllGames);
            this.Name = "frmTestJSON";
            this.Text = "King Bingo JSON Tester";
            this.Load += new System.EventHandler(this.frmTestJSON_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetAllGames;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Button btnGetAllUsers;
        private System.Windows.Forms.Button btnGetUser;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.Button btnAuth;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnHash;
        private System.Windows.Forms.Label lblHash;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbTarget;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkPrivate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnCreateGame;
        private System.Windows.Forms.TextBox txtPlayers;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtUserLimit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtWinLimit;
    }
}

