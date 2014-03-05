using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Details
{
    public class SongDetails
    {
        public string SongName { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string Length { get; set; }
        public int Rating { get; set; }
        public long Plays { get; set; }
        public string Path { get; set; }
        public string Track { get; set; }
        public string Year { get; set; }

        public SongDetails(string songName, string artist, string[] contributing, string album, string genre, string length, int rating, long plays, string path, string track, string year)
        {
            SongName = songName;
            if (contributing == null)
            {
                Artist = artist;
            }
            else
            {
                Artist = artist + " ft. ";
                int count = 0;
                foreach (string name in contributing)
                {
                    count++;
                    if (count == contributing.Length)
                    {
                        Artist += name;
                    }
                    else
                    {
                        Artist += name + ", ";
                    }
                }
            }
            Album = album;
            Genre = genre;
            Length = length.Replace('.', ' ').Split(' ')[0].Trim();
            Rating = rating;
            Plays = plays;
            Path = path;
            Track = track;
            Year = year;
        }

        public bool equals(SongDetails song)
        {
            if(SongName == song.SongName && Artist == song.Artist && Album == song.Album && 
                Genre == song.Genre && Length == song.Length && Rating == song.Rating && 
                Plays == song.Plays && Path == song.Path && Track == song.Track && Year == song.Year)
            {
                return true;
            }
            return false;
        }
    }
}
