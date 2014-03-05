using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mp3.Containers;
using Mp3.Comparers;
using Mp3.Details;

namespace Mp3.Functions
{
    class GuiControl
    {
        public static void changePlayPicture(Mp3_Container container)
        {
            if ((container.booleans.isPlaying && !container.booleans.isPaused))
            {
                container.buttons.play.BackgroundImage = Mp3.Properties.Resources.pause2;
            }
            else if (container.booleans.isPaused && !container.booleans.isPlaying || (!container.booleans.isPlaying && !container.booleans.isPaused))
            {
                container.buttons.play.BackgroundImage = Mp3.Properties.Resources.play2;
            }
        }

        public static void sort(Mp3_Container container, string value)
        {
            container.trackers.lastSortParam = value;
            switch (value)
            {
                case "Name":
                    if (container.gui.sortOptions.nameAscending.Checked)
                    {
                        container.gui.songList.Sort(new NameComparer(SortOrder.Ascending));
                    }
                    else if (container.gui.sortOptions.nameDescending.Checked)
                    {
                        container.gui.songList.Sort(new NameComparer(SortOrder.Descending));
                    }
                    break;
                case "Length":
                    if (container.gui.sortOptions.lengthAscending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Ascending, 2));
                    }
                    else if (container.gui.sortOptions.lengthDescending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Descending, 2));
                    }
                    break;
                case "Artist":
                    if (container.gui.sortOptions.artistAscending.Checked)
                    {
                        container.gui.songList.Sort(new ArtistComparer(SortOrder.Ascending));
                    }
                    else if (container.gui.sortOptions.artistDescending.Checked)
                    {
                        container.gui.songList.Sort(new ArtistComparer(SortOrder.Descending));
                    }
                    break;
                case "Album":
                    if (container.gui.sortOptions.albumAscending.Checked)
                    {
                        container.gui.songList.Sort(new AlbumComparer(SortOrder.Ascending));
                    }
                    else if (container.gui.sortOptions.albumDescending.Checked)
                    {
                        container.gui.songList.Sort(new AlbumComparer(SortOrder.Descending));
                    }
                    break;
                case "Year":
                    if (container.gui.sortOptions.yearAscending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Ascending, 6));
                    }
                    else if (container.gui.sortOptions.yearDescending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Descending, 6));
                    }
                    break;
                case "Genre":
                    if (container.gui.sortOptions.genreAscending.Checked)
                    {
                        container.gui.songList.Sort(new GenreComparer(SortOrder.Ascending));
                    }
                    else if (container.gui.sortOptions.genreDescending.Checked)
                    {
                        container.gui.songList.Sort(new GenreComparer(SortOrder.Descending));
                    }
                    break;
                case "Plays":
                    if (container.gui.sortOptions.playsAscending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Ascending, 8));
                    }
                    else if (container.gui.sortOptions.playsDescending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Descending, 8));
                    }
                    break;
                case "Rating":
                    if (container.gui.sortOptions.ratingAscending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Ascending, 9));
                    }
                    else if (container.gui.sortOptions.ratingDescending.Checked)
                    {
                        container.gui.songList.Sort(new PositionComparer(SortOrder.Descending, 9));
                    }
                    break;
            }
        }

        public static void fillLabels(Mp3_Container container)
        {
            if (container.songlists.nowPlaying.SongName.Contains("&"))
            {
                string[] songName = container.songlists.nowPlaying.SongName.Split('&');
                string name = "";
                if (songName.Length > 1)
                {
                    int count = 0;
                    foreach (string temp in songName)
                    {
                        count++;
                        if (count == songName.Length)
                        {
                            name += temp;
                        }
                        else
                        {
                            name += temp + "&&";
                        }
                    }
                }
                container.labels.songTitle.Text = name;
            }
            else
            {
                container.labels.songTitle.Text = container.songlists.nowPlaying.SongName;
            }

            if (container.songlists.nowPlaying.Artist.Contains("&"))
            {
                string[] artistName = container.songlists.nowPlaying.Artist.Split('&');
                string name = "";
                if (artistName.Length > 1)
                {
                    int count = 0;
                    foreach (string temp in artistName)
                    {
                        count++;
                        if (count == artistName.Length)
                        {
                            name += temp;
                        }
                        else
                        {
                            name += temp + "&&";
                        }
                    }
                }
                container.labels.arist.Text = name;
            }
            else
            {
                container.labels.arist.Text = container.songlists.nowPlaying.Artist;
            }

            if (container.songlists.nowPlaying.Album.Contains("&"))
            {
                string[] albumName = container.songlists.nowPlaying.Album.Split('&');
                string name = "";
                if (albumName.Length > 1)
                {
                    int count = 0;
                    foreach (string temp in albumName)
                    {
                        count++;
                        if (count == albumName.Length)
                        {
                            name += temp;
                        }
                        else
                        {
                            name += temp + "&&";
                        }
                    }
                }
                container.labels.album.Text = name;
            }
            else
            {
                container.labels.album.Text = container.songlists.nowPlaying.Album;
            }

            container.labels.genre.Text = container.songlists.nowPlaying.Genre;
        }

        public static void updateTimer(Mp3_Container container)
        {
            container.trackers.time++;
            container.player.progressBar.Value++;
            if (container.trackers.time.Equals(Miscellaneous.totalTime(container.songlists.nowPlaying.Length)))
            {
                SongControl.stop(container);
                if (container.gui.repeatAll.Checked && (container.trackers.nowPlayingRow == container.gui.songList.RowCount - 1))
                {
                    container.gui.songList.SelectedRows[0].Selected = false;
                    container.gui.songList.Rows[0].Selected = true;
                    
                }
                else if (container.gui.shuffle.Checked || container.gui.repeat.Checked) {}
                else
                {
                    container.trackers.nowPlayingRow++;
                    container.gui.songList.SelectedRows[0].Selected = false;
                    container.gui.songList.Rows[container.trackers.nowPlayingRow].Selected = true;
                }
                SongControl.setupPlay(container);
                return;
            }

            container.player.timeLabel.Text = getTimeLabel(container.trackers.time) + " / " + container.songlists.nowPlaying.Length;
        }

        public static string getTimeLabel(long inTime)
        {
            string time;
            if (inTime < (long)60)
            {
                string seconds = inTime.ToString();
                if (inTime < (long)10)
                {
                    time = "00:00:0" + seconds;
                }
                else
                {
                    time = "00:00:" + seconds;
                }
            }
            else if (inTime < (long)3600)
            {
                string mins = (inTime / (long)60).ToString();
                string seconds = (inTime % (long)60).ToString();
                if ((inTime / (long)60) < (long)10)
                {
                    if ((inTime % 60) < (long)10)
                    {
                        time = "00:0" + mins + ":0" + seconds;
                    }
                    else
                    {
                        time = "00:0" + mins + ":" + seconds;
                    }
                }
                else
                {
                    if ((inTime % 60) < (long)10)
                    {
                        time = "00:" + mins + ":0" + seconds;
                    }
                    else
                    {
                        time = "00:" + mins + ":" + seconds;
                    }
                }
            }
            else
            {
                string mins = (inTime % (long)60).ToString();
                string seconds = (inTime % (long)3600).ToString();
                string hours = (inTime / (long)3600).ToString();

                if ((inTime / (long)3600) < (long)10)
                {
                    if ((inTime % (long)60) < (long)10)
                    {
                        if ((inTime % (long)3600) < (long)10)
                        {
                            time = "0" + hours + ":0" + mins + ":0" + seconds;
                        }
                        else
                        {
                            time = "0" + hours + ":0" + mins + ":" + seconds;
                        }
                    }
                    else
                    {
                        if ((inTime % (long)3600) < (long)10)
                        {
                            time = "0" + hours + ":" + mins + ":0" + seconds;
                        }
                        else
                        {
                            time = "0" + hours + ":" + mins + ":" + seconds;
                        }
                    }
                }
                else
                {
                    if ((inTime % (long)60) < (long)10)
                    {
                        if ((inTime % (long)3600) < (long)10)
                        {
                            time = hours + ":0" + mins + ":0" + seconds;
                        }
                        else
                        {
                            time = hours + ":0" + mins + ":" + seconds;
                        }
                    }
                    else
                    {
                        if ((inTime % (long)3600) < (long)10)
                        {
                            time = hours + ":" + mins + ":0" + seconds;
                        }
                        else
                        {
                            time = hours + ":" + mins + ":" + seconds;
                        }
                    }
                }
            }

            return time;
        }

        public static void progressBarUpdate(Mp3_Container container, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                container.player.timer.Stop();
                decimal pos = 0M;
                pos = ((decimal)e.X / (decimal)container.player.progressBar.Width) * container.player.progressBar.Maximum;
                pos = Convert.ToInt32(pos);

                if (pos >= container.player.progressBar.Minimum && pos <= container.player.progressBar.Maximum)
                {
                    container.player.progressBar.Value = (int)pos;
                    container.trackers.time = container.player.progressBar.Value;
                    container.player.timeLabel.Text = getTimeLabel(container.trackers.time) + " / " + container.songlists.nowPlaying.Length;
                    container.player.wmplayer.controls.currentPosition = (double)container.trackers.time;
                }
                container.player.timer.Start();
            }
        }

        public static void changePlaylist(Mp3_Container container)
        {
            if (container.gui.playlistList.SelectedNode.IsSelected && container.gui.playlistList.SelectedNode.Nodes.Count == 0)
            {
                List<SongDetails> songListTemp = container.songlists.playlistDictionary[container.gui.playlistList.SelectedNode.Text];
                container.songlists.currentPlaylist.Clear();
                container.songlists.currentPlaylist.AddRange(songListTemp);
                container.gui.songList.Rows.Clear();

                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (SongDetails song in songListTemp)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(container.gui.songList);
                    row.Cells[0].Value = 0;
                    row.Cells[1].Value = song.SongName;
                    row.Cells[2].Value = song.Length;
                    row.Cells[3].Value = song.Artist;
                    row.Cells[4].Value = song.Album;
                    row.Cells[5].Value = song.Track;
                    row.Cells[6].Value = song.Year;
                    row.Cells[7].Value = song.Genre;
                    row.Cells[8].Value = song.Plays;
                    row.Cells[9].Value = song.Rating;
                    row.Cells[10].Value = song.Path;
                    rows.Add(row);
                }

                container.gui.songList.Rows.InsertRange(0, rows.ToArray());
                container.labels.playlistLabel.Text = container.gui.playlistList.SelectedNode.Text;
            }
            container.gui.playlistList.SelectedNode = null;
        }
    }
}
