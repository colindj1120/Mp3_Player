using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mp3.Containers;
using Mp3.Details;
using WMPLib;

namespace Mp3.Functions
{
    static class SongControl
    {
        public static void setupPlay(Mp3_Container container)
        {
            if(container.gui.songList.SelectedRows.Count == 0)
            {
                return;
            }
            if (container.songlists.nowPlaying != null && container.songlists.nowPlaying.equals(Create.createSelectedSong(container)))
            {
                SongControl.play_pause(container);
                return;
            }

            if (container.trackers.nowPlayingRow == -1 || (container.trackers.nowPlayingRow != container.gui.songList.SelectedRows[0].Index))
            {
                container.trackers.nowPlayingRow = container.gui.songList.SelectedRows[0].Index;
            }

            if (container.gui.shuffle.Checked)
            {
                SongControl.shuffleSong(container);
            }
            else if (container.gui.songList.SelectedRows.Count == 0)
            {
                DataGridViewRow row = container.gui.songList.Rows[0];
                row.Selected = true;
                if (Miscellaneous.checkNull(row))
                {
                    return;
                }
                container.trackers.nowPlayingRow = row.Index;
            }
            stop(container);
            SongControl.play(container);
            GuiControl.fillLabels(container);
        }

        public static void play(Mp3_Container container)
        {
            DataGridViewRow row = container.gui.songList.Rows[container.trackers.nowPlayingRow];
            if (Miscellaneous.checkNull(row))
            {
                return;
            }
            container.gui.songList.CurrentCell = container.gui.songList[1, row.Index];
            row.Selected = true;
            container.songlists.nowPlaying = Create.createAsong(container, row);

            if (container.trackers.listPosition != -1)
            {
                if (container.songlists.previousSongsPlaylist.ElementAt(container.trackers.listPosition).Item1.equals(container.songlists.nowPlaying))
                { }
                else
                {
                    container.trackers.listPosition++;
                    container.songlists.previousSongsPlaylist.Add(new Tuple<SongDetails, int>(container.songlists.nowPlaying, row.Index));
                }
            }
            else
            {
                container.trackers.listPosition++;
                container.songlists.previousSongsPlaylist.Add(new Tuple<SongDetails, int>(container.songlists.nowPlaying, row.Index));
            }

            container.player.wmplayer.URL = container.songlists.nowPlaying.Path;
            container.player.timeLabel.Text = "00:00:00 / " + container.songlists.nowPlaying.Length;
            container.player.progressBar.Maximum = Miscellaneous.totalTime(container.songlists.nowPlaying.Length);
            container.player.progressBar.Value = 0;
            container.player.timer.Enabled = false;
            container.player.timer.Stop();
            container.player.wmplayer.controls.play();
            container.player.timer.Enabled = true;
            container.player.timer.Start();
            container.booleans.isPlaying = true;
            container.booleans.isPaused = false;
            GuiControl.changePlayPicture(container);
        }

        public static void play_pause(Mp3_Container container)
        {
            if (container.trackers.nowPlayingRow < 0)
            {
                return;
            }
            if (container.booleans.isPlaying && !container.booleans.isPaused)
            {
                container.player.wmplayer.controls.pause();
                container.booleans.isPaused = true;
                container.booleans.isPlaying = false;
                container.player.timer.Stop();
                if(container.songlists.nowPlaying.equals(Create.createSelectedSong(container)))
                {
                    GuiControl.changePlayPicture(container);
                }
            }
            else if (!container.booleans.isPlaying && container.booleans.isPaused)
            {
                container.player.wmplayer.controls.play();
                container.booleans.isPaused = false;
                container.booleans.isPlaying = true;
                container.player.timer.Start();
                if (container.songlists.nowPlaying.equals(Create.createSelectedSong(container)))
                {
                    GuiControl.changePlayPicture(container);
                }
            }
        }

        public static void previous(Mp3_Container container, MouseEventArgs e)
        {
            if(container.trackers.listPosition == -1)
            {
                return;
            }
            container.trackers.listPosition--;

            if(e.Clicks > 1)
            {
                if (container.trackers.listPosition < 0 && container.gui.repeat.Checked)
                {
                    container.trackers.listPosition = 0;
                    container.trackers.nowPlayingRow = container.songlists.previousSongsPlaylist.ElementAt(container.trackers.listPosition).Item2;
                }
                else if (container.trackers.listPosition >= 0 && container.gui.repeat.Checked)
                {
                    container.trackers.listPosition++;
                }
                else if (container.trackers.listPosition < 0)
                {
                    stop(container);
                    return;
                }
                container.trackers.nowPlayingRow = container.songlists.previousSongsPlaylist.ElementAt(container.trackers.listPosition).Item2;

                stop(container);
                SongControl.play(container);
            }
            else
            {
                if ((container.trackers.listPosition < 0 && container.gui.repeat.Checked) || container.gui.repeat.Checked || container.player.progressBar.Value >= 3)
                {
                    container.trackers.listPosition++;
                }
                else if (container.trackers.listPosition < 0)
                {
                    stop(container);
                    return;
                }
                container.trackers.nowPlayingRow = container.songlists.previousSongsPlaylist.ElementAt(container.trackers.listPosition).Item2;
                stop(container);
                SongControl.play(container);
            }

            if (container.songlists.nowPlaying != null)
            {
                GuiControl.fillLabels(container);
            }
        }

        public static void next(Mp3_Container container)
        {
            stop(container);
            container.gui.songList.SelectedRows[0].Selected = false;
            if (container.gui.shuffle.Checked)
            {
                SongControl.shuffleSong(container);
            }
            else
            {
                container.trackers.nowPlayingRow++;
                if (container.trackers.nowPlayingRow >= container.gui.songList.RowCount && container.gui.repeatAll.Checked)
                {
                    container.trackers.nowPlayingRow = 0;
                }
                else if (container.gui.repeat.Checked)
                {
                    container.trackers.nowPlayingRow--;
                }
                else if (container.trackers.nowPlayingRow >= container.gui.songList.RowCount && !container.gui.repeatAll.Checked) 
                {
                    return;
                }
                else
                {
                    SongControl.play(container);
                }
            }
            SongControl.play(container);
            if (container.songlists.nowPlaying != null)
            {
                GuiControl.fillLabels(container);
            }
        }

        public static void stop(Mp3_Container container)
        {
            container.player.wmplayer.controls.stop();
            container.player.timeLabel.Text = "00:00:00 / 00:00:00";
            container.player.timer.Enabled = false;
            container.player.timer.Stop();
            container.trackers.time = 0;
            container.player.progressBar.Value = 0;
            container.booleans.isPlaying = false;
            container.booleans.isPaused = false;
            GuiControl.changePlayPicture(container);
            container.songlists.nowPlaying = null;
            container.labels.songTitle.Text = "";
            container.labels.arist.Text = "";
            container.labels.album.Text = "";
            container.labels.genre.Text = "";
        }

        public static void shuffleSong(Mp3_Container container)
        {
            int rowCount = container.gui.songList.Rows.Count - 1;
            Random rand = new Random();
            int rowChoice = container.trackers.nowPlayingRow;
            while (rowChoice == container.trackers.nowPlayingRow)
            {
                rowChoice = rand.Next() % rowCount;
            }
            DataGridViewRow row = container.gui.songList.Rows[rowChoice];
            row.Selected = true;
            if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                row.Cells[3].Value == null && row.Cells[4].Value == null && row.Cells[5].Value == null &&
                row.Cells[6].Value == null && row.Cells[7].Value == null && row.Cells[8].Value == null)
            {
                return;
            }
            container.trackers.nowPlayingRow = row.Index;
        }
    }
}
