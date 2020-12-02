using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using LibVLCSharp.Shared;

namespace TeslaVideoCenter.Common.Views
{
    public class VideoView : UserControl
    {
        private LibVLCSharp.Avalonia.VideoView VlcVideoView;

        private TextBlock CurrentPart;

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        private IEnumerator<string> currentVideo;
        private int currentPart;


        public VideoView()
        {
            InitializeComponent();

            VlcVideoView = this.Get<LibVLCSharp.Avalonia.VideoView>("VideoView");
            CurrentPart = this.Get<Avalonia.Controls.TextBlock>("CurrentPart");
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            this.CurrentPart.Text = "No Video Selected";

            VlcVideoView.MediaPlayer = _mediaPlayer;
            _mediaPlayer.EndReached += (sender, arg) =>
            {
                PlayCurrent();
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (VlcVideoView.MediaPlayer.IsPlaying)
            {
                VlcVideoView.MediaPlayer.Stop();
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var video = this.DataContext as string[];
            if (video == null)
            {
                this.CurrentPart.Text = "No Video Selected";
            }
            else
            {
                this.currentVideo = video.Cast<string>().GetEnumerator();
                this.currentPart = 0;
                if (!VlcVideoView.MediaPlayer.IsPlaying && this.currentVideo != null)
                {
                    PlayCurrent();
                }
            }
        }

        private void PlayCurrent()
        {
            // ensure we are on the UI thread for changing this
            if (Dispatcher.UIThread.CheckAccess())
            {

                if (this.currentVideo.MoveNext())
                {
                    this.currentPart++;
                    VlcVideoView.MediaPlayer.Play(new Media(_libVLC, this.currentVideo.Current, FromType.FromPath));
                }
                else
                {
                    this.currentPart = -1;
                }

                this.CurrentPart.Text = "Part : " + this.currentPart.ToString();
            }
            else
            {
                Dispatcher.UIThread.Post(new Action(this.PlayCurrent));
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            VlcVideoView.MediaPlayer.Pause();
        }
    }
}