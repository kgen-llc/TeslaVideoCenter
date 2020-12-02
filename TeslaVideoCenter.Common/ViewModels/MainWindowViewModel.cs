﻿using System;
using System.IO;
using System.Windows.Input;
using ReactiveUI;
using TeslaVideoCenter.Models;
using TeslaVideoCenter.Common.Services;

namespace TeslaVideoCenter.Common.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EventsRepository repository;
        private Event currentEvent;

        private Video currentVideo;

        public MainWindowViewModel()
        {
            var document = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            // on Mac, endure we have the documents folder
            if (Path.GetFileName(document) != "Documents")
            {
                document = Path.Combine(document, "Documents");
            }
            this.repository = new EventsRepository(Path.Combine(document, "TeslaCam"));

            this.EventsViewModel = new EventsViewModel(this.repository);

            this.HelpMeCommand = ReactiveCommand.Create(() => OpenUrl.WithSystemBrowser("https://github.com/fforjan/TeslaVideoCenter"));

        }

        public EventsViewModel EventsViewModel { get; }

        public Event CurrentEvent { get => this.currentEvent; set => this.RaiseAndSetIfChanged(ref this.currentEvent, value); }
        public Video CurrentVideo { get => this.currentVideo; set => this.RaiseAndSetIfChanged(ref this.currentVideo, value); }

        public ICommand HelpMeCommand { get; }
    }
}
