

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TeslaVideoCenter.Common.Services
{
    static class OpenUrl
    {
        public static void WithSystemBrowser(string url)
        {
#pragma warning disable  CS0642 - We wait on process
            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
                Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"-e {url}" : "",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            }));
#pragma warning restore CS0642
        }
    }
}