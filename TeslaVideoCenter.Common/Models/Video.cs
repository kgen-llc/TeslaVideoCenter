using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using TeslaVideoCenter.Common.Services;

namespace TeslaVideoCenter.Models
{

    public class Video : ReactiveObject
    {

        public Video(string name, IEnumerable<string> filePath)
        {
            this.FilePath = filePath.OrderBy(_ => _).ToArray();

            this.RawName = name;

            this.IsAlreadyProcess = false;

            this.TotalTime = VideoManager.GetAccumulatedTime(this.FilePath);
            this.IsCorrupted = this.TotalTime == null;
        }

        public Video(string name, string filePath)
        {
            this.RawName = name;
            this.FilePath = new[] { filePath };

            this.IsAlreadyProcess = true;
        }

        public string[] FilePath { get; }

        public string RawName { get; }

        public bool IsAlreadyProcess { get; }
        public TimeSpan? TotalTime { get; }
        public bool IsCorrupted { get; }

    }
}