using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using WMPLib;
using Mp3.Comparers;

namespace Mp3.Mp3_Player_and_Controls
{
    public partial class Mp3PlayerForm : Form
    {
        /*******************  Complex Variables  ****************************************************/

        private XmlDocument xml = new XmlDocument();
        private WMPLib.WindowsMediaPlayer wmplayer = new WMPLib.WindowsMediaPlayer();
        private SongDetails nowPlaying;
        private Dictionary<String, List<SongDetails>> playlistDictionary = new Dictionary<string, List<SongDetails>>();
        private List<Tuple<SongDetails, int>> previousSongsPlaylist = new List<Tuple<SongDetails, int>>();
        private List<SongDetails> masterSongList = new List<SongDetails>();
        private List<SongDetails> searchList = new List<SongDetails>();
        private List<SongDetails> currentPlaylist = new List<SongDetails>();

        /********************************************************************************************/

        /*******************  String Variables  *****************************************************/

        public string User {get; set;}
        public string UserPath { get; set; }
        public string UserDirectory { get; set; }
        private string currentDomain = AppDomain.CurrentDomain.BaseDirectory;
        private string lastSortParam = "";

        /********************************************************************************************/

        /*******************  Integer Variables  ****************************************************/

        private int nowPlayingRow = -1;
        private int listPosition = -1;
        private int Time { get; set; }

        /********************************************************************************************/

        /*******************  Boolean Variables  ****************************************************/

        private bool columnWidthChanged = false;
        private bool cameFromDouble = false;
        private bool previous = false;
        private bool isPlaying = false;
        private bool isPaused = false;

        /********************************************************************************************/

        /*******************  Position Variables  ***************************************************/

        private Tuple<String, int> NAME = new Tuple<string,int>("Song Name", 1);
        private Tuple<String, int> LENGTH = new Tuple<string,int>("Length", 2);
        private Tuple<String, int> ARTIST = new Tuple<string, int>("Artist", 3);
        private Tuple<String, int> ALBUM = new Tuple<string, int>("Album", 4);
        private Tuple<String, int> GENRE = new Tuple<string, int>("Genre", 7);
        private Tuple<String, int> PLAYS = new Tuple<string, int>("Plays", 8);
        private Tuple<String, int> RATING = new Tuple<string, int>("Rating", 9);
        private Tuple<String, int> YEAR = new Tuple<string, int>("Year", 6);

        /********************************************************************************************/

        /*******************  Start Up Functions  ***************************************************/

        public Mp3PlayerForm(string user)
        {
            InitializeComponent();
            User = user;
            UserDirectory = currentDomain + "Users\\";
            UserPath = UserDirectory + user + ".xml";
            Time = 0;
            fieldComboBox.SelectedIndex = 0;
        }

        private void startMp3_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Focus();
            this.KeyPreview = true;
            loadUser();
        }

        private void loadUser()
        {
            if (File.Exists(UserPath))
            {
                xml.Load(UserPath);
                if (xml.ChildNodes[1].ChildNodes[0].HasChildNodes)
                {
                    playlistList.Nodes.Add("Master Library");
                    foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[0].ChildNodes[0])
                    {
                        SongDetails song = new SongDetails(xn.ChildNodes[0].InnerText, xn.ChildNodes[1].InnerText, null,
                                                           xn.ChildNodes[2].InnerText, xn.ChildNodes[5].InnerText, xn.ChildNodes[6].InnerText,
                                                           int.Parse(xn.ChildNodes[7].InnerText), long.Parse(xn.ChildNodes[8].InnerText),
                                                           xn.ChildNodes[9].InnerText, xn.ChildNodes[3].InnerText, xn.ChildNodes[4].InnerText);

                        if (!songListContains(song))
                        {
                            masterSongList.Add(song);
                            currentPlaylist.Add(song);
                            songList.Rows.Insert(0, 0, song.SongName, song.Length, song.Artist, song.Album, song.Track, song.Year, song.Genre, song.Plays, song.Rating, song.Path);
                        }
                    }

                    int count = 0;
                    foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[0])
                    {
                        if (count == 0)
                        {
                            count++;
                            continue;
                        }
                        playlistList.Nodes.Add(xn.Attributes["name"].Value);
                        if (xn.HasChildNodes)
                        {
                            List<SongDetails> tempSongList = new List<SongDetails>();
                            foreach (XmlNode xcn in xn)
                            {
                                playlistList.Nodes[count].Nodes.Add(xcn.Attributes["name"].Value);
                                foreach (XmlNode xccn in xcn)
                                {
                                    SongDetails song = new SongDetails(xccn.ChildNodes[0].InnerText, xccn.ChildNodes[1].InnerText, null,
                                                               xccn.ChildNodes[2].InnerText, xccn.ChildNodes[5].InnerText, xccn.ChildNodes[6].InnerText,
                                                               int.Parse(xccn.ChildNodes[7].InnerText), long.Parse(xccn.ChildNodes[8].InnerText),
                                                               xccn.ChildNodes[9].InnerText, xccn.ChildNodes[3].InnerText, xccn.ChildNodes[4].InnerText);
                                    tempSongList.Add(song);
                                }
                                playlistDictionary.Add(xcn.Attributes["name"].Value, tempSongList);
                            }
                        }

                        count++;
                    }
                }

                playlistDictionary["Master Library"] = masterSongList;
            }
            else
            {
                xml.LoadXml("<UserMusic><PlaylistsGroup><PlaylistGroupNames name=\"Master Library\"></PlaylistGroupNames></PlaylistsGroup></UserMusic>");
                if (!Directory.Exists(UserDirectory))
                {
                    Directory.CreateDirectory(UserDirectory);
                }
                XmlTextWriter writer = new XmlTextWriter(UserPath, null);
                writer.Formatting = Formatting.Indented;
                xml.Save(writer);
            }
            columnWidthChanged = false;
            playlistLabel.Text = "Master Library";
            songList.Sort(new NameComparer(SortOrder.Ascending));
            nameAscending.Checked = true;
        }

        /********************************************************************************************/

        /*******************  Play, Stop, Previous, Next Buttons ************************************/

        private void playButton_Click(object sender, EventArgs e)
        {
            if (isPlaying && (nowPlayingRow == songList.SelectedRows[0].Index))
            {
                wmplayer.controls.pause();
                songTimer.Stop();
                isPaused = true;
                isPlaying = false;
                changePlayPicture();
                return;
            }
            if (isPaused && (nowPlayingRow == songList.SelectedRows[0].Index))
            {
                songTimer.Start();
                wmplayer.controls.play();
                isPaused = false;
                isPlaying = true;
                changePlayPicture();
                return;
            }

            bool shuffle = shuffleMenuItem.Checked;
            if (cameFromDouble)
            {
                shuffleMenuItem.Checked = false;
                cameFromDouble = false;
            }
            if (shuffleMenuItem.Checked)
            {
                shuffleSong();
                play();
            }
            else if (songList.SelectedRows.Count == 0)
            {
                DataGridViewRow row = songList.Rows[0];
                row.Selected = true;
                if (checkNull(row))
                {
                    return;
                }
                nowPlayingRow = row.Index;
                play();
            }
            else
            {
                stopButton_Click(sender, e);
                DataGridViewRow row = songList.SelectedRows[0];
                if (checkNull(row))
                {
                    return;
                }
                if (shuffle)
                {
                    shuffleMenuItem.Checked = true;
                }
                nowPlayingRow = row.Index;
                play();
            }
            fillLabels();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            wmplayer.controls.stop();
            timeLabel.Text = "00:00:00 / 00:00:00";
            songTimer.Enabled = false;
            songTimer.Stop();
            Time = 0;
            progressBar1.Value = 0;
            isPlaying = false;
            isPaused = false;
            changePlayPicture();
            nowPlaying = null;
            songTitleLabel.Text = "";
            artistLabel.Text = "";
            albumLabel.Text = "";
            genreLabel.Text = "";
        }

        private void previousButton_Click(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
            {
                previous = true;
            }
            if (nowPlayingRow < 0)
            {
                return;
            }
            songList.SelectedRows[0].Selected = false;
            if (shuffleMenuItem.Checked)
            {
                listPosition--;
                if (listPosition < 0 && repeatMenuItem.Checked)
                {
                    stopButton_Click(sender, e);
                    listPosition = 0;
                    nowPlayingRow = previousSongsPlaylist.ElementAt(listPosition).Item2;
                    play();
                }
                else if (listPosition >= 0)
                {
                    nowPlayingRow = previousSongsPlaylist.ElementAt(listPosition).Item2;
                    if (e.Clicks == 1 && !previous)
                    {
                        if (progressBar1.Value <= 3)
                        {
                            listPosition++;
                            nowPlayingRow = previousSongsPlaylist.ElementAt(listPosition).Item2;
                        }
                    }
                    stopButton_Click(sender, e);

                    if (repeatMenuItem.Checked)
                    {
                        listPosition++;
                        nowPlayingRow = previousSongsPlaylist.ElementAt(listPosition).Item2;
                        play();
                    }
                    else
                    {
                        play();
                    }
                }
                else
                {
                    stopButton_Click(sender, e);
                }
            }
            else
            {
                nowPlayingRow--;
                if (e.Clicks == 1 && !previous)
                {
                    if (progressBar1.Value <= 3)
                    {
                        nowPlayingRow++;
                        play();
                        return;
                    }
                }
                stopButton_Click(sender, e);
                if (nowPlayingRow < 0 && repeatMenuItem.Checked)
                {
                    nowPlayingRow = 0;
                    play();
                }
                else if (repeatMenuItem.Checked)
                {
                    nowPlayingRow++;
                    play();
                }
                else if (nowPlayingRow < 0) { }
                else
                {
                    play();
                }

            }

            if (nowPlaying != null)
            {
                fillLabels();
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            stopButton_Click(sender, e);
            songList.SelectedRows[0].Selected = false;
            if (shuffleMenuItem.Checked)
            {
                shuffleSong();
                play();
            }
            else
            {
                nowPlayingRow++;
                if (nowPlayingRow >= songList.RowCount - 1 && repeatAllMenuItem.Checked)
                {
                    nowPlayingRow = 0;
                    play();
                }
                else if (repeatMenuItem.Checked)
                {
                    nowPlayingRow--;
                    play();
                }
                else if (nowPlayingRow >= songList.RowCount - 1 && !repeatAllMenuItem.Checked) { }
                else
                {
                    play();
                }

            }

            if (nowPlaying != null)
            {
                fillLabels();
            }
        }

        /********************************************************************************************/

        /*******************  Play, Shuffle, Fill Labels, Picutre Change  ***************************/

        private void play()
        {
            DataGridViewRow row = songList.Rows[nowPlayingRow];
            songList.CurrentCell = songList[1, row.Index];
            row.Selected = true;
            nowPlaying = new SongDetails(row.Cells[1].Value.ToString(), row.Cells[3].Value.ToString(), null,
                     row.Cells[4].Value.ToString(), row.Cells[7].Value.ToString(), row.Cells[2].Value.ToString(),
                     int.Parse(row.Cells[9].Value.ToString()), long.Parse(row.Cells[8].Value.ToString()),
                     row.Cells[10].Value.ToString(), row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString());

            if (listPosition != -1)
            {
                if (previousSongsPlaylist.ElementAt(listPosition).Item1.SongName.Equals(nowPlaying.SongName) &&
                    previousSongsPlaylist.ElementAt(listPosition).Item1.Length == nowPlaying.Length &&
                    previousSongsPlaylist.ElementAt(listPosition).Item1.Artist.Equals(nowPlaying.Artist) &&
                    previousSongsPlaylist.ElementAt(listPosition).Item1.Album.Equals(nowPlaying.Album))
                { }
                else
                {
                    listPosition++;
                    previousSongsPlaylist.Add(new Tuple<SongDetails, int>(nowPlaying, row.Index));
                }
            }
            else
            {
                listPosition++;
                previousSongsPlaylist.Add(new Tuple<SongDetails, int>(nowPlaying, row.Index));
            }


            wmplayer.URL = nowPlaying.Path;
            timeLabel.Text = "00:00:00 / " + nowPlaying.Length;
            progressBar1.Maximum = totalTime(nowPlaying.Length);
            progressBar1.Value = 0;
            songTimer.Enabled = false;
            songTimer.Stop();
            wmplayer.controls.play();
            songTimer.Enabled = true;
            songTimer.Start();
            isPlaying = true;
            isPaused = false;
            changePlayPicture();
        }

        private void shuffleSong()
        {
            int rowCount = songList.Rows.Count - 1;
            Random rand = new Random();
            int rowChoice = rand.Next() % rowCount;
            DataGridViewRow row = songList.Rows[rowChoice];
            row.Selected = true;
            if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                row.Cells[3].Value == null && row.Cells[4].Value == null && row.Cells[5].Value == null &&
                row.Cells[6].Value == null && row.Cells[7].Value == null && row.Cells[8].Value == null)
            {
                return;
            }
            nowPlayingRow = row.Index;
        }

        private void fillLabels()
        {
            if (nowPlaying.SongName.Contains("&"))
            {
                string[] songName = nowPlaying.SongName.Split('&');
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
                songTitleLabel.Text = name;
            }
            else
            {
                songTitleLabel.Text = nowPlaying.SongName;
            }

            if (nowPlaying.Artist.Contains("&"))
            {
                string[] artistName = nowPlaying.Artist.Split('&');
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
                artistLabel.Text = name;
            }
            else
            {
                artistLabel.Text = nowPlaying.Artist;
            }

            if (nowPlaying.Album.Contains("&"))
            {
                string[] albumName = nowPlaying.Album.Split('&');
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
                albumLabel.Text = name;
            }
            else
            {
                albumLabel.Text = nowPlaying.Album;
            }

            genreLabel.Text = nowPlaying.Genre;
        }

        private void changePlayPicture()
        {
            if ((isPlaying && !isPaused) || (!isPlaying && !isPaused))
            {
                playButton.BackgroundImage = Mp3.Properties.Resources.pause2;
            }
            else if (isPaused && !isPlaying)
            {
                playButton.BackgroundImage = Mp3.Properties.Resources.play2;
            }
        }

        /********************************************************************************************/

        /********************************************************************************************/
        /*******************  Menu Item Buttons  ****************************************************/
        /********************************************************************************************/

        /*******************  Playback Options  *****************************************************/

        private void shuffleMenuItem_Click(object sender, EventArgs e)
        {
            previousSongsPlaylist.Clear();
            if (nowPlaying != null)
            {
                previousSongsPlaylist.Add(new Tuple<SongDetails, int>(nowPlaying, nowPlayingRow));
                listPosition = 0;
            }
            else
            {
                listPosition = -1;
            }

            if (shuffleMenuItem.Checked)
            {
                shuffleMenuItem.Checked = false;
            }
            else
            {
                shuffleMenuItem.Checked = true;
            }
        }

        private void repeatMenuItem_Click(object sender, EventArgs e)
        {
            if (repeatMenuItem.Checked)
            {
                repeatMenuItem.Checked = false;
            }
            else
            {
                repeatMenuItem.Checked = true;
                repeatAllMenuItem.Checked = false;
            }
        }

        private void repeatAllMenuItem_Click(object sender, EventArgs e)
        {
            if (repeatAllMenuItem.Checked)
            {
                repeatAllMenuItem.Checked = false;
            }
            else
            {
                repeatAllMenuItem.Checked = true;
                repeatMenuItem.Checked = false;
            }
        }

        /********************************************************************************************/

        /*******************  Song Options  *********************************************************/

        private void addSongsButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "Mp3 File|*.mp3";
                dlgOpen.InitialDirectory = "C:\\";
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    createSong(dlgOpen.FileName);
                }
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlgOpen = new FolderBrowserDialog())
            {
                dlgOpen.RootFolder = System.Environment.SpecialFolder.Desktop;
                List<string> songs = new List<string>();
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    ProcessDirectory(dlgOpen.SelectedPath, songs);
                    foreach (string fileName in songs)
                    {
                        createSong(fileName);
                    }
                }
            }

        }

        private void editSongToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete " + songList.SelectedRows[0].Cells[1].Value.ToString() + " by " +
                                                                     songList.SelectedRows[0].Cells[3].Value.ToString() + "?", "Delete Song",
                                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                                     MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                if (xml.ChildNodes[1].ChildNodes[1].HasChildNodes)
                {
                    foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[1])
                    {
                        if (xn.ChildNodes[9].InnerText.Equals(songList.SelectedRows[0].Cells[10].Value.ToString()))
                        {
                            xml.ChildNodes[1].ChildNodes[1].RemoveChild(xn);
                            int rowIndex = songList.SelectedRows[0].Index;
                            songList.SelectedRows[0].Selected = false;
                            songList.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }
                }
                xml.Save(UserPath);
            }
        }

        /********************************************************************************************/

        /*******************  Playlist Options  *****************************************************/

        private void addSelectedToPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPlaylistGroup toAdd = new AddPlaylistGroup();
            if (toAdd.ShowDialog() == DialogResult.OK)
            {
                if (alreadyAdded(toAdd.getPlaylistName()))
                {
                    toAdd.Dispose();
                    addGroupToolStripMenuItem_Click(sender, e);
                    return;
                }
                addPlaylistGroupXML(toAdd.getPlaylistName());
            }
            toAdd.Dispose();
        }

        private void addPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> comboBoxItems = new List<string>();
            comboBoxItems.Add("");
            foreach(XmlNode xn in xml.ChildNodes[1].ChildNodes[0])
            {
                if(!playlistDictionary.ContainsKey(xn.Attributes["name"].Value))
                {
                    comboBoxItems.Add(xn.Attributes["name"].Value);
                }
            }
            AddPlaylist toAdd = new AddPlaylist(comboBoxItems);
            if (toAdd.ShowDialog() == DialogResult.OK)
            {
                if (alreadyAdded(toAdd.getPlaylist()))
                {
                    toAdd.Dispose();
                    addPlaylistToolStripMenuItem_Click(sender, e);
                    return;
                }

                playlistList.BeginUpdate();
                if (toAdd.getPlaylistGroup().Equals(""))
                {
                    if (!xmlContainsPlaylistGroup(toAdd.getPlaylist()))
                    {
                        addPlaylistGroupXML(toAdd.getPlaylist());
                    }
                    List<SongDetails> songs = new List<SongDetails>();
                    playlistDictionary.Add(toAdd.getPlaylist(), songs);
                }
                else
                {
                    if (!checkAddTree(toAdd.getPlaylist(), toAdd.getPlaylistGroup()))
                    {
                        addPlaylistGroupXML(toAdd.getPlaylistGroup());
                        if (!checkAddTree(toAdd.getPlaylist(), toAdd.getPlaylistGroup()))
                        {
                            MessageBox.Show("Error in adding Playlist to tree see system administrator", 
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
                playlistList.EndUpdate();
            }
            toAdd.Dispose();
        }

        private void editPlayListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deletePlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (playlistList.SelectedNode != null)
            {
                if (playlistList.SelectedNode.Text.Equals("Master Library"))
                {
                    MessageBox.Show("You cannot delete your Master Library", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }

                bool canDelete = false;
                if(playlistList.SelectedNode.Nodes.Count > 0)
                {
                    if (MessageBox.Show("Are you sure you want to delete " + playlistList.SelectedNode.Text +
                                        "? This will Also delete all childern in this group!", "Delete",
                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button1) == DialogResult.OK) 
                    {
                        canDelete = true;
                    }
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to delete " + playlistList.SelectedNode.Text + "?", 
                                        "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, 
                                        MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        canDelete = true;
                    }
                }
                if (!canDelete)
                {
                    return;
                }
                else
                {
                    string playlist = playlistList.SelectedNode.Text;
                    foreach (TreeNode xn in playlistList.Nodes)
                    {
                        if (xn.Text.Equals(playlist))
                        {
                            playlistList.Nodes.Remove(xn);
                            break;
                        }
                        else if (xn.Nodes.Count > 0)
                        {
                            bool broke = false;
                            foreach (TreeNode xcn in xn.Nodes)
                            {
                                if (xcn.Text.Equals(playlist))
                                {
                                    xn.Nodes.Remove(xcn);
                                    broke = true;
                                    break;
                                }
                            }
                            if (broke)
                            {
                                break;
                            }
                        }
                    }

                    foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[0])
                    {
                        if (xn.Attributes["name"].Value.Equals(playlist))
                        {
                            xml.ChildNodes[1].ChildNodes[0].RemoveChild(xn);
                            break;
                        }
                        else if (xn.HasChildNodes)
                        {
                            bool broke = false;
                            foreach (XmlNode xcn in xn)
                            {
                                if (xcn.Name.Equals("Song"))
                                {
                                    continue;
                                }
                                if (xcn.Attributes["name"].Value.Equals(playlist))
                                {
                                    xn.RemoveChild(xcn);
                                    broke = true;
                                    break;
                                }
                            }
                            if (broke)
                            {
                                break;
                            }
                        }
                    }
                    xml.Save(UserPath);
                }
            }
        }

        /********************************************************************************************/

        /*******************  Sorting Options  ******************************************************/

        private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameAscending.Checked = true;

            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Name");
        }

        private void descendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nameDescending.Checked = true;

            nameAscending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Name");
        }

        private void lengthAscending_Click(object sender, EventArgs e)
        {
            lengthAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Length");
        }

        private void lengthDescending_Click(object sender, EventArgs e)
        {
            lengthDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Length");
        }

        private void artistAscending_Click(object sender, EventArgs e)
        {
            artistAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Artist");
        }

        private void artistDescending_Click(object sender, EventArgs e)
        {
            artistDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Artist");
        }

        private void albumAscending_Click(object sender, EventArgs e)
        {
            albumAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Album");
        }

        private void albumDescending_Click(object sender, EventArgs e)
        {
            albumDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Album");
        }

        private void yearAscending_Click(object sender, EventArgs e)
        {
            yearAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Year");
        }

        private void yearDescending_Click(object sender, EventArgs e)
        {
            yearDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Year");
        }

        private void genreAscending_Click(object sender, EventArgs e)
        {
            genreAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Genre");
        }

        private void genreDescending_Click(object sender, EventArgs e)
        {
            genreDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Genre");
        }

        private void playsAscending_Click(object sender, EventArgs e)
        {
            playsAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Plays");
        }

        private void playsDescending_Click(object sender, EventArgs e)
        {
            playsDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            ratingAscending.Checked = false;
            ratingDescending.Checked = false;

            sort("Plays");
        }

        private void ratingAscending_Click(object sender, EventArgs e)
        {
            ratingAscending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingDescending.Checked = false;

            sort("Rating");
        }

        private void ratingDescending_Click(object sender, EventArgs e)
        {
            ratingDescending.Checked = true;

            nameAscending.Checked = false;
            nameDescending.Checked = false;
            lengthAscending.Checked = false;
            lengthDescending.Checked = false;
            artistAscending.Checked = false;
            artistDescending.Checked = false;
            albumAscending.Checked = false;
            albumDescending.Checked = false;
            yearAscending.Checked = false;
            yearDescending.Checked = false;
            genreAscending.Checked = false;
            genreDescending.Checked = false;
            playsAscending.Checked = false;
            playsDescending.Checked = false;
            ratingAscending.Checked = false;

            sort("Rating");
        }

        private void multiSortAndSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /********************************************************************************************/

        /*******************  Sorting Helper Functions  *********************************************/

        private void sort(string value)
        {
            lastSortParam = value;
            switch (value)
            {
                case "Name":
                    if (nameAscending.Checked)
                    {
                        songList.Sort(new NameComparer(SortOrder.Ascending));
                    }
                    else if(nameDescending.Checked)
                    {
                        songList.Sort(new NameComparer(SortOrder.Descending));
                    }
                    break;
                case "Length":
                    if (lengthAscending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Ascending, 2));
                    }
                    else if (lengthDescending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Descending, 2));
                    }
                    break;
                case "Artist":
                    if (artistAscending.Checked)
                    {
                        songList.Sort(new ArtistComparer(SortOrder.Ascending));
                    }
                    else if (artistDescending.Checked)
                    {
                        songList.Sort(new ArtistComparer(SortOrder.Descending));
                    }
                    break;
                case "Album":
                    if (albumAscending.Checked)
                    {
                        songList.Sort(new AlbumComparer(SortOrder.Ascending));
                    }
                    else if (albumDescending.Checked)
                    {
                        songList.Sort(new AlbumComparer(SortOrder.Descending));
                    }
                    break;
                case "Year":
                    if (yearAscending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Ascending, 6));
                    }
                    else if (yearDescending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Descending, 6));
                    }
                    break;
                case "Genre":
                    if (genreAscending.Checked)
                    {
                        songList.Sort(new GenreComparer(SortOrder.Ascending));
                    }
                    else if (genreDescending.Checked)
                    {
                        songList.Sort(new GenreComparer(SortOrder.Descending));
                    }
                    break;
                case "Plays":
                    if (playsAscending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Ascending, 8));
                    }
                    else if (playsDescending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Descending, 8));
                    }
                    break;
                case "Rating":
                    if (ratingAscending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Ascending, 9));
                    }
                    else if (ratingDescending.Checked)
                    {
                        songList.Sort(new PositionComparer(SortOrder.Descending, 9));
                    }
                    break;
            }
        }

        /********************************************************************************************/

        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/



        /********************************************************************************************/
        /*******************  Menu Item Helper Functions  *******************************************/
        /********************************************************************************************/

        public static void ProcessDirectory(string targetDirectory, List<string> listSongs)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                listSongs.Add(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, listSongs);
        }

        private bool songListContains(SongDetails song)
        {
            bool isFound = false;
            foreach (DataGridViewRow row in songList.Rows)
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

        private void createSong(String file)
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

            if (!xmlContainsSong(song))
            {
                songList.Rows.Insert(0, 0, song.SongName, song.Length, song.Artist, song.Album, song.Track, song.Year, song.Genre, song.Plays, song.Rating, song.Path);
                masterSongList.Add(song);
                currentPlaylist.Add(song);

                XmlElement parent = xml.CreateElement("Song");

                XmlElement userElem = xml.CreateElement("name");
                XmlText text = xml.CreateTextNode(song.SongName);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("artist");
                text = xml.CreateTextNode(song.Artist);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("album");
                text = xml.CreateTextNode(song.Album);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("track");
                text = xml.CreateTextNode(song.Track);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("year");
                text = xml.CreateTextNode(song.Year);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("genre");
                text = xml.CreateTextNode(song.Genre);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("length");
                text = xml.CreateTextNode(song.Length);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("rating");
                text = xml.CreateTextNode(song.Rating.ToString());
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("plays");
                text = xml.CreateTextNode(song.Plays.ToString());
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                userElem = xml.CreateElement("path");
                text = xml.CreateTextNode(song.Path);
                parent.AppendChild(userElem);
                parent.LastChild.AppendChild(text);

                xml.ChildNodes[1].ChildNodes[0].ChildNodes[0].AppendChild(parent);

                XmlTextWriter writer = new XmlTextWriter(UserPath, null);
                writer.Formatting = Formatting.Indented;
                xml.Save(writer);
                writer.Close();
            }
            else
            {
                MessageBox.Show("The Song" + '\n' + file + '\n' + "has already been added", "Song has been added");
            }
            object sender = null;
            EventArgs e = null;
            searchTextBox_TextChanged(sender, e);
            sort(lastSortParam);
        }

        private bool checkNull(DataGridViewRow row)
        {
            bool isNull = false;
            if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null &&
                row.Cells[3].Value == null && row.Cells[4].Value == null && row.Cells[5].Value == null &&
                row.Cells[6].Value == null && row.Cells[7].Value == null && row.Cells[8].Value == null)
            {
                isNull = true;
            }
            return isNull;
        }

        private bool xmlContainsSong(SongDetails song)
        {
            bool isFound = false;
            if (xml.ChildNodes[1].ChildNodes[0].ChildNodes[0].HasChildNodes)
            {
                foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[0].ChildNodes[0])
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

        private bool xmlContainsPlaylistGroup(String playlist)
        {
            bool isFound = false;
            if (xml.ChildNodes[1].ChildNodes[0].HasChildNodes)
            {
                foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[0])
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

        private bool xmlContainsPlaylist(String playlist)
        {
            bool isFound = false;
            if (xml.ChildNodes[1].ChildNodes[0].HasChildNodes)
            {
                foreach (XmlNode xn in xml.ChildNodes[1].ChildNodes[0])
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

        private void addPlaylistGroupXML(string playlistName)
        {
            playlistList.BeginUpdate();
            playlistList.Nodes.Add(playlistName);
            playlistList.EndUpdate();

            if (!xmlContainsPlaylistGroup(playlistName))
            {
                XmlElement parent = xml.CreateElement("PlaylistGroupNames");
                parent.SetAttribute("name", playlistName);
                xml.ChildNodes[1].ChildNodes[0].AppendChild(parent);

                XmlTextWriter writer = new XmlTextWriter(UserPath, null);
                writer.Formatting = Formatting.Indented;
                xml.Save(writer);
                writer.Close();
            }
        }

        private void addPlaylistXML(string playlist, string playlistGroup)
        {
            if (!xmlContainsPlaylist(playlist))
            {
                foreach (XmlNode xcn in xml.ChildNodes[1].ChildNodes[0])
                {
                    if (xcn.Attributes["name"].Value.Equals(playlistGroup))
                    {
                        XmlElement child = xml.CreateElement("Playlist");
                        child.SetAttribute("name", playlist);
                        xcn.AppendChild(child);

                        XmlTextWriter writer = new XmlTextWriter(UserPath, null);
                        writer.Formatting = Formatting.Indented;
                        xml.Save(writer);
                        writer.Close();
                    }
                }
            }
        }

        private bool checkAddTree(string playlist, string playlistGroup)
        {
            bool found = false;
            foreach (TreeNode xn in playlistList.Nodes)
            {
                if (xn.Text.Equals(playlistGroup))
                {
                    found = true;
                    xn.Nodes.Add(playlist);
                    addPlaylistXML(playlist, playlistGroup);
                    List<SongDetails> songs = new List<SongDetails>();
                    playlistDictionary.Add(playlist, songs);
                }             
            }
            return found;
        }

        private bool alreadyAdded(string playlist)
        {
            foreach (TreeNode xn in playlistList.Nodes)
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

        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/



        /********************************************************************************************/
        /*******************  Time and Progress Bar Functions  **************************************/
        /********************************************************************************************/

        private void songTimer_Tick(object sender, EventArgs e)
        {
            Time++;
            if (Time >= 3)
            {
                previous = false;
            }
            progressBar1.Value++;
            if (Time.Equals(totalTime(nowPlaying.Length)))
            {
                stopButton_Click(sender, e);
                if (repeatAllMenuItem.Checked && (songList.SelectedRows[0].Index == songList.RowCount - 1))
                {
                    songList.SelectedRows[0].Selected = false;
                    songList.Rows[0].Selected = true;
                    playButton_Click(sender, e);
                }
                else if (repeatMenuItem.Checked)
                {
                    play();
                }
                else if (shuffleMenuItem.Checked || repeatMenuItem.Checked)
                {
                    playButton_Click(sender, e);
                }
                return;
            }

            timeLabel.Text = getTimeLabel(Time) + " / " + nowPlaying.Length;
        }

        private string getTimeLabel(long inTime)
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

        private int totalTime(String length)
        {
            string[] split = length.Split(':');
            int seconds = int.Parse(split[2]);
            int minutes = int.Parse(split[1]);
            int hours = int.Parse(split[0]);

            int total = (hours * 3600) + (minutes * 60) + seconds;

            return total;
        }

        private void progressBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                songTimer.Stop();
                decimal pos = 0M;
                pos = ((decimal)e.X / (decimal)progressBar1.Width) * progressBar1.Maximum;
                pos = Convert.ToInt32(pos);

                if (pos >= progressBar1.Minimum && pos <= progressBar1.Maximum)
                {
                    progressBar1.Value = (int)pos;
                    Time = progressBar1.Value;
                    timeLabel.Text = getTimeLabel(Time) + " / " + nowPlaying.Length;
                    wmplayer.controls.currentPosition = (double)Time;
                }
                songTimer.Start();
            }
        }

        private void progressBar1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                songTimer.Stop();
                decimal pos = 0M;
                pos = ((decimal)e.X / (decimal)progressBar1.Width) * progressBar1.Maximum;
                pos = Convert.ToInt32(pos);

                if (pos >= progressBar1.Minimum && pos <= progressBar1.Maximum)
                {
                    progressBar1.Value = (int)pos;
                    Time = progressBar1.Value;
                    timeLabel.Text = getTimeLabel(Time) + " / " + nowPlaying.Length;
                    wmplayer.controls.currentPosition = (double)Time;
                }
                songTimer.Start();
            }
        }

        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/



        /********************************************************************************************/
        /*******************  Song List and Volume Bar Functions  ***********************************/
        /********************************************************************************************/

        private void songList_SelectionChanged(object sender, EventArgs e)
        {
            if (songList.SelectedRows.Count > 0 && nowPlayingRow != -1)
            {
                if (songList.SelectedRows[0].Index != nowPlayingRow)
                {
                    playButton.BackgroundImage = Mp3.Properties.Resources.play2;
                }
                else
                {
                    playButton.BackgroundImage = Mp3.Properties.Resources.pause2;
                }
            }
        }

        private void songList_DoubleClick(object sender, EventArgs e)
        {
            if (!columnWidthChanged)
            {
                cameFromDouble = true;
                playButton_Click(sender, e);
            }
            else
            {
                columnWidthChanged = false;
            }
        }

        private void songList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            columnWidthChanged = true;
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            wmplayer.settings.volume = volumeBar.Value * 10;
        }

        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/



        /********************************************************************************************/
        /*******************  Playlist List Functions  **********************************************/
        /********************************************************************************************/

        private void playlistList_DoubleClick(object sender, EventArgs e)
        {
            if (playlistList.SelectedNode.IsSelected && playlistList.SelectedNode.Nodes.Count == 0)
            {
                List<SongDetails> songListTemp = playlistDictionary[playlistList.SelectedNode.Text];
                currentPlaylist.Clear();
                currentPlaylist.AddRange(songListTemp);
                songList.Rows.Clear();

                List<DataGridViewRow> rows = new List<DataGridViewRow>();
                foreach (SongDetails song in songListTemp)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(songList);
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

                songList.Rows.InsertRange(0, rows.ToArray());
                playlistLabel.Text = playlistList.SelectedNode.Text;
            }
            playlistList.SelectedNode = null;
        }

        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/



        /********************************************************************************************/
        /*******************  Search Functions  *****************************************************/
        /********************************************************************************************/

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            searchList.Clear();
            Tuple<String, int> field = null;
            switch (fieldComboBox.SelectedItem.ToString())
            {
                case "Song Name":
                    field = NAME;
                    break;
                case "Length":
                    field = LENGTH;
                    break;
                case "Artist":
                    field = ARTIST;
                    break;
                case "Album":
                    field = ALBUM;
                    break;
                case "Genre":
                    field = GENRE;
                    break;
                case "Plays":
                    field = PLAYS;
                    break;
                case "Rating":
                    field = RATING;
                    break;
                case "Year":
                    field = YEAR;
                    break;
            }

            search(field);

        }

        private void search(Tuple<String, int> field)
        {
            bool broke = false;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            rows.Clear();

            foreach (SongDetails song in currentPlaylist)
            {
                String find = "";

                switch (field.Item1)
                {
                    case "Song Name":
                        find = song.SongName;
                        break;
                    case "Length":
                        find = song.Length;
                        break;
                    case "Artist":
                        find = song.Artist;
                        break;
                    case "Album":
                        find = song.Album;
                        break;
                    case "Genre":
                        find = song.Genre;
                        break;
                    case "Plays":
                        find = song.Plays.ToString();
                        break;
                    case "Rating":
                        find = song.Rating.ToString();
                        break;
                    case "Year":
                        find = song.Year.ToString();
                        break;
                }
                if (find.ToUpper().Contains(searchTextBox.Text.ToUpper()))
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(songList);
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
            }
            if (broke)
            {
                return;
            }
            else
            {
                songList.Rows.Clear();
                songList.Rows.InsertRange(0, rows.ToArray());
                sort(lastSortParam);
            }
        }

        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/



        /********************************************************************************************/
        /*******************  Shortcut Bindings  ****************************************************/
        /********************************************************************************************/
        bool paused = false;

        private void Key_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (!paused)
                {
                    paused = true;
                    shuffleMenuItem_Click(sender, e);
                }
            }
            else if(e.Control && e.KeyCode == Keys.R)
            {
                if (!paused)
                {
                    paused = true;
                    repeatMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (!paused)
                {
                    paused = true;
                    repeatAllMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                if (!paused)
                {
                    paused = true;
                    addSongsButton_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                if (!paused)
                {
                    paused = true;
                    addFolderToolStripMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                if (!paused)
                {
                    paused = true;
                    editSongToolStripMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                if (!paused)
                {
                    paused = true;
                    deleteSongToolStripMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.M)
            {
                if (!paused)
                {
                    paused = true;
                    addSelectedToPlaylistToolStripMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                if (!paused)
                {
                    paused = true;
                    addGroupToolStripMenuItem_Click(sender, e);
                }
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                if (!paused)
                {
                    paused = true;
                    addPlaylistToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void resetPause()
        {
            paused = false;
        }

        private void Mp3PlayerForm_KeyUp(object sender, KeyEventArgs e)
        {
            resetPause();
        }
        /********************************************************************************************/
        /********************************************************************************************/
        /********************************************************************************************/     
    }
}