
using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        private readonly ReadOnlyObservableCollection<EventViewModel> events;

        public EventsViewModel(EventsRepository repository) {
            repository.Events.ToObservableChangeSet()   
                .Transform(@event => new EventViewModel(@event))
                // No need to use the .ObserveOn() operator here, as
                // ObservableCollectionExtended is single-threaded.
                .Bind(out this.events)
                .Subscribe();
        }

        public ReadOnlyObservableCollection<EventViewModel> Events { get => events; }
    }
}