namespace Mp3.Mp3_Player_and_Controls
{
    partial class AddPlaylist
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
            this.label1 = new System.Windows.Forms.Label();
            this.playlistTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.playlistGroupComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.infoPictureBox = new System.Windows.Forms.PictureBox();
            this.createGroupButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.infoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Playlist Name:";
            // 
            // playlistTextBox
            // 
            this.playlistTextBox.Location = new System.Drawing.Point(133, 28);
            this.playlistTextBox.Name = "playlistTextBox";
            this.playlistTextBox.Size = new System.Drawing.Size(340, 20);
            this.playlistTextBox.TabIndex = 1;
            this.playlistTextBox.TextChanged += new System.EventHandler(this.playlistTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Place in Playlist Group:";
            // 
            // playlistGroupComboBox
            // 
            this.playlistGroupComboBox.FormattingEnabled = true;
            this.playlistGroupComboBox.Location = new System.Drawing.Point(133, 6);
            this.playlistGroupComboBox.Name = "playlistGroupComboBox";
            this.playlistGroupComboBox.Size = new System.Drawing.Size(340, 21);
            this.playlistGroupComboBox.TabIndex = 3;
            this.playlistGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.playlistGroupComboBox_SelectedIndexChanged);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(133, 54);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(167, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(306, 54);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(167, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // infoPictureBox
            // 
            this.infoPictureBox.BackgroundImage = global::Mp3.Properties.Resources.InformationIcon;
            this.infoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.infoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoPictureBox.Location = new System.Drawing.Point(479, 47);
            this.infoPictureBox.Name = "infoPictureBox";
            this.infoPictureBox.Size = new System.Drawing.Size(32, 30);
            this.infoPictureBox.TabIndex = 6;
            this.infoPictureBox.TabStop = false;
            this.infoPictureBox.Click += new System.EventHandler(this.infoPictureBox_Click);
            // 
            // createGroupButton
            // 
            this.createGroupButton.Location = new System.Drawing.Point(479, 4);
            this.createGroupButton.Name = "createGroupButton";
            this.createGroupButton.Size = new System.Drawing.Size(90, 23);
            this.createGroupButton.TabIndex = 7;
            this.createGroupButton.Text = "Create Group";
            this.createGroupButton.UseVisualStyleBackColor = true;
            this.createGroupButton.Click += new System.EventHandler(this.createGroupButton_Click);
            // 
            // AddPlaylist
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(581, 85);
            this.Controls.Add(this.createGroupButton);
            this.Controls.Add(this.infoPictureBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.playlistGroupComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.playlistTextBox);
            this.Controls.Add(this.label1);
            this.Name = "AddPlaylist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddPlaylist";
            ((System.ComponentModel.ISupportInitialize)(this.infoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox playlistTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox playlistGroupComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.PictureBox infoPictureBox;
        private System.Windows.Forms.Button createGroupButton;
    }
}