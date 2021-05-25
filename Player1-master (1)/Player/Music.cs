using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    class Music : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string title { get; set; }
        private string artist { get; set; }
        private string genre { get; set; }
        private string album { get; set; }
        private short? year { get; set; }
        private string lyrics { get; set; }
        private TimeSpan runtime { get; set; }
        private string file { get; set; }
        public string File
        {
            get { return file; }
            set
            {
                file = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("File"));
            }
        }

        public string Lyrics
        {
            get { return lyrics; }
            set
            {
                lyrics = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Lyrics"));
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }
        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Artist"));
            }
        }
        public string Genre
        {
            get { return genre; }
            set
            {
                genre = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Genre"));
            }
        }
        public string Album
        {
            get { return album; }
            set
            {
                album = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Album"));
            }
        }
        public short? Year
        {
            get { return year; }
            set
            {
                year = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Year"));
            }
        }
        public TimeSpan RunTime
        {
            get { return runtime; }
            set
            {
                runtime = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RunTime"));
            }
        }

        public override string ToString()
        {
            return "제목 : " + Title + " || 가수 : " + Artist + " || 앨범 : " + Album + " || 장르 : " + Genre + " || 연도 : " + Year + " || 재생시간 : " + RunTime;
        }

    }

    class MusicLyrics
    {
        public string Lyrics { get; set; }
        public TimeSpan Time { get; set; }

        public MusicLyrics(string time, string lyrics)
        {
            try
            {
                if (time != "")
                {
                    string time2 = "00:" + time;
                    Lyrics = lyrics;
                    Time = TimeSpan.Parse(time2);
                }
            }
            catch
            {

            }
        }
    }
}
