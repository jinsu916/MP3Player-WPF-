using HundredMilesSoftware.UltraID3Lib;
using Microsoft.Win32;
using NAudio.Wave;
using JLyrics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Player
{
     class MusicList : ObservableCollection<Music>
    {
        public MusicList()
        {
            //Add(new Music() { Title = "노래제목", Artist = "가수", Album = "앨범", Genre = "장르" });
        }
    }
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private Lyrics fJL = new Lyrics();
        public delegate void timeTick();
        public UserControl1 user = null;
        DispatcherTimer ticks = new DispatcherTimer();
        Random random = new Random();
        MediaPlayer mp3 = new MediaPlayer();
        int musicplay = 0;
        bool Loop_play = true;
        bool Random_Play = false;

        TimeSpan beforeposition = TimeSpan.Parse("00:00:00");
        TimeSpan newTimeSpan = TimeSpan.Parse("00:00:00");

        int i;
        
        public MainWindow()
        {
            InitializeComponent();

            user = new UserControl1(this);
            MusicSearch();
        }

        
        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        void MusicSearch()
        {
            //Bitmap mpImgBitmap;
            MusicList musiclist = (MusicList)FindResource("musiclist");
            //string[] files = Directory.GetFiles("C:\\Users\\user\\Desktop\\Player1-master\\Player1-master\\Player\\Music\\", "*.mp3", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles("C:\\Users\\jinsu\\OneDrive\\문서\\GitHub\\MP3Player-WPF-\\Music\\", "*.mp3", SearchOption.AllDirectories);
        //C: \Users\jinsu\OneDrive\문서\GitHub\MP3Player - WPF -\Music
            string AlbumArtFileName = string.Empty;
            //string AlbumArtFilePath = "C:\\Users\\user\\Desktop\\Player1 - master\\Player1 - master\\Player\\Music";
            foreach (string file in files)
            {
                LyricInfo xLyricInfo = new LyricInfo();
                xLyricInfo = fJL.GetLyricsFromFile(file);

                FileInfo tempMp3 = new FileInfo(file);

                UltraID3 myFile = new UltraID3();

                myFile.Read(tempMp3.FullName);

                //string album = xLyricInfo.Album;
                //string title = xLyricInfo.Title;
                //string artist = xLyricInfo.Artist;

                string album = myFile.ID3v2Tag.Album;
                string title = myFile.ID3v2Tag.Title;
                string artist = myFile.ID3v2Tag.Artist;
                string genre = myFile.ID3v2Tag.Genre;
                short? year = myFile.ID3v2Tag.Year;
                TimeSpan runtime = new Mp3FileReader(file).TotalTime;



                string[] result1 = file.Split('\\');
                string[] result2 = result1[4].Split('.');
                Music music = new Music()
                {
                    File = file,
                    Album = album,
                    Title = result2[0],
                    Artist = artist,
                    Genre = genre,
                    Year = year,
                    RunTime = runtime,
                    Lyrics = xLyricInfo.Lyric
                };
                musiclist.Add(music);
            }
        }

        //private void Filesearch_Click(object sender, RoutedEventArgs e)
        //{

        //    string mp3file = string.Empty;
        //    OpenFileDialog fileDialog = new OpenFileDialog
        //    {
        //        Multiselect = false,
        //        DefaultExt = ".mp3"
        //    };
        //    bool? dialogOk = fileDialog.ShowDialog();
        //    if (dialogOk == true)
        //    {
        //        filename = fileDialog.FileName;
        //        TBFileName.Text = fileDialog.SafeFileName;

        //        mp3.Open(new Uri(filename));
        //    }
        //}

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (musicplay == 0)
            {
                mp3.Play();
                //mp3.SpeedRatio = mp3.SpeedRatio * 20;
                playstop.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                musicplay = 1;
                
            }
            else if (musicplay == 1)
            {
                mp3.Pause();
                playstop.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                musicplay = 0;
            }
        }

        private void Musiclistview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            i = 0;
            musiclytext.Text = "간주중...";
            MusicList mlist = (MusicList)FindResource("musiclist");
            if (mlist != null)
            {
                beforeposition = TimeSpan.Parse("00:00:00");
                string filename;
                int idx = musiclistview.SelectedIndex;

                filename = mlist[idx].File;

                Control.Instance.SplitLyrics(mlist[idx].Lyrics);

                mp3.Open(new Uri(filename));
                mp3.MediaOpened += loadedMusic;
                mp3.Play();
                musicplay = 1;
                playstop.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                nowmusic.Text = mlist[idx].Title;
            }



            /*private void Proxima_Click(object sender, RoutedEventArgs e)
            {
                if (c1.Offset >= 0)
                {
                    c1.Offset -= 0.01;
                    c2.Offset -= 0.01;
                }
                else
                {
                    c1.Offset = 1;
                    c2.Offset = 0.89;
                }
            }

            private void Anterior_Click(object sender, RoutedEventArgs e)
            {
                if (c2.Offset <= 1)
                {
                    c1.Offset += 0.01;
                    c2.Offset += 0.01;
                }
                else
                {
                    c1.Offset = 0.11;
                    c2.Offset = 0;
                }
            }*/
        }

        private void Anterior_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                view.MoveCurrentToPrevious();
            }
            catch(Exception)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                view.MoveCurrentToFirst();
            }
        }

        private void Proxima_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                RandomOption();
            }
            catch (Exception)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                if (Loop_play == true)
                {
                    view.MoveCurrentToFirst();
                }
                else if (Loop_play == false)
                {
                    view.MoveCurrentToLast();
                }
            }
        }
     
        #region 슬라이더 이동 함수
        private void loadedMusic(object sender, EventArgs e)
        {          
            musicSlider.Maximum = mp3.NaturalDuration.TimeSpan.TotalMilliseconds;
            totalTimeLabel.Content = mp3.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            ticks.Interval = TimeSpan.FromMilliseconds(1);
            ticks.Tick += ticks_Tick;
            ticks.Start();
        }

        void ticks_Tick(object sender, object e)
        {
            try
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                MusicList musiclist = (MusicList)FindResource("musiclist");
                newTimeSpan = mp3.Position;
                musicSlider.Value = newTimeSpan.TotalMilliseconds;

                int count = Control.Instance.lyricslist.Count;
                if (beforeposition > newTimeSpan)
                {
                    if (i > 0)
                    {
                        if (beforeposition < Control.Instance.lyricslist[i].Time)
                        {
                            i--;
                            if (i != 0)
                                musiclytext.Text = Control.Instance.lyricslist[i].Lyrics;
                        }
                        else if (beforeposition < Control.Instance.lyricslist[i].Time && i == 1)
                        {
                            musiclytext.Text = Control.Instance.lyricslist[0].Lyrics;
                        }
                    }
                }
                else
                {
                    if (i < count)
                    {
                        if (Control.Instance.lyricslist[i].Time < newTimeSpan)
                        {
                            musiclytext.Text = Control.Instance.lyricslist[i].Lyrics;
                            if (i != count)
                                i++;
                        }
                        else if (Control.Instance.lyricslist[i].Time < newTimeSpan && i == count - 1)
                        {
                            musiclytext.Text = Control.Instance.lyricslist[count].Lyrics;
                        }
                    }
                }

                progressLabel.Content = newTimeSpan.ToString("mm':'ss");
                if (progressLabel.Content.ToString() == totalTimeLabel.Content.ToString())
                {
                    RandomOption();
                }
                beforeposition = newTimeSpan;

            }
            catch 
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                if (Loop_play == true)
                {
                    view.MoveCurrentToFirst();
                }
                else if (Loop_play == false)
                {
                    view.MoveCurrentToLast();
                }
            }
        }
        public void RandomOption()
        {
            try
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                MusicList musiclist = (MusicList)FindResource("musiclist");
                if (Random_Play == false)
                {
                    view.MoveCurrentToNext();
                }
                else if (Random_Play == true)
                {
                    view.MoveCurrentToPosition(random.Next(0, musiclist.Count() - 1));
                }
            }
            catch
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(FindResource("musiclist"));
                beforeposition = TimeSpan.Parse("00:00:00");
                if (Loop_play == true)
                {
                    view.MoveCurrentToFirst();
                }
                else if (Loop_play == false)
                {
                    view.MoveCurrentToLast();
                }
            }
        }
        #endregion

        //#region 볼륨 조절 함수
        //private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    volumeSlider.Maximum = 100.0;
        //    volumeSlider.Minimum = 0;

        //    mp3.Volume = volumeSlider.Value / 100.0;
        //    //volumeLabel.Content = "Volume : " + (int)volumeSlider.Value;
        //}
        //#endregion

        #region 슬라이드 이동으로 음악위치 바뀌는함수
        private void MusicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan newTimeSpan = mp3.Position;
            progressLabel.Content = newTimeSpan.ToString(@"mm\:ss");
        }
        private void MusicSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            mp3.Position = new TimeSpan(0, 0, 0, 0, (int)musicSlider.Value);

            TimeSpan newTimeSpan = mp3.Position;
            progressLabel.Content = newTimeSpan.ToString(@"mm\:ss");        
        }

        #endregion

        private void Loopbutton_Click(object sender, RoutedEventArgs e)
        {
            if (Loop_play == true)
            {
                loopicon.Kind = MaterialDesignThemes.Wpf.PackIconKind.StopCircleOutline;
                Loop_play = false;
            }
            else if(Loop_play == false)
            {
                loopicon.Kind = MaterialDesignThemes.Wpf.PackIconKind.RotateLeft;
                Loop_play = true;
            }
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            if (Random_Play == false)
            {
                randomplay.Kind = MaterialDesignThemes.Wpf.PackIconKind.ShuffleVariant;
                Random_Play = true;
            }
            else if(Random_Play == true)
            {
                randomplay.Kind = MaterialDesignThemes.Wpf.PackIconKind.ShuffleDisabled;
                Random_Play = false;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //로그인
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(user);
        }

        private void ButtonMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Minimized : WindowState.Normal;
        }
    }
}
