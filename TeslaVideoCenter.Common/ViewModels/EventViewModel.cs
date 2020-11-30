using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public EventViewModel(Event @event) {
            this.Event = @event;

            this.GenerateOverallVideoCommand = ReactiveCommand.Create(GenerateOverallVideo);
        }

        public Event Event {get;}

        public ICommand GenerateOverallVideoCommand {get;}

        private Task GenerateOverallVideo() {
            return Task.CompletedTask;
        }
    }

}