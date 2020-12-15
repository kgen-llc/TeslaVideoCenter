// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicData.Binding;
using TeslaVideoCenter.Common.Services;

namespace TeslaVideoCenter.Models
{

    public class Event
    {

        public Event(string eventJson)
        {
            var document = JsonDocument.Parse(File.ReadAllText(eventJson));
            this.Reason = document.RootElement.GetProperty("reason").GetString();
            this.City = document.RootElement.GetProperty("city").GetString();
            this.Date = document.RootElement.GetProperty("timestamp").GetDateTime();

            this.VideosDirectory = Path.GetDirectoryName(eventJson);

            this.RawName = Directory.GetParent(eventJson).Name;

            this.Videos = new ObservableCollectionExtended<Video>();

            Task.Run(async () =>
            {
                await foreach (var video in GetVideos())
                {
                    this.Videos.Add(video);
                }

                CheckForFullEvent();
            });
        }

        private async IAsyncEnumerable<Video> GetVideos()
        {
            foreach (var file in Directory
                .GetFiles(this.VideosDirectory, "*-*.mp4")
                .GroupBy(getVideoName))
            {
                var v = new Video(file.Key, file);
                await v.Initialized;
                yield return v;
            }

            string getVideoName(string filePath)
            {
                var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                return fileNameOnly.Substring(fileNameOnly.LastIndexOf('-') + 1);
            }
        }

        public string RawName { get; }

        public string VideosDirectory { get; }

        public string Reason { get; }

        public string City { get; }

        public DateTime Date { get; }

        public string Information
        {
            get
            {
                if (File.Exists(InformationFileName))
                    return File.ReadAllText(Path.Combine(VideosDirectory, "tvc.information"));
                else
                    return string.Empty;
            }
            set
            {
                File.WriteAllText(InformationFileName, value);
            }
        }

        private string InformationFileName
        {
            get { return Path.Combine(VideosDirectory, "tvc.information"); }
        }

        public ObservableCollectionExtended<Video> Videos { get; }

        public void CheckForFullEvent()
        {
            var fullEvent = Path.Combine(this.VideosDirectory, VideoManager.FullEventVideo);

            if (File.Exists(fullEvent))
            {
                this.Videos.Add(new Video("Full Event", fullEvent));
            }
        }
    }
}