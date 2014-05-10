namespace TestJSON
{
    partial class frmDebug
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
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.btnLint = new System.Windows.Forms.Button();
            this.btnRequestCopy = new System.Windows.Forms.Button();
            this.btnResponseCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(27, 71);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequest.Size = new System.Drawing.Size(239, 295);
            this.txtRequest.TabIndex = 0;
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(305, 71);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponse.Size = new System.Drawing.Size(244, 295);
            this.txtResponse.TabIndex = 1;
            // 
            // btnLint
            // 
            this.btnLint.Location = new System.Drawing.Point(442, 12);
            this.btnLint.Name = "btnLint";
            this.btnLint.Size = new System.Drawing.Size(107, 23);
            this.btnLint.TabIndex = 2;
            this.btnLint.Text = "JSON Lint";
            this.btnLint.UseVisualStyleBackColor = true;
            this.btnLint.Click += new System.EventHandler(this.btnLint_Click);
            // 
            // btnRequestCopy
            // 
            this.btnRequestCopy.Location = new System.Drawing.Point(27, 42);
            this.btnRequestCopy.Name = "btnRequestCopy";
            this.btnRequestCopy.Size = new System.Drawing.Size(133, 23);
            this.btnRequestCopy.TabIndex = 3;
            this.btnRequestCopy.Text = "Copy to Clipboard";
            this.btnRequestCopy.UseVisualStyleBackColor = true;
            this.btnRequestCopy.Click += new System.EventHandler(this.btnRequestCopy_Click);
            // 
            // btnResponseCopy
            // 
            this.btnResponseCopy.Location = new System.Drawing.Point(305, 42);
            this.btnResponseCopy.Name = "btnResponseCopy";
            this.btnResponseCopy.Size = new System.Drawing.Size(133, 23);
            this.btnResponseCopy.TabIndex = 4;
            this.btnResponseCopy.Text = "Copy to Clipboard";
            this.btnResponseCopy.UseVisualStyleBackColor = true;
            this.btnResponseCopy.Click += new System.EventHandler(this.btnResponseCopy_Click);
            // 
            // frmDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 389);
            this.Controls.Add(this.btnResponseCopy);
            this.Controls.Add(this.btnRequestCopy);
            this.Controls.Add(this.btnLint);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.txtRequest);
            this.Name = "frmDebug";
            this.Text = "JSON Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtRequest;
        public System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Button btnLint;
        private System.Windows.Forms.Button btnRequestCopy;
        private System.Windows.Forms.Button btnResponseCopy;
    }
}