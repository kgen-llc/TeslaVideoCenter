// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TeslaVideoCenter.Common.Views
{
    public class EventInfoView : UserControl
    {
        public EventInfoView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}