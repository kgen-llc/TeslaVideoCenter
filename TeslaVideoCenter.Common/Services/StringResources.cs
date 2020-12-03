using System.Reflection;
using System.Resources;

namespace TeslaVideoCenter.Common.Services
{
    public class StringResources
    {
        private readonly ResourceManager rsm;

        public const bool ChineseLetterToLetterIsOn = false;

        public StringResources()
        {
            this.rsm = new ResourceManager("TeslaVideoCenter.Common.Services.StringResources", Assembly.GetExecutingAssembly());
        }

        public string GetResource(string id)
        {
            var resource = this.rsm.GetString(id);
            if(string.IsNullOrEmpty(resource)) 
            {
                resource = id;
            }
            else if(ChineseLetterToLetterIsOn) {
                resource = ChineseLetterToLetter.Convert(resource);
            }

            return resource;
        }

        public string this[string id] {
             get { return GetResource(id);} 
        }
    }
}