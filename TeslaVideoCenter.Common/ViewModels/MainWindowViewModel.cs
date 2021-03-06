﻿// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.IO;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly EventsRepository repository;
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

        }

        public EventsViewModel EventsViewModel { get; }
    }
}
