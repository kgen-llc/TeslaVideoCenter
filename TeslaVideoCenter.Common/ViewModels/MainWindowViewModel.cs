using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReactiveUI;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EventsRepository repository;
        private Event currentEvent;

        private Video currentVideo;

        public MainWindowViewModel()
        {
            var document = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            
            // on Mac, endure we have the documents folder
            if(!document.EndsWith(Path.PathSeparator + "Documents")) {
                document = Path.Combine(document, "Documents");
            }
            this.repository = new EventsRepository( Path.Combine( document, "TeslaCam"));

            this.EventsViewModel = new EventsViewModel(this.repository);
        }

        public EventsViewModel EventsViewModel { get; }

        public Event CurrentEvent { get => this.currentEvent; set => this.RaiseAndSetIfChanged(ref this.currentEvent, value); }
        public Video CurrentVideo { get => this.currentVideo; set => this.RaiseAndSetIfChanged(ref this.currentVideo, value); }
    }
}
