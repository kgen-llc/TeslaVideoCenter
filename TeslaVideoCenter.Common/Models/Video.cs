using System.Collections.Generic;
using System.Linq;

namespace TeslaVideoCenter.Models
{

    public class Video {

        public Video(string name, IEnumerable<string> filePath) {
            this.FilePath = filePath.OrderBy(_ => _).ToArray();
            this.RawName = name;
            this.Name = name + "- " + this.FilePath.Length + " parts";
            
            this.IsAlreadyProcess = false;
        }

        public Video(string name, string filePath) {
            this.Name = this.RawName = name;
            this.FilePath = new [] { filePath};

            this.IsAlreadyProcess = true;
        }

        public string[] FilePath {get;}

        public string Name { get;}

        public string RawName { get;}

        public bool IsAlreadyProcess {get;}

    }
}