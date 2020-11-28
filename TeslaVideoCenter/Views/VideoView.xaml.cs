using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using TeslaVideoCenter.ViewModels;

namespace TeslaVideoCenter.Views
{
    public class VideoView : UserControl
    {
        private LibVLCSharp.Avalonia.VideoView VlcVideoView;
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;


        public VideoView()
        {
            InitializeComponent();

              VlcVideoView = this.Get<LibVLCSharp.Avalonia.VideoView>("VideoView");

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            VlcVideoView.MediaPlayer = _mediaPlayer;
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
            if (!VlcVideoView.MediaPlayer.IsPlaying && video != null)
            {
                VlcVideoView.MediaPlayer.Play(new Media(_libVLC,video[0], FromType.FromPath));
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            VlcVideoView.MediaPlayer.Pause();
        }
    }
}