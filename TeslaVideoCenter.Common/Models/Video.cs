using System.Collections.Generic;
using System.Linq;

namespace TeslaVideoCenter.Models
{

    public class Video {

        public Video(string name, IEnumerable<string> filePath) {
            this.FilePath = filePath.OrderBy(_ => _).ToArray();
            this.RawName = name;
            this.Name = name + "- " + this.FilePath.Length + " parts";
        }

        public string[] FilePath {get;}

        public string Name { get;}

        public string RawName { get;}

    }
}