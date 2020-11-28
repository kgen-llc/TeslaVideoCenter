using System.Collections.Generic;
using System.Linq;

namespace TeslaVideoCenter.Models
{

    public class Video {

        public Video(string name, IEnumerable<string> filePath) {
            this.FilePath = filePath.OrderBy(_ => _).ToArray();
            this.Name = name;
        }

        public string[] FilePath {get;}

        public string Name { get;}

    }
}