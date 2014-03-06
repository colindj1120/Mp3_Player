using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WMPLib;
using Mp3.Details;

namespace Mp3.Containers
{
    public class Booleans
    {
        public bool isPlaying = false;
        public bool isPaused = false;
    }

    public class PositionVariables
    {
        public Tuple<String, int> NAME = new Tuple<string, int>("Song Name", 1);
        public Tuple<String, int> LENGTH = new Tuple<string, int>("Length", 2);
        public Tuple<String, int> ARTIST = new Tuple<string, int>("Artist", 3);
        public Tuple<String, int> ALBUM = new Tuple<string, int>("Album", 4);
        public Tuple<String, int> GENRE = new Tuple<string, int>("Genre", 7);
        public Tuple<String, int> PLAYS = new Tuple<string, int>("Plays", 8);
        public Tuple<String, int> RATING = new Tuple<string, int>("Rating", 9);
        public Tuple<String, int> YEAR = new Tuple<string, int>("Year", 6);
    }

    public struct Labels
    {
        public Label songTitle;
        public Label arist;
        public Label album;
        public Label genre;
        public Label playlistLabel;
    }

    public struct Buttons
    {
        public PictureBox play;
        public PictureBox stop;
        public PictureBox next;
        public PictureBox previous;
    }

    public class WindowPlayers
    {
        public Timer timer;
        public ProgressBar progressBar;
        public Label timeLabel;
        public WindowsMediaPlayer wmplayer = new WMPLib.WindowsMediaPlayer();
    }

    public class XmlFile
    {
        public XmlDocument xml = new XmlDocument();
        public string user = "";
        public string userPath = "";
        public string userDirectory = "";
        public string currentDomain = AppDomain.CurrentDomain.BaseDirectory;
    }

    public class SongLists
    {
        public SongDetails nowPlaying;
        public Dictionary<String, List<SongDetails>> playlistDictionary = new Dictionary<string, List<SongDetails>>();
        public List<Tuple<SongDetails, int>> previousSongsPlaylist = new List<Tuple<SongDetails, int>>();
        public List<SongDetails> masterSongList = new List<SongDetails>();
        public List<SongDetails> searchList = new List<SongDetails>();
        public List<SongDetails> currentPlaylist = new List<SongDetails>();
    }

    public class Trackers
    {
        public string lastSortParam = "";
        public int nowPlayingRow = -1;
        public int listPosition = -1;
        public int time = 0;
    }

    public class SortOptions
    {
        public ToolStripMenuItem nameAscending;
        public ToolStripMenuItem nameDescending;
        public ToolStripMenuItem lengthAscending;
        public ToolStripMenuItem lengthDescending;
        public ToolStripMenuItem artistAscending;
        public ToolStripMenuItem artistDescending;
        public ToolStripMenuItem albumAscending;
        public ToolStripMenuItem albumDescending;
        public ToolStripMenuItem yearAscending;
        public ToolStripMenuItem yearDescending;
        public ToolStripMenuItem genreAscending;
        public ToolStripMenuItem genreDescending;
        public ToolStripMenuItem playsAscending;
        public ToolStripMenuItem playsDescending;
        public ToolStripMenuItem ratingAscending;
        public ToolStripMenuItem ratingDescending;
    }

    public class Gui
    {
        public DataGridView songList;
        public TreeView playlistList;
        public TextBox searchTextBox;
        public ToolStripMenuItem shuffle;
        public ToolStripMenuItem repeat;
        public ToolStripMenuItem repeatAll;
        public ComboBox searchFieldComboBox;
        public SortOptions sortOptions = new SortOptions();
    }

    public class Mp3_Container
    {
        public Buttons buttons = new Buttons();
        public Labels labels = new Labels();
        public PositionVariables positionVariables = new PositionVariables();
        public Booleans booleans = new Booleans();
        public WindowPlayers player = new WindowPlayers();
        public XmlFile xmlFile = new XmlFile();
        public SongLists songlists = new SongLists();
        public Gui gui = new Gui();
        public Trackers trackers = new Trackers();
    }
}
