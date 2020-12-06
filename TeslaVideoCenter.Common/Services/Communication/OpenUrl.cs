using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TeslaVideoCenter.Common.Services.Communication
{
    static class OpenUrl
    {
        public static void WithSystemBrowser(string url)
        {
#pragma warning disable  CS0642 // We wait on process
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

         public static  async Task<string> AndReturnContent(string url) {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
         }
    }
}