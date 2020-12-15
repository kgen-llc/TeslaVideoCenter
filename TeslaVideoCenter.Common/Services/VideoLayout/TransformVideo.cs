// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMpegCore;
using TeslaVideoCenter.Models;


namespace TeslaVideoCenter.Common.Services.VideoLayout
{
    public abstract class TransformVideo : IDisposable
    {
        private string workingFolder;
        protected TransformVideo()
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

        protected async Task CreateOutput(Action<double> progress, IReadOnlyList<Video> videos, string outputFile)
        {
            await Task.WhenAll(videos.Select(this.CreateOutput));

            FFMpegCore.FFMpegArguments ffmpeg = null;
            foreach (var video in videos)
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

            try {
                var duration = (await FFProbe.AnalyseAsync(GetCombineVideoPath(videos[0]))).Duration;
                await ffmpeg.OutputToFile(outputFile, true,_ => this.ApplyOption(videos, _))
                    .NotifyOnProgress(progress, duration)
                     .ProcessAsynchronously();
            }
            catch( Exception e) {
                Console.Error.WriteLine(e.ToString());
            }
        }

        protected abstract void ApplyOption(IReadOnlyCollection<Video> videos, FFMpegArgumentOptions options);

        private Task<bool> CreateOutput(Video video)
        {
            return Task.Run(() =>
                FFMpegCore.FFMpeg.Join(GetCombineVideoPath(video), video.FilePath));
        }

        private string GetCombineVideoPath(Video video)
        {
            return Path.Join(this.workingFolder, video.RawName + ".mp4");
        }
    }
}