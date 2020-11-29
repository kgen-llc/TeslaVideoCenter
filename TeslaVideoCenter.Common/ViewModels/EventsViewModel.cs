
using DynamicData.Binding;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class EventsViewModel : ViewModelBase
    {
        public EventsViewModel(EventsRepository repository) {
            this.Events = repository.Events;
        }

        public ObservableCollectionExtended<Event> Events { get; }
    }
}