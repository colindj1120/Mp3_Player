using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Mp3.Mp3_Player_and_Controls;

namespace Mp3.Login_Screen
{
    public partial class StartMp3 : Form
    {
        private XmlDocument xml = new XmlDocument();
        private string currentDomain = AppDomain.CurrentDomain.BaseDirectory;
        private const string userFileName = "users.xml";

        public StartMp3()
        {
            InitializeComponent();
        }

        private void StartMp3_Load(object sender, EventArgs e)
        {
            loadXML();
        }

        private void loadXML()
        {
            if (File.Exists(currentDomain + '/' + userFileName))
            {
                xml.Load(currentDomain + '/' + userFileName);
                foreach (XmlNode xn in xml.ChildNodes[1])
                {
                    userComboBox.Items.Add(xn.InnerText);
                }
            }
            else
            {
                xml.LoadXml("<users><name></name></users>");
                XmlTextWriter writer = new XmlTextWriter(userFileName, null);
                writer.Formatting = Formatting.Indented;
                xml.Save(writer);
            }
        }

        private void saveXML()
        {
            foreach(object item in userComboBox.Items)
            {
                if (!xmlContains(item.ToString()))
                {
                    XmlElement userElem = xml.CreateElement("name");
                    XmlText text = xml.CreateTextNode(item.ToString());
                    xml.DocumentElement.AppendChild(userElem);
                    xml.DocumentElement.LastChild.AppendChild(text);
                    XmlTextWriter writer = new XmlTextWriter(userFileName, null);
                    writer.Formatting = Formatting.Indented;
                    xml.Save(writer);
                }
            }
        }

        private bool xmlContains(string item)
        {
            bool isFound = false;

            foreach (XmlNode xn in xml.ChildNodes[1])
            {
                if (xn.InnerText.Equals(item))
                {
                    isFound = true;
                    break;
                }
            }
            return isFound;
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            AddUser add = new AddUser();
            add.ShowDialog();
            if (add.User.Length != 0)
            {
                userComboBox.Items.Add(add.User);
            }
            saveXML();
        }

        private void userLoginButton_Click(object sender, EventArgs e)
        {
            if (userComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a user.");
                return;
            }
            else
            {
                this.Visible = false;
                Mp3PlayerForm mp3 = new Mp3PlayerForm(userComboBox.SelectedItem.ToString());
                mp3.Text = userComboBox.SelectedItem.ToString() + "'s Mp3 Player";
                mp3.ShowDialog();
                this.Dispose();

            }
        }
    }
}
