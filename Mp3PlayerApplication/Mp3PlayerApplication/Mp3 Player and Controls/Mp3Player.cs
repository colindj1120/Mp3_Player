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
using Mp3.Functions;
using Mp3.Containers;
using Mp3.Details;

namespace Mp3.Mp3_Player_and_Controls
{
    public partial class Mp3PlayerForm : Form
    {
        Mp3_Container container;

        public delegate void function(object sender, EventArgs e);
        public Dictionary<Tuple<Keys, Keys>, function> functionPairMap = new Dictionary<Tuple<Keys, Keys>, function>();
        public Dictionary<Tuple<Keys, Keys, Keys>, function> functionTriMap = new Dictionary<Tuple<Keys, Keys, Keys>, function>();
        
        /*******************  Start Up Functions  ***************************************************/

        public Mp3PlayerForm(string user)
        {
            InitializeComponent();

            container = new Mp3_Container();
            setupContainer(user);

            fieldComboBox.SelectedIndex = 0;
        }

        private void setupContainer(string user)
        {
            container.labels.songTitle = songTitleLabel;
            container.labels.arist = artistLabel;
            container.labels.album = albumLabel;
            container.labels.genre = genreLabel;
            container.labels.playlistLabel = playlistLabel;

            container.buttons.play = playButton;
            container.buttons.stop = stopButton;
            container.buttons.next = nextButton;
            container.buttons.previous = previousButton;

            container.player.timer = songTimer;
            container.player.progressBar = progressBar1;
            container.player.timeLabel = timeLabel;

            container.xmlFile.user = user;
            container.xmlFile.userDirectory = container.xmlFile.currentDomain + "Users\\";
            container.xmlFile.userPath = container.xmlFile.userDirectory + user + ".xml";

            container.gui.songList = songList;
            container.gui.playlistList = playlistList;
            container.gui.searchTextBox = searchTextBox;
            container.gui.shuffle = shuffleMenuItem;
            container.gui.repeat = repeatMenuItem;
            container.gui.repeatAll = repeatAllMenuItem;
            container.gui.searchFieldComboBox = fieldComboBox;
            container.gui.sortOptions.nameAscending = nameAscending;
            container.gui.sortOptions.nameDescending = nameDescending;
            container.gui.sortOptions.lengthAscending = lengthAscending;
            container.gui.sortOptions.lengthDescending = lengthDescending;
            container.gui.sortOptions.artistAscending = artistAscending;
            container.gui.sortOptions.artistDescending = artistDescending;
            container.gui.sortOptions.albumAscending = albumAscending;
            container.gui.sortOptions.albumDescending = albumDescending;
            container.gui.sortOptions.genreAscending = genreAscending;
            container.gui.sortOptions.genreDescending = genreDescending;
            container.gui.sortOptions.yearAscending = yearAscending;
            container.gui.sortOptions.yearDescending = yearDescending;
            container.gui.sortOptions.playsAscending = playsAscending;
            container.gui.sortOptions.playsDescending = playsDescending;
            container.gui.sortOptions.ratingAscending = ratingAscending;
            container.gui.sortOptions.ratingDescending = ratingDescending;
        }

        private void startMp3_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Focus();
            this.KeyPreview = true;
            Load_Save.loadUser(container);
            loadFunctionMap();
            container.gui.songList.CurrentCell = container.gui.songList[1, 0];
            container.gui.songList.SelectedRows[0].Selected = false;
        }

        /*******************  Play, Stop, Previous, Next Buttons ************************************/

        private void playButton_Click(object sender, EventArgs e)
        {
            SongControl.setupPlay(container);
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            SongControl.stop(container);
        }

        private void previousButton_Click(object sender, MouseEventArgs e)
        {
            SongControl.previous(container, e);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            SongControl.next(container);
        }

        /********************************************************************************************/
        /*******************  Menu Item Buttons  ****************************************************/
        /********************************************************************************************/

        /*******************  Playback Options  *****************************************************/

        private void shuffleMenuItem_Click(object sender, EventArgs e)
        {
            container.songlists.previousSongsPlaylist.Clear();
            if (container.songlists.nowPlaying != null)
            {
                container.songlists.previousSongsPlaylist.Add(new Tuple<SongDetails, int>(container.songlists.nowPlaying, container.trackers.nowPlayingRow));
                container.trackers.listPosition = 0;
            }
            else
            {
                container.trackers.listPosition = -1;
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
        /********************************************************************************************/

        private void addSongsButton_Click(object sender, EventArgs e)
        {
            if(Add.addSong(container))
            {
                searchTextBox_TextChanged(sender, e);
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Add.addFolder(container))
            {
                searchTextBox_TextChanged(sender, e);
            }
        }

        private void editSongToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteSongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete.deleteSong(container);
        }

        /********************************************************************************************/
        /*******************  Playlist Options  *****************************************************/
        /********************************************************************************************/

        private void addSelectedToPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add.addPlaylistGroup(container);
        }

        private void addPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add.addPlaylist(container);
        }

        private void editPlayListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deletePlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete.deletePlaylist(container);
        }

        /********************************************************************************************/
        /*******************  Sorting Options  ******************************************************/
        /********************************************************************************************/

        private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortItems.nameAscending(container);
        }

        private void descendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortItems.nameDescending(container);
        }

        private void lengthAscending_Click(object sender, EventArgs e)
        {
            SortItems.lengthAscending(container);
        }

        private void lengthDescending_Click(object sender, EventArgs e)
        {
            SortItems.lengthDescending(container);
        }

        private void artistAscending_Click(object sender, EventArgs e)
        {
            SortItems.artistAscending(container);
        }

        private void artistDescending_Click(object sender, EventArgs e)
        {
            SortItems.artistDescending(container);
        }

        private void albumAscending_Click(object sender, EventArgs e)
        {
            SortItems.albumAscending(container);
        }

        private void albumDescending_Click(object sender, EventArgs e)
        {
            SortItems.albumDescending(container);
        }

        private void yearAscending_Click(object sender, EventArgs e)
        {
            SortItems.yearAscending(container);
        }

        private void yearDescending_Click(object sender, EventArgs e)
        {
            SortItems.yearDescending(container);
        }

        private void genreAscending_Click(object sender, EventArgs e)
        {
            SortItems.genreAscending(container);
        }

        private void genreDescending_Click(object sender, EventArgs e)
        {
            SortItems.genreDescending(container);
        }

        private void playsAscending_Click(object sender, EventArgs e)
        {
            SortItems.playsAscending(container);
        }

        private void playsDescending_Click(object sender, EventArgs e)
        {
            SortItems.playsDescending(container);
        }

        private void ratingAscending_Click(object sender, EventArgs e)
        {
            SortItems.ratingAscending(container);
        }

        private void ratingDescending_Click(object sender, EventArgs e)
        {
            SortItems.ratingDescending(container);
        }

        private void multiSortAndSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /********************************************************************************************/
        /*******************  Menu Item Helper Functions  *******************************************/
        /********************************************************************************************/

        private void addPlaylistGroupXML(string playlistName)
        {
            Load_Save.savePlaylistGroupToXml(container, playlistName);
        }

        private void addPlaylistXML(string playlist, string playlistGroup)
        {
            Load_Save.savePlaylistToXml(container, playlist, playlistGroup);
        }

        /********************************************************************************************/
        /*******************  Time and Progress Bar Functions  **************************************/
        /********************************************************************************************/

        private void songTimer_Tick(object sender, EventArgs e)
        {
            GuiControl.updateTimer(container);
        }

        private void progressBar1_MouseMove(object sender, MouseEventArgs e)
        {
            GuiControl.progressBarUpdate(container, e);
        }

        private void progressBar1_Click(object sender, MouseEventArgs e)
        {
            GuiControl.progressBarUpdate(container, e);
        }

        /********************************************************************************************/
        /*******************  Song List and Volume Bar Functions  ***********************************/
        /********************************************************************************************/

        private void songList_SelectionChanged(object sender, EventArgs e)
        {
            if ((songList.SelectedRows.Count < 0 && container.trackers.nowPlayingRow == -1) || (!container.booleans.isPlaying && !container.booleans.isPaused))
            {
                return;
            }
            else if (!container.songlists.nowPlaying.equals(Create.createSelectedSong(container)))
            {
                playButton.BackgroundImage = Mp3.Properties.Resources.play2;
            }
            else
            {
                playButton.BackgroundImage = Mp3.Properties.Resources.pause2;
            }
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            container.player.wmplayer.settings.volume = volumeBar.Value * 10;
        }

        /********************************************************************************************/
        /*******************  Playlist List Functions  **********************************************/
        /********************************************************************************************/

        private void playlistList_Click(object sender, EventArgs e)
        {
            GuiControl.changePlaylist(container);
        }

        /********************************************************************************************/
        /*******************  Search Functions  *****************************************************/
        /********************************************************************************************/

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            Search.setupSearch(container);
        }

        /********************************************************************************************/
        /*******************  Shortcut Bindings  ****************************************************/
        /********************************************************************************************/

        private void Key_KeyDown(object sender, KeyEventArgs e)
        {
            Keys k = System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.ShiftKey;
            if(e.Control && e.KeyCode != k && !(e.Shift || e.Alt))
            {
                Tuple<Keys, Keys> keyValues = new Tuple<Keys, Keys>(Keys.Control, e.KeyCode);
                if(functionPairMap.ContainsKey(keyValues))
                {
                    function fpt = functionPairMap[keyValues];
                    fpt.Invoke(sender, e);
                }           
            }
            else if(e.Control && e.Shift && !e.Alt && e.KeyCode != k)
            {
                Tuple<Keys, Keys, Keys> keyValues = new Tuple<Keys,Keys,Keys>(Keys.Control, Keys.Shift, e.KeyCode);
                if(functionTriMap.ContainsKey(keyValues))
                {
                    function fpt = functionTriMap[keyValues];
                    fpt.Invoke(sender, e);
                }
            }
            else if (e.Control && !e.Shift && e.Alt && e.KeyCode != k)
            {
                Tuple<Keys, Keys, Keys> keyValues = new Tuple<Keys, Keys, Keys>(Keys.Control, Keys.Alt, e.KeyCode);
                if (functionTriMap.ContainsKey(keyValues))
                {
                    function fpt = functionTriMap[keyValues];
                    fpt.Invoke(sender, e);
                }
            }
            else if(e.KeyCode == Keys.Enter)
            {
                if(container.trackers.nowPlayingRow != songList.SelectedRows[0].Index)
                {
                    container.trackers.nowPlayingRow = songList.SelectedRows[0].Index;
                    SongControl.play(container);
                }
            }
            else if(e.KeyCode == Keys.Space)
            {
                if (!container.booleans.isPlaying && !container.booleans.isPaused)
                {
                    playButton_Click(sender, e);
                }
                else
                {
                    SongControl.play_pause(container);
                }
            }
        }

        private void loadFunctionMap()
        {
            function fpt = addPlaylistToolStripMenuItem_Click;
            Tuple<Keys, Keys> keyValues = new Tuple<Keys,Keys>(Keys.Control, Keys.P);
            functionPairMap.Add(keyValues, fpt);

            fpt = addGroupToolStripMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.G);
            functionPairMap.Add(keyValues, fpt);

            fpt = addSelectedToPlaylistToolStripMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.M);
            functionPairMap.Add(keyValues, fpt);

            fpt = deleteSongToolStripMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.D);
            functionPairMap.Add(keyValues, fpt);

            fpt = editSongToolStripMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.E);
            functionPairMap.Add(keyValues, fpt);

            fpt = addFolderToolStripMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.F);
            functionPairMap.Add(keyValues, fpt);

            fpt = shuffleMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.S);
            functionPairMap.Add(keyValues, fpt);

            fpt = repeatMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.R);
            functionPairMap.Add(keyValues, fpt);

            fpt = repeatAllMenuItem_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.A);
            functionPairMap.Add(keyValues, fpt);

            fpt = addSongsButton_Click;
            keyValues = new Tuple<Keys, Keys>(Keys.Control, Keys.N);
            functionPairMap.Add(keyValues, fpt);

            fpt = editPlayListToolStripMenuItem_Click;
            Tuple<Keys, Keys, Keys> triKeyValues = new Tuple<Keys, Keys, Keys>(Keys.Control, Keys.Shift, Keys.E);
            functionTriMap.Add(triKeyValues, fpt);

            fpt = deletePlaylistToolStripMenuItem_Click;
            triKeyValues = new Tuple<Keys, Keys, Keys>(Keys.Control, Keys.Shift, Keys.D);
            functionTriMap.Add(triKeyValues, fpt);
        }

        private void songList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SortItems.columnSortClick(container, e);
        }

        private void songList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1)
            {
                return;
            }
            playButton_Click(sender, e);
        }
    }
}