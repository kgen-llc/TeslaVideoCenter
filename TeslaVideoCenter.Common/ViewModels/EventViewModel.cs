using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public EventViewModel(Event @event) {
            this.Event = @event;
        }


        public Event Event {get;}
    }

}