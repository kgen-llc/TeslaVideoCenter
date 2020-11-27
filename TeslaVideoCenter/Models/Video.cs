using System.IO;

namespace TeslaVideoCenter.Models
{

    public class Video {

        public Video(string filePath) {
            this.FilePath = filePath;
            var fileNameOnly = Path.GetFileNameWithoutExtension(filePath);
            this.Name = fileNameOnly.Substring(fileNameOnly.LastIndexOf('-') + 1);
        }

        public string FilePath {get;}

        public string Name { get;}

    }
}