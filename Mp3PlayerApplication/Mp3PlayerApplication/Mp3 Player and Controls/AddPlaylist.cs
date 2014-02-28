using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mp3.Mp3_Player_and_Controls
{
    public partial class AddPlaylist : Form
    {
        private string playlist = "";
        private string playlistGroup = "";

        public AddPlaylist(List<string> comboBoxItems)
        {
            InitializeComponent();
            playlistGroupComboBox.Items.Clear();
            foreach (string item in comboBoxItems)
            {
                playlistGroupComboBox.Items.Add(item);
            }
        }

        private void infoPictureBox_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To add playlist to the parent list leave the Playlist Group field blank.\n" + 
                            "Filling in this field will my the created playlist a child of this parent group.", 
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void playlistTextBox_TextChanged(object sender, EventArgs e)
        {
            playlist = playlistTextBox.Text;
        }

        private void playlistGroupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            playlistGroup = playlistGroupComboBox.SelectedItem.ToString();
        }

        public string getPlaylist()
        {
            return playlist;
        }

        public string getPlaylistGroup()
        {
            return playlistGroup;
        }

        private void createGroupButton_Click(object sender, EventArgs e)
        {
            AddPlaylistGroup toAdd = new AddPlaylistGroup();
            if (toAdd.ShowDialog() == DialogResult.OK)
            {
                playlistGroupComboBox.Items.Add(toAdd.getPlaylistName());
            }
            toAdd.Dispose();
        }
    }
}
