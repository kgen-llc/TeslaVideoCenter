using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using TeslaVideoCenter.Models;
using TeslaVideoCenter.Services;
using DynamicData;
using DynamicData.Binding;
using System;

namespace TeslaVideoCenter.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        public EventViewModel(Event @event) {
            this.Event = @event;

            this.GenerateOverallVideoCommand = ReactiveCommand.CreateFromTask(GenerateOverallVideo,new MonitorNewProcessedVideo(@event));
            
        }

        public Event Event {get;}

        public ICommand GenerateOverallVideoCommand {get;}

        private async Task GenerateOverallVideo() {
            await TransformVideo.Process(this.Event);
        }
    }

}