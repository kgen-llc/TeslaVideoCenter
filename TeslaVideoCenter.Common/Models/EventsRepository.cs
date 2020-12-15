// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System.IO;
using System.Linq;
using DynamicData.Binding;

namespace TeslaVideoCenter.Models
{
    public class EventsRepository  {

        public EventsRepository(string folder) {

            // ensure the directory exists
            if(!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }

            this.Events = new ObservableCollectionExtended<Event>(
                Directory
                    .GetFiles(folder, "event.json", SearchOption.AllDirectories)
                    .Select(_ => new Event(_)));

        }

        public ObservableCollectionExtended<Event> Events {get; }

    }
}