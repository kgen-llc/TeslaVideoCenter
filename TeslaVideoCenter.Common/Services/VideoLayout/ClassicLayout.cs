// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFMpegCore;
using FFMpegCore.Arguments;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.Services.VideoLayout
{
    /// <summary>     
    /// this is a classic Layout amd will combine all videos in one of size 960x480 using the layout :
    /// [blank][front][blank]
    /// [left_repeater ][back ][right_repeater  ]
    /// </summary>
    public class ClassicLayout : TransformVideo
    {
        private static readonly ClassicLayoutArgument classicLayoutArgument = new ClassicLayoutArgument();
        
        ///<summary>
        /// Process the event and generate the full event video
        ///</summary>
        public static async Task Process(Action<double> progress, Event @event)
        {
            using var processing = new ClassicLayout();

            // we need to ensure video are in order :
            // front,left,back,right
            var videos = new Video[] {
                @event.Videos.First(_ => _.RawName == "front"),
                @event.Videos.First(_ => _.RawName == "left_repeater"),
                @event.Videos.First(_ => _.RawName == "back"),
                @event.Videos.First(_ => _.RawName == "right_repeater")
            };

            await processing.CreateOutput(progress,videos, Path.Combine(@event.VideosDirectory, VideoManager.FullEventVideo));
        }

        protected override void ApplyOption(IReadOnlyCollection<Video> videos, FFMpegArgumentOptions options)
        {
            options.WithArgument(classicLayoutArgument);
        }

        /// <summary>
        /// this is a classic Layout
        /// [blank][0][blank]
        /// [  1  ][2][  3  ]
        /// </summary>
        private class ClassicLayoutArgument : IArgument
        {
            public string Text => @" -filter_complex ""color=s=960x480:c=black [base];
            [0:v] setpts=PTS-STARTPTS, scale=320x240 [UpperMiddle];
            [1:v] setpts=PTS-STARTPTS, scale=320x240 [LowerLeft];
            [2:v] setpts=PTS-STARTPTS, scale=320x240 [LowerMiddle];
            [3:v] setpts=PTS-STARTPTS, scale=320x240 [LowerRight];
            [base][UpperMiddle] overlay=shortest=1:x=320 [tmp1];
            [tmp1][LowerLeft] overlay=shortest=1:y=240 [tmp2];
            [tmp2][LowerMiddle] overlay=shortest=1:y=240:x=320 [tmp3];
            [tmp3][LowerRight] overlay=shortest=1:x=640:y=240"" ";
        } 
    }
}
