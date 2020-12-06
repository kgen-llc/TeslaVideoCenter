using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using TeslaVideoCenter.Common.Services;
using TeslaVideoCenter.Common.Services.VideoLayout;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private readonly ReadOnlyObservableCollection<VideoViewModel> videos;

        public EventViewModel(Event @event)
        {

            this.stringResources = new StringResources();
            this.Event = @event;

            var videoMonitoring = @event.Videos.ToObservableChangeSet().Transform(video => new VideoViewModel(video))
                // No need to use the .ObserveOn() operator here, as
                // ObservableCollectionExtended is single-threaded.
                .Bind(out this.videos)
                .Subscribe();


            var anyVideoToBeProcessedMonitor = @event.Videos.ToObservableChangeSet()
                .ToCollection()                      // Get the new collection of items
                .Select(x => x.All(y => !y.IsAlreadyProcess));

            this.GenerateOverallVideoCommand = ReactiveCommand.CreateFromTask(GenerateOverallVideo, anyVideoToBeProcessedMonitor);

        }

        private readonly StringResources stringResources;

        public Event Event { get; }

        public string Reason { get { return this.stringResources.GetResource(this.Event.Reason); } }

        public ReadOnlyObservableCollection<VideoViewModel> Videos { get => videos; }

        public ICommand GenerateOverallVideoCommand { get; }

        private double progress;
        public double Progress { get => progress; }

        public void SetProgress(double currentProgress)
        {
            this.RaiseAndSetIfChanged(ref this.progress, currentProgress, nameof(Progress));
        }

        private async Task GenerateOverallVideo()
        {
            await ClassicLayout.Process(this.SetProgress, this.Event);
            this.Event.CheckForFullEvent();
        }
    }

}