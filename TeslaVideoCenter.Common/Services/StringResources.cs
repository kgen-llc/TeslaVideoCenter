using System.Reflection;
using System.Resources;

namespace TeslaVideoCenter.Services
{
    public class StringResources
    {
        private readonly ResourceManager rsm;

        public StringResources()
        {
            this.rsm = new ResourceManager("StringResources", Assembly.GetExecutingAssembly());
        }

        public string GetResource(string id)
        {
            var resource = this.rsm.GetString(id);
            return string.IsNullOrEmpty(resource) ? id : resource;
        }
    }
}