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
    public partial class AddPlaylistGroup : Form
    {
        private string playlistName = "";

        public AddPlaylistGroup()
        {
            InitializeComponent();
        }

        public string getPlaylistName()
        {
            return playlistName;
        }

        private void playlistTextBox_TextChanged(object sender, EventArgs e)
        {
            playlistName = playlistTextBox.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
