using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Mp3.Containers;
using Mp3.Details;

namespace Mp3.Functions
{
    class Contains
    {
        public static bool songListContains(Mp3_Container container, SongDetails song)
        {
            bool isFound = false;
            foreach (DataGridViewRow row in container.gui.songList.Rows)
            {
                if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                    row.Cells[3].Value == null && row.Cells[4].Value == null && row.Cells[5].Value == null &&
                    row.Cells[6].Value == null && row.Cells[7].Value == null && row.Cells[8].Value == null)
                {
                    break;
                }
                if (row.Cells[1].Value.ToString() == song.SongName && row.Cells[2].Value.ToString() == song.Length && row.Cells[3].Value.ToString() == song.Artist && row.Cells[4].Value.ToString() == song.Genre)
                {
                    isFound = true;
                    break;
                }
            }

            return isFound;
        }

        public static bool xmlContainsSong(Mp3_Container container, SongDetails song)
        {
            bool isFound = false;
            if (container.xmlFile.xml.ChildNodes[1].ChildNodes[0].ChildNodes[0].HasChildNodes)
            {
                foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0].ChildNodes[0])
                {
                    if (xn.ChildNodes[9].InnerText.Equals(song.Path))
                    {
                        isFound = true;
                        break;
                    }
                }
            }
            return isFound;
        }

        public static bool xmlContainsPlaylistGroup(Mp3_Container container, String playlist)
        {
            bool isFound = false;
            if (container.xmlFile.xml.ChildNodes[1].ChildNodes[0].HasChildNodes)
            {
                foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0])
                {
                    if (xn.Attributes["name"].Value == playlist)
                    {
                        isFound = true;
                        break;
                    }
                }
            }
            return isFound;
        }

        public static bool xmlContainsPlaylist(Mp3_Container container, String playlist)
        {
            bool isFound = false;
            if (container.xmlFile.xml.ChildNodes[1].ChildNodes[0].HasChildNodes)
            {
                foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0])
                {
                    if (xn.HasChildNodes)
                    {
                        foreach (XmlNode xcn in xn)
                        {
                            if (xcn.Name.Equals("Song"))
                            {
                                break;
                            }
                            if (xcn.Attributes["name"].Value == playlist)
                            {
                                isFound = true;
                                break;
                            }
                        }
                    }
                    if (isFound)
                    {
                        break;
                    }
                }
            }
            return isFound;
        }

        public static bool isPlaylistContained(Mp3_Container container, string playlist)
        {
            foreach (TreeNode xn in container.gui.playlistList.Nodes)
            {
                if (xn.Text.Equals(playlist))
                {
                    string name = "";
                    if (xn.Nodes.Count == 0)
                    {
                        name = "Playlist Name";
                    }
                    else
                    {
                        name = "Playlist Group Name";
                    }
                    MessageBox.Show(playlist + " has already been added as a " + name,
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return true;
                }
                if (xn.Nodes.Count > 0)
                {
                    foreach (TreeNode xcn in xn.Nodes)
                    {
                        if (xcn.Text.Equals(playlist))
                        {
                            MessageBox.Show(playlist + " has already been added as a Playlist Name",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
