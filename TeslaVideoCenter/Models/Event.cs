using System;
using System.IO;
using System.Text.Json;

namespace TeslaVideoCenter.Models
{

    public class Event {

        public Event(string eventJson) {
            var document = JsonDocument.Parse(File.ReadAllText(eventJson));
            this.Reason = document.RootElement.GetProperty("reason").GetString();
            this.City = document.RootElement.GetProperty("city").GetString();
            this.Date = document.RootElement.GetProperty("timestamp").GetDateTime();
        }
        public string Reason { get;}

        public string City {get;}

        public DateTime Date {get;}
    }
}