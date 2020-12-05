using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TeslaVideoCenter.Models;
using TeslaVideoCenter.Common.Services;
using DynamicData;
using DynamicData.Binding;
using System.Reactive.Linq;
using System.Linq;
using TeslaVideoCenter.Common.Services.VideoLayout;

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

        public string Reason { get { return this.stringResources.GetResource(this.Event.Reason); } }

        public ICommand GenerateOverallVideoCommand { get; }

        private double progress;
        public double Progress {get => progress;}

        public void SetProgress(double currentProgress) {
            this.RaiseAndSetIfChanged(ref this.progress, currentProgress, nameof(Progress));
        }

        private async Task GenerateOverallVideo()
        {
            await GridLayout.Process(this.SetProgress, this.Event);
            this.Event.CheckForFullEvent();
        }
    }

}