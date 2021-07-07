using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model
{
    internal class FileExtension
    {
        [JsonProperty("associations")]
        public List<Association> Associations { get; internal set; }

        public FileExtension(List<Association> associations)
        {
            this.Associations = associations;
        }
    }
}
