// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TeslaVideoCenter.Common.Views
{
    public class HintText : UserControl
    {
        public static readonly AvaloniaProperty<string> TextProperty =
            AvaloniaProperty.RegisterDirect<HintText, string>(
                nameof(Text),
                o => o.Text,
                (o, v) => o.Text = v);

        public HintText()
        {
            InitializeComponent();
        }

        private string text = string.Empty;
        public string Text
        {
            get { return text; }
            set
            { 
                SetAndRaise(TextProperty, ref text, value);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}