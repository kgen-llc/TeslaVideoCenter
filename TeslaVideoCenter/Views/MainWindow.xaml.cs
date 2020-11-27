using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using TeslaVideoCenter.ViewModels;

namespace TeslaVideoCenter.Views
{
    public class MainWindow : Window
    {
        private VideoView VideoView;
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        public MainWindow()
        {
            InitializeComponent();

             VideoView = this.Get<VideoView>("VideoView");

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            VideoView.MediaPlayer = _mediaPlayer;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }



        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView.MediaPlayer.IsPlaying)
            {
                VideoView.MediaPlayer.Stop();
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var video = ((MainWindowViewModel)this.DataContext).CurrentVideo;
            if (!VideoView.MediaPlayer.IsPlaying && video != null)
            {
                VideoView.MediaPlayer.Play(new Media(_libVLC,video.FilePath, FromType.FromPath));
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            VideoView.MediaPlayer.Pause();
        }
    }
}