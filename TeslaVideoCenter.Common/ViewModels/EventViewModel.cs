using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TeslaVideoCenter.Models;
using TeslaVideoCenter.Services;
using DynamicData;
using DynamicData.Binding;
using System.Reactive.Linq;
using System.Linq;

namespace TeslaVideoCenter.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public EventViewModel(Event @event)
        {
            this.Event = @event;

            var anyVideoToBeProcessedMonitor = @event.Videos
                .ToObservableChangeSet()
                .ToCollection()                      // Get the new collection of items
                .Select(x => x.All(y => !y.IsAlreadyProcess));

            this.GenerateOverallVideoCommand = ReactiveCommand.CreateFromTask(GenerateOverallVideo, anyVideoToBeProcessedMonitor);

        }

        public Event Event { get; }

        public ICommand GenerateOverallVideoCommand { get; }

        private async Task GenerateOverallVideo()
        {
            await TransformVideo.Process(this.Event);
            this.Event.CheckForFullEvent();
        }
    }

}