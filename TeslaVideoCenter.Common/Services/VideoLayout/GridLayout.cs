// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Arguments;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.Services.VideoLayout
{
    public class GridLayout : TransformVideo
    {
        public static async Task Process(Action<double> progress,Event @event)
        {
            using var processing = new GridLayout();

            await processing.CreateOutput(progress, @event.Videos, Path.Combine(@event.VideosDirectory, VideoManager.FullEventVideo));
        }

        protected override void ApplyOption(IReadOnlyCollection<Video> videos, FFMpegArgumentOptions options)
        {
            options.WithArgument(new GridArragment(videos.Count));
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

                while (videoIndex < this.NumberOfVideos)
                {
                    var currentColumnIndex = 0;
                    var rowName = 'a' + rowIndex++;
                    verticalStack += "[" + rowName + "]";
                    var horizontalStack = string.Empty;

                    while (currentColumnIndex < numberOfColumns && currentColumnIndex < this.NumberOfVideos)
                    {
                        horizontalStack += "[" + videoIndex + "]";
                        videoIndex++;
                        currentColumnIndex++;
                    }
                    filters.Add(horizontalStack + "hstack[" + rowName + "]");
                }

                filters.Add(verticalStack + "vstack");

                this.Text = "-filter_complex \"" + string.Join(';', filters) + "\"";

            }

            public int NumberOfVideos { get; }

            public string Text { get; }
        }
    }
}