using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using Mp3.Containers;
using Mp3.Comparers;
using Mp3.Details;

namespace Mp3.Functions
{
    static class Load_Save
    {
        public static void loadUser(Mp3_Container container)
        {
            if (File.Exists(container.xmlFile.userPath))
            {
                container.xmlFile.xml.Load(container.xmlFile.userPath);
                if (container.xmlFile.xml.ChildNodes[1].ChildNodes[0].HasChildNodes)
                {
                    container.gui.playlistList.Nodes.Add("Master Library");
                    foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0].ChildNodes[0])
                    {
                        SongDetails song = new SongDetails(xn.ChildNodes[0].InnerText, xn.ChildNodes[1].InnerText, null,
                                                           xn.ChildNodes[2].InnerText, xn.ChildNodes[5].InnerText, xn.ChildNodes[6].InnerText,
                                                           int.Parse(xn.ChildNodes[7].InnerText), long.Parse(xn.ChildNodes[8].InnerText),
                                                           xn.ChildNodes[9].InnerText, xn.ChildNodes[3].InnerText, xn.ChildNodes[4].InnerText);

                        if (!Contains.songListContains(container, song))
                        {
                            container.songlists.masterSongList.Add(song);
                            container.songlists.currentPlaylist.Add(song);
                            container.gui.songList.Rows.Insert(0, 0, song.SongName, song.Length, song.Artist, song.Album, song.Track, song.Year, song.Genre, song.Plays, song.Rating, song.Path);
                        }
                    }

                    int count = 0;
                    foreach (XmlNode xn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0])
                    {
                        if (count == 0)
                        {
                            count++;
                            continue;
                        }
                        container.gui.playlistList.Nodes.Add(xn.Attributes["name"].Value);
                        if (xn.HasChildNodes)
                        {
                            List<SongDetails> tempSongList = new List<SongDetails>();
                            foreach (XmlNode xcn in xn)
                            {
                                container.gui.playlistList.Nodes[count].Nodes.Add(xcn.Attributes["name"].Value);
                                foreach (XmlNode xccn in xcn)
                                {
                                    SongDetails song = new SongDetails(xccn.ChildNodes[0].InnerText, xccn.ChildNodes[1].InnerText, null,
                                                               xccn.ChildNodes[2].InnerText, xccn.ChildNodes[5].InnerText, xccn.ChildNodes[6].InnerText,
                                                               int.Parse(xccn.ChildNodes[7].InnerText), long.Parse(xccn.ChildNodes[8].InnerText),
                                                               xccn.ChildNodes[9].InnerText, xccn.ChildNodes[3].InnerText, xccn.ChildNodes[4].InnerText);
                                    tempSongList.Add(song);
                                }
                                container.songlists.playlistDictionary.Add(xcn.Attributes["name"].Value, tempSongList);
                            }
                        }

                        count++;
                    }
                }

                container.songlists.playlistDictionary["Master Library"] = container.songlists.masterSongList;
            }
            else
            {
                container.xmlFile.xml.LoadXml("<UserMusic><PlaylistsGroup><PlaylistGroupNames name=\"Master Library\"></PlaylistGroupNames></PlaylistsGroup></UserMusic>");
                if (!Directory.Exists(container.xmlFile.userDirectory))
                {
                    Directory.CreateDirectory(container.xmlFile.userDirectory);
                }
                XmlTextWriter writer = new XmlTextWriter(container.xmlFile.userPath, null);
                writer.Formatting = Formatting.Indented;
                container.xmlFile.xml.Save(writer);
            }
            container.booleans.columnWidthChanged = false;
            container.labels.playlistLabel.Text = "Master Library";
            container.gui.songList.Sort(new NameComparer(SortOrder.Ascending));
            container.gui.sortOptions.nameAscending.Checked = true;
        }

        public static void savePlaylistGroupToXml(Mp3_Container container, string playlistName)
        {
            container.gui.playlistList.BeginUpdate();
            container.gui.playlistList.Nodes.Add(playlistName);
            container.gui.playlistList.EndUpdate();

            if (!Contains.xmlContainsPlaylistGroup(container, playlistName))
            {
                XmlElement parent = container.xmlFile.xml.CreateElement("PlaylistGroupNames");
                parent.SetAttribute("name", playlistName);
                container.xmlFile.xml.ChildNodes[1].ChildNodes[0].AppendChild(parent);

                XmlTextWriter writer = new XmlTextWriter(container.xmlFile.userPath, null);
                writer.Formatting = Formatting.Indented;
                container.xmlFile.xml.Save(writer);
                writer.Close();
            }
        }

        public static void savePlaylistToXml(Mp3_Container container, string playlist, string playlistGroup)
        {
            if (!Contains.xmlContainsPlaylist(container, playlist))
            {
                foreach (XmlNode xcn in container.xmlFile.xml.ChildNodes[1].ChildNodes[0])
                {
                    if (xcn.Attributes["name"].Value.Equals(playlistGroup))
                    {
                        XmlElement child = container.xmlFile.xml.CreateElement("Playlist");
                        child.SetAttribute("name", playlist);
                        xcn.AppendChild(child);

                        XmlTextWriter writer = new XmlTextWriter(container.xmlFile.userPath, null);
                        writer.Formatting = Formatting.Indented;
                        container.xmlFile.xml.Save(writer);
                        writer.Close();
                    }
                }
            }
        }
    }
}
