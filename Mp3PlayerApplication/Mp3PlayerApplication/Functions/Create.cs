using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using Mp3.Containers;
using Mp3.Details;

namespace Mp3.Functions
{
    class Create
    {
        public static void createSong(Mp3_Container container, String file)
        {
            TagLib.File f = TagLib.File.Create(file);
            SongDetails song;
            if (f.Tag.Performers.Length == 0)
            {
                song = new SongDetails(f.Tag.Title, f.Tag.AlbumArtists[0], null,
                                                   f.Tag.Album, f.Tag.Genres[0], f.Properties.Duration.ToString(),
                                                   0, 0, file, f.Tag.Track.ToString(), f.Tag.Year.ToString());
            }
            else
            {
                song = new SongDetails(f.Tag.Title, f.Tag.AlbumArtists[0], f.Tag.
                    Performers,
                                                   f.Tag.Album, f.Tag.Genres[0], f.Properties.Duration.ToString(),
                                                   0, 0, file, f.Tag.Track.ToString(), f.Tag.Year.ToString());
            }

            if (!Contains.xmlContainsSong(container, song))
            {
                container.gui.songList.Rows.Insert(0, 0, song.SongName, song.Length, song.Artist, song.Album, song.Track, song.Year, song.Genre, song.Plays, song.Rating, song.Path);
                container.songlists.masterSongList.Add(song);
                container.songlists.currentPlaylist.Add(song);

                XmlElement parent = container.xmlFile.xml.CreateElement("Song");

                XmlElement userElem = container.xmlFile.xml.CreateElement("name");
                XmlText text = container.xmlFile.xml.CreateTextNode(song.SongName);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("artist");
                text = container.xmlFile.xml.CreateTextNode(song.Artist);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("album");
                text = container.xmlFile.xml.CreateTextNode(song.Album);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("track");
                text = container.xmlFile.xml.CreateTextNode(song.Track);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("year");
                text = container.xmlFile.xml.CreateTextNode(song.Year);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("genre");
                text = container.xmlFile.xml.CreateTextNode(song.Genre);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("length");
                text = container.xmlFile.xml.CreateTextNode(song.Length);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("rating");
                text = container.xmlFile.xml.CreateTextNode(song.Rating.ToString());
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("plays");
                text = container.xmlFile.xml.CreateTextNode(song.Plays.ToString());
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = container.xmlFile.xml.CreateElement("path");
                text = container.xmlFile.xml.CreateTextNode(song.Path);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                container.xmlFile.xml.ChildNodes[1].ChildNodes[0].ChildNodes[0].AppendChild(parent);

                XmlTextWriter writer = new XmlTextWriter(container.xmlFile.userPath, null);
                writer.Formatting = Formatting.Indented;
                container.xmlFile.xml.Save(writer);
                writer.Close();
            }
            else
            {
                MessageBox.Show("The Song" + '\n' + file + '\n' + "has already been added", "Song has been added");
            }
            GuiControl.sort(container, container.trackers.lastSortParam);
        }

        public static SongDetails createSelectedSong(Mp3_Container container)
        {
            SongDetails song = new SongDetails(container.gui.songList.SelectedRows[0].Cells[1].Value.ToString(), 
                container.gui.songList.SelectedRows[0].Cells[3].Value.ToString(), null,
                container.gui.songList.SelectedRows[0].Cells[4].Value.ToString(), container.gui.songList.SelectedRows[0].Cells[7].Value.ToString(), 
                container.gui.songList.SelectedRows[0].Cells[2].Value.ToString(),
                Convert.ToInt32(container.gui.songList.SelectedRows[0].Cells[9].Value.ToString()), 
                Convert.ToInt64(container.gui.songList.SelectedRows[0].Cells[8].Value.ToString()), container.gui.songList.SelectedRows[0].Cells[10].Value.ToString(),
                container.gui.songList.SelectedRows[0].Cells[5].Value.ToString(), container.gui.songList.SelectedRows[0].Cells[6].Value.ToString());

            return song;
        }

        public static SongDetails createAsong(Mp3_Container container, DataGridViewRow row)
        {
            SongDetails song = new SongDetails(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString(), null,
                     row.Cells[4].Value.ToString(), row.Cells[7].Value.ToString(), row.Cells[2].Value.ToString(),
                     int.Parse(row.Cells[9].Value.ToString()), long.Parse(row.Cells[8].Value.ToString()),
                     row.Cells[10].Value.ToString(), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString());

            return song;
        }
    }
}
