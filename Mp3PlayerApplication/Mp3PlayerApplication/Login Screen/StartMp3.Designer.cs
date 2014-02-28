namespace Mp3.Login_Screen

{
    partial class StartMp3
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
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.addUserButton = new System.Windows.Forms.Button();
            this.userLoginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // userComboBox
            // 
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(16, 34);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(173, 21);
            this.userComboBox.TabIndex = 1;
            // 
            // addUserButton
            // 
            this.addUserButton.Location = new System.Drawing.Point(16, 90);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(173, 23);
            this.addUserButton.TabIndex = 2;
            this.addUserButton.Text = "Add User";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // userLoginButton
            // 
            this.userLoginButton.Location = new System.Drawing.Point(16, 61);
            this.userLoginButton.Name = "userLoginButton";
            this.userLoginButton.Size = new System.Drawing.Size(173, 23);
            this.userLoginButton.TabIndex = 3;
            this.userLoginButton.Text = "User Login";
            this.userLoginButton.UseVisualStyleBackColor = true;
            this.userLoginButton.Click += new System.EventHandler(this.userLoginButton_Click);
            // 
            // StartMp3
            // 
            this.AcceptButton = this.userLoginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 150);
            this.Controls.Add(this.userLoginButton);
            this.Controls.Add(this.addUserButton);
            this.Controls.Add(this.userComboBox);
            this.Name = "StartMp3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User";
            this.Load += new System.EventHandler(this.StartMp3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.Button userLoginButton;
    }
}