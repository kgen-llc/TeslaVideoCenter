// Copyright (c) Frederic Forjan 
// Licensed under MIT License

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
            this.IsCorrupted = true;
            this.Initialized = Initializing();
        }

        public Video(string name, string filePath)
        {
            this.RawName = name;
            this.FilePath = new[] { filePath };

            this.IsAlreadyProcess = true;
            this.IsCorrupted = true;
            this.Initialized = Initializing();
        }

        private  Task Initializing() {
            return Task.Run( async () => {
                this.TotalTime = await VideoManager.GetAccumulatedTimeAsync(this.FilePath);
                this.RaisePropertyChanged(nameof(this.TotalTime));
                this.IsCorrupted = this.TotalTime == null;
                this.RaisePropertyChanged(nameof(this.IsCorrupted));
            });
        }

        public string[] FilePath { get; }

        public string RawName { get; }

        public bool IsAlreadyProcess { get; }
        public TimeSpan? TotalTime { get; private set; }
        public bool IsCorrupted { get; private set; }

        public Task Initialized {get;}
    }
}