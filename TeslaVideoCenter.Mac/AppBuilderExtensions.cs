using Avalonia.Controls;
using FFMpegCore;
using FFMpegCore.Helpers;

namespace TeslaVideoCenter
{
    public static class AppBuilderExtensions
    {
        public static T UseFFMpeg<T>(this AppBuilderBase<T> b, FFOptions fFMpegOptions)
            where T : AppBuilderBase<T>, new()
        {
            return b.AfterSetup(_ =>
            {  
                GlobalFFOptions.Configure(fFMpegOptions);
                FFMpegHelper.VerifyFFMpegExists(fFMpegOptions);
            });
        }
    }
}
