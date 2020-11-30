using System.IO;
using Avalonia.Controls;
using FFMpegCore;
using FFMpegCore.Helpers;

namespace TeslaVideoCenter
{
    public static class AppBuilderExtensions
    {
        public static T UseFFMpeg<T>(this AppBuilderBase<T> b, FFMpegOptions fFMpegOptions)
            where T : AppBuilderBase<T>, new()
        {
            var dummy = Directory.GetCurrentDirectory();
            return b.AfterSetup(_ => {
                
                FFMpegOptions.Configure(fFMpegOptions);
                FFMpegHelper.VerifyFFMpegExists();
            });
        }
    }
}
