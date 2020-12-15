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
    public class HorizontalLayout : TransformVideo
    {
        public static async Task Process(Action<double> progress,Event @event)
        {
            using var processing = new HorizontalLayout();

            await processing.CreateOutput(progress, @event.Videos, Path.Combine(@event.VideosDirectory, VideoManager.FullEventVideo));
        }

        protected override void ApplyOption(IReadOnlyCollection<Video> videos, FFMpegArgumentOptions options)
        {
            options.WithArgument(new HorizontalStack(videos.Count));
        }

        public class HorizontalStack : IArgument
        {
            public HorizontalStack(int numberOfVideos)
            {
                this.NumberOfVideos = numberOfVideos;
            }

            public int NumberOfVideos { get; }

            public string Text => $" -filter_complex hstack=inputs={NumberOfVideos}";
        }
    }

}