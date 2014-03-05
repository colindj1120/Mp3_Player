using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using System.Windows.Forms;
using System.Xml;
using Mp3.Containers;
using Mp3.Details;
using Mp3.Functions;

namespace Mp3.Functions
{
    class Add
    {
        public static bool addSubPlaylist(Mp3_Container container, string playlist, string playlistGroup)
        {
            bool found = false;
            foreach (TreeNode xn in container.gui.playlistList.Nodes)
            {
                if (xn.Text.Equals(playlistGroup))
                {
                    found = true;
                    xn.Nodes.Add(playlist);
                    Load_Save.savePlaylistToXml(container, playlist, playlistGroup);
                    List<SongDetails> songs = new List<SongDetails>();
                    container.songlists.playlistDictionary.Add(playlist, songs);
                }
            }
            return found;
        }

        public static void addPlaylistGroup(Mp3_Container container)
        {
            Mp3.Mp3_Player_and_Controls.AddPlaylistGroup toAdd = new Mp3.Mp3_Player_and_Controls.AddPlaylistGroup();
            if (toAdd.ShowDialog() == DialogResult.OK)
            {
                if (Contains.xmlContainsPlaylistGroup(container, toAdd.getPlaylistName()))
                {
                    toAdd.Dispose();
                    addPlaylistGroup(container);
                    return;
                }
                Load_Save.savePlaylistGroupToXml(container, toAdd.getPlaylistName());
            }
            toAdd.Dispose();
        }

        public static void addPlaylist(Mp3_Container container)
        {
            List<string> comboBoxItems = new List<string>();
            comboBoxItems.Add("");
            foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0])
            {
                if (!container.songlists.playlistDictionary.ContainsKey(xn.Attributes["name"].Value))
                {
                    comboBoxItems.Add(xn.Attributes["name"].Value);
                }
            }
            Mp3.Mp3_Player_and_Controls.AddPlaylist toAdd = new Mp3.Mp3_Player_and_Controls.AddPlaylist(comboBoxItems);
            if (toAdd.ShowDialog() == DialogResult.OK)
            {
                if (Contains.isPlaylistContained(container, toAdd.getPlaylist()))
                {
                    toAdd.Dispose();
                    addPlaylist(container);
                    return;
                }

                container.gui.playlistList.BeginUpdate();
                if (toAdd.getPlaylistGroup().Equals(""))
                {
                    if (!Contains.xmlContainsPlaylistGroup(container, toAdd.getPlaylist()))
                    {
                        Load_Save.savePlaylistGroupToXml(container, toAdd.getPlaylist());
                    }
                    List<SongDetails> songs = new List<SongDetails>();
                    container.songlists.playlistDictionary.Add(toAdd.getPlaylist(), songs);
                }
                else
                {
                    if (!addSubPlaylist(container, toAdd.getPlaylist(), toAdd.getPlaylistGroup()))
                    {
                        Load_Save.savePlaylistGroupToXml(container, toAdd.getPlaylistGroup());
                        if (!addSubPlaylist(container, toAdd.getPlaylist(), toAdd.getPlaylistGroup()))
                        {
                            MessageBox.Show("Error in adding Playlist to tree see system administrator",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
                container.gui.playlistList.EndUpdate();
            }
            toAdd.Dispose();
        }

        public static bool addSong(Mp3_Container container)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "Mp3 File|*.mp3";
                dlgOpen.InitialDirectory = "C:\\";
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    Create.createSong(container, dlgOpen.FileName);
                    return true;
                }
                return false;
            }
        }

        public static bool addFolder(Mp3_Container container)
        {
            using (FolderBrowserDialog dlgOpen = new FolderBrowserDialog())
            {
                dlgOpen.RootFolder = System.Environment.SpecialFolder.Desktop;
                List<string> songs = new List<string>();
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    Miscellaneous.ProcessDirectory(dlgOpen.SelectedPath, songs);
                    foreach (string fileName in songs)
                    {
                        Create.createSong(container, fileName);
                    }
                    return true;
                }
                return false;
            }
        }
    }
}
