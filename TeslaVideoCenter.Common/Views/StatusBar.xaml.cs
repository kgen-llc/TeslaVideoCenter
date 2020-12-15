// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TeslaVideoCenter.Common.ViewModels;

namespace TeslaVideoCenter.Common.Views
{
    public class StatusBar : UserControl
    {
        public StatusBar()
        {
            InitializeComponent();
            this.DataContext = new StatusBarViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}