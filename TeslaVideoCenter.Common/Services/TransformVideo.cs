using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMpegCore.Arguments;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.Services
{

    public class HorizontalStack : IArgument
    {
        public HorizontalStack(int numberOfVideos)
        {
            this.NumberOfVideos = numberOfVideos;
        }

        public int NumberOfVideos { get; }

        public string Text => $" -filter_complex hstack=inputs={NumberOfVideos}";
    }

    public class GridArragment : IArgument
    {
         public GridArragment(int numberOfVideos)
        {
            this.NumberOfVideos = numberOfVideos;
            var numberOfColumns = Math.Ceiling(Math.Sqrt(this.NumberOfVideos));

            var videoIndex = 0;
            var rowIndex = 0;

            var filters = new List<string>();

            var verticalStack = string.Empty;

            while(videoIndex < this.NumberOfVideos) {
                var currentColumnIndex = 0;
                var rowName = 'a' + rowIndex ++;
                verticalStack += "[" + rowName +"]";
                var horizontalStack = string.Empty;

                while(currentColumnIndex < numberOfColumns && currentColumnIndex < this.NumberOfVideos) {
                    horizontalStack += "[" + videoIndex  + "]"; 
                    videoIndex++;
                    currentColumnIndex++;
                }
                filters.Add(horizontalStack + "hstack[" + rowName + "]");
            }

            filters.Add(verticalStack + "vstack");

            this.Text = "-filter_complex \"" + string.Join(';', filters) +"\"";

        }

        public int NumberOfVideos { get; }

        public string Text {get;}
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
            options => options.WithArgument(new GridArragment(@event.Videos.Count))
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


        public static async Task Process(Event @event)
        {
            using var processing = new TransformVideo();

            await processing.CreateOutput(@event);
        }

    }
}