using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using DynamicData.Binding;

namespace TeslaVideoCenter.Models
{

    public class Event {

        public Event(string eventJson) {
            var document = JsonDocument.Parse(File.ReadAllText(eventJson));
            this.Reason = document.RootElement.GetProperty("reason").GetString();
            this.City = document.RootElement.GetProperty("city").GetString();
            this.Date = document.RootElement.GetProperty("timestamp").GetDateTime();

            var directory = Path.GetDirectoryName(eventJson);

            this.Videos = new ObservableCollectionExtended<Video>(
                Directory
                .GetFiles(directory, "*.mp4")
                .Select(_ => new Video(_))
            );

        }
        public string Reason { get;}

        public string City {get;}

        public DateTime Date {get;}

        public ObservableCollectionExtended<Video> Videos { get; }
    }
}