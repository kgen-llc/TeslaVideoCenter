using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DynamicData.Binding;
using TeslaVideoCenter.Services;

namespace TeslaVideoCenter.Models
{

    public class Event {

        public Event(string eventJson) {
            var document = JsonDocument.Parse(File.ReadAllText(eventJson));
            this.Reason = document.RootElement.GetProperty("reason").GetString();
            this.City = document.RootElement.GetProperty("city").GetString();
            this.Date = document.RootElement.GetProperty("timestamp").GetDateTime();

            this.VideosDirectory = Path.GetDirectoryName(eventJson);

            this.RawName = Directory.GetParent(eventJson).Name;


            this.Videos = new ObservableCollectionExtended<Video>(
                GetVideos(this.VideosDirectory)
            );

           CheckForFullEvent();

        }

        private IEnumerable<Video> GetVideos(string folder) {
          
            return Directory
                .GetFiles(folder, "*-*.mp4")
                .GroupBy(getVideoName)
                .Select(_ => new Video(_.Key, _));

            string getVideoName(string filePath) {
                var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
                return fileNameOnly.Substring(fileNameOnly.LastIndexOf('-') + 1);
            }
        }

        public string RawName {get;}

        public string VideosDirectory { get;}

        public string Reason { get;}

        public string City {get;}

        public DateTime Date {get;}

        public string Information { get; set;}

        public ObservableCollectionExtended<Video> Videos { get; }

        public void CheckForFullEvent(){
             var fullEvent = Path.Combine(this.VideosDirectory, VideoManager.FullEventVideo);

            if(File.Exists(fullEvent)) {
                this.Videos.Add(new Video("Full Event", fullEvent));
            }
        }
    }
}