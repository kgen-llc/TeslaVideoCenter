using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMpegCore.Arguments;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Services
{

    public class HorizontalStack : IArgument
    {
        public HorizontalStack(int numberOfVideos) {
            this.NumberOfVideos = numberOfVideos;
        }

        public int NumberOfVideos {get;}

        public string Text => $" -filter_complex hstack=inputs={NumberOfVideos}";
    }
    public class TransformVideo : IDisposable
    {

        private string workingFolder;
        private TransformVideo()
        {

            this.workingFolder = getTemporaryDirectory();

            string getTemporaryDirectory()
            {
                string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDirectory);
                return tempDirectory;
            }
        }

        public void Dispose()
        {
            Directory.Delete(this.workingFolder, true);
        }

        private async Task CreateOutput(Event @event)
        {
            await Task.WhenAll(@event.Videos.Select(this.CreateOutput));

            FFMpegCore.FFMpegArguments ffmpeg = null;
            foreach (var video in @event.Videos)
            {
                if (ffmpeg == null)
                {
                    ffmpeg = FFMpegCore.FFMpegArguments.FromFileInput(GetCombineVideoPath(video));
                }
                else
                {
                    ffmpeg = ffmpeg.AddFileInput(GetCombineVideoPath(video));
                }
            }
            

            await ffmpeg.OutputToFile(Path.Combine(@event.VideosDirectory, VideoManager.FullEventVideo), true,
            options => options.WithArgument(new HorizontalStack(@event.Videos.Count))
            )
                     .ProcessAsynchronously();
        }

        private Task<bool> CreateOutput(Video video)
        {
            return Task.Run(() =>
                FFMpegCore.FFMpeg.Join(GetCombineVideoPath(video), video.FilePath));
        }

        private string GetCombineVideoPath(Video video)
        {
            return Path.Join(this.workingFolder, video.RawName + ".mp4");
        }


        public static async Task Process(Event @event) {
            using var processing = new TransformVideo();
            
            await processing.CreateOutput(@event);
        }

    }
}