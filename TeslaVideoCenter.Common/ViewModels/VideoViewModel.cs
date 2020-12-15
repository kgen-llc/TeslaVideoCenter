// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System.Globalization;
using ReactiveUI;
using TeslaVideoCenter.Common.Services;
using TeslaVideoCenter.Models;

namespace TeslaVideoCenter.Common.ViewModels
{
    public class VideoViewModel : ViewModelBase
    {

        public VideoViewModel(Video video) {
            Video = video;

            this.stringResources = new StringResources();

            this.isNotCorruptedHelper = this.Video
                .WhenAnyValue(_ => _.IsCorrupted, _ => !_)
                .ToProperty(this, nameof(this.IsNotCorrupted));

            this.nameHelper = this.Video
                .WhenAnyValue(_ => _.RawName, _ => _.FilePath.Length, calculateName)
                .ToProperty(this, nameof(this.Name));

            string calculateName(string rawName, int fileCount) {
                return string.Format(CultureInfo.CurrentCulture, this.stringResources.GetResource("VideoNameWithPartsCount"), rawName, fileCount);
            }
        }

        public Video Video { get; }

        private readonly StringResources stringResources;
        private readonly ObservableAsPropertyHelper<string> nameHelper;

        public string Name {get => nameHelper.Value; }

         private readonly ObservableAsPropertyHelper<bool> isNotCorruptedHelper;

        public bool IsNotCorrupted {get => isNotCorruptedHelper.Value; }
    }

}