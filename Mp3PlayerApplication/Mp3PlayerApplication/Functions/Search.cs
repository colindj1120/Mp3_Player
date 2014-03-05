using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mp3.Containers;
using Mp3.Details;

namespace Mp3.Functions
{
    class Search
    {
        public static void search(Mp3_Container container, Tuple<String, int> field)
        {
            bool broke = false;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            rows.Clear();

            foreach (SongDetails song in container.songlists.currentPlaylist)
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
                if (find.ToUpper().Contains(container.gui.searchTextBox.Text.ToUpper()))
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
            }
            if (broke)
            {
                return;
            }
            else
            {
                container.gui.songList.Rows.Clear();
                container.gui.songList.Rows.InsertRange(0, rows.ToArray());
                GuiControl.sort(container, container.trackers.lastSortParam);
            }
        }

        public static void setupSearch(Mp3_Container container)
        {
            container.songlists.searchList.Clear();
            Tuple<String, int> field = null;
            switch (container.gui.searchFieldComboBox.SelectedItem.ToString())
            {
                case "Song Name":
                    field = container.positionVariables.NAME;
                    break;
                case "Length":
                    field = container.positionVariables.LENGTH;
                    break;
                case "Artist":
                    field = container.positionVariables.ARTIST;
                    break;
                case "Album":
                    field = container.positionVariables.ALBUM;
                    break;
                case "Genre":
                    field = container.positionVariables.GENRE;
                    break;
                case "Plays":
                    field = container.positionVariables.PLAYS;
                    break;
                case "Rating":
                    field = container.positionVariables.RATING;
                    break;
                case "Year":
                    field = container.positionVariables.YEAR;
                    break;
            }

            search(container, field);
        }
    }
}
