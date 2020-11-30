using System;
using System.Collections.Generic;
using System.Linq;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.ViewModels
{
    public class MonitorNewProcessedVideo : IObservable<bool>
    {
        public MonitorNewProcessedVideo(Event @event)
        {
            observers = new List<IObserver<bool>>();

            @event.Videos.CollectionChanged += (sender, args) =>
            {
                this.UpdateState(CanProcessVideo());
            };
            this.@event = @event;
        }

        private List<IObserver<bool>> observers;
        private readonly Event @event;

        private bool CanProcessVideo() {
            return this.@event.Videos.All(_ => !_.IsAlreadyProcess);
        }

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            observer.OnNext(@event.Videos.All(_ => !_.IsAlreadyProcess));
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<bool>> _observers;
            private IObserver<bool> _observer;

            public Unsubscriber(List<IObserver<bool>> observers, IObserver<bool> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
        public void UpdateState(bool videoIsProcessed)
        {
            foreach (var obs in this.observers)
            {
                obs.OnNext(videoIsProcessed);
            }
        }
    }
}
