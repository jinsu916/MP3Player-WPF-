using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    class Control
    {
        #region 싱글톤 패턴
        public static Control Instance { get; private set; }

        static Control()
        {
            Instance = new Control();
        }

        private Control()
        {

        }
        #endregion 
        public List<MusicLyrics> lyricslist = new List<MusicLyrics>();

        public void SplitLyrics(string lyrics)
        {
            lyricslist.Clear();
            string[] lyrics1 = lyrics.Split('[');

            for (int i = 1; i < lyrics1.Length; i++)
            {
                string[] lyrics2 = lyrics1[i].Split(']');

                MusicLyrics musicLyrics = new MusicLyrics(lyrics2[0], lyrics2[1]);
                lyricslist.Add(musicLyrics);
            }
        }
    }
}
