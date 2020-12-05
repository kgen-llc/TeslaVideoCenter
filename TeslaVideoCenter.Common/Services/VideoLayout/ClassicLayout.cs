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
    public class ClassicLayout : TransformVideo
    {
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

            await processing.CreateOutput(progress,@event.Videos, Path.Combine(@event.VideosDirectory, VideoManager.FullEventVideo));
        }

        protected override void ApplyOption(IReadOnlyCollection<Video> videos, FFMpegArgumentOptions options)
        {
            options.WithArgument(new ClassicLayoutArgument());
        }

        /// <summary>
        /// this is a classic Layout
        /// [blank][0][blank]
        /// [  1  ][2][  3  ]
        /// </summary>
        private class ClassicLayoutArgument : IArgument
        {
            public string Text => @" -filter_complex ""nullsrc=size=960x480 [base];
            [0:v] setpts=PTS-STARTPTS, scale=320x240 [UpperMiddle];
            [1:v] setpts=PTS-STARTPTS, scale=320x240 [LowerLeft];
            [2:v] setpts=PTS-STARTPTS, scale=320x240 [LowerMiddle];
            [3:v] setpts=PTS-STARTPTS, scale=320x240 [LowerRight];
            [base][UpperMiddle] overlay=x=320 [tmp1];
            [tmp1][LowerLeft] overlay=y=240 [tmp2];
            [tmp2][LowerMiddle] overlay=y=240:x=320 [tmp3];
            [tmp3][LowerRight] overlay=x=640:y=240"" ";
        } 
    }
}