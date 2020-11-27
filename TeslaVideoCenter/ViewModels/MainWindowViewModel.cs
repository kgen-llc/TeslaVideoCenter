using System;
using System.Collections.Generic;
using System.Text;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EventsRepository repository;

        public MainWindowViewModel() {
            this.repository = new EventsRepository("/Users/GeoVah/Documents/TeslaCam");

            this.EventsViewModel = new EventsViewModel(this.repository);
        }

        public EventsViewModel EventsViewModel {get;} 
    }
}
