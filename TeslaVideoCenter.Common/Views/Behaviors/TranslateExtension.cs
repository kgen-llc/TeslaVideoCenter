using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using TeslaVideoCenter.Common.Services;

namespace TeslaVideoCenter.Common.Views.Behaviors
{
    public class TranslateExtension : MarkupExtension
    {
        public TranslateExtension(string key)
        {
            this.Key = key;
        }

        public string Key { get; set; }

        public string FallBack { get; set; }

        public string Context { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new ReflectionBindingExtension($"[{Key}]")
            {
                Mode = BindingMode.OneWay,
                Source = new StringResources(),
                FallbackValue = FallBack ?? Key,
            };

            return binding.ProvideValue(serviceProvider);
        }
    }

}