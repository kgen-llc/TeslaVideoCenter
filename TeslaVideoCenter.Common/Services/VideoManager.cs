// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFMpegCore;

namespace TeslaVideoCenter.Common.Services
{
    public class VideoManager
    {
        public const string FullEventVideo = "fullEvent.mp4";

        public static async Task<TimeSpan?> GetAccumulatedTimeAsync(IEnumerable<string> filePath)
        {
            var totalTime = new TimeSpan();

            foreach (var file in filePath)
            {
                try
                {
                    var info = await FFProbe.AnalyseAsync(file);
                    if (info == null)
                    {
                        return null;
                    }

                    totalTime += info.Duration;
                }
                catch
                {
                    return null;
                }
            }

            return totalTime;
        }

        public static TimeSpan? GetAccumulatedTime(IEnumerable<string> filePath)
        {
            var totalTime = new TimeSpan();

            foreach (var file in filePath)
            {
                try
                {
                    var info = FFProbe.Analyse(file);
                    if (info == null)
                    {
                        return null;
                    }
                    totalTime += info.Duration;
                }
                catch
                {
                    return null;
                }
            }

            return totalTime;
        }
    }
}