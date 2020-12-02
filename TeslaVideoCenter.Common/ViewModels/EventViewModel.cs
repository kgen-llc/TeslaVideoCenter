using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TeslaVideoCenter.Models;
using TeslaVideoCenter.Common.Services;
using DynamicData;
using DynamicData.Binding;
using System.Reactive.Linq;
using System.Linq;

namespace TeslaVideoCenter.Common.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public EventViewModel(Event @event)
        {

            this.stringResources = new StringResources();
            this.Event = @event;

            var anyVideoToBeProcessedMonitor = @event.Videos
                .ToObservableChangeSet()
                .ToCollection()                      // Get the new collection of items
                .Select(x => x.All(y => !y.IsAlreadyProcess));

            this.GenerateOverallVideoCommand = ReactiveCommand.CreateFromTask(GenerateOverallVideo, anyVideoToBeProcessedMonitor);

        }

        private readonly StringResources stringResources;

        public Event Event { get; }

        public string Reason {get { return this.stringResources.GetResource(this.Event.Reason); }}
        
        public ICommand GenerateOverallVideoCommand { get; }

        private async Task GenerateOverallVideo()
        {
            await TransformVideo.Process(this.Event);
            this.Event.CheckForFullEvent();
        }
    }

}