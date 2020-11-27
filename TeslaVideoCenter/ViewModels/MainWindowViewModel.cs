using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EventsRepository repository;
        private Event currentEvent;
        public MainWindowViewModel()
        {
            this.repository = new EventsRepository("/Users/GeoVah/Documents/TeslaCam");

            this.EventsViewModel = new EventsViewModel(this.repository);
        }

        public EventsViewModel EventsViewModel { get; }

        public Event CurrentEvent { get => this.currentEvent; set => this.RaiseAndSetIfChanged(ref this.currentEvent, value); }
    }
}
