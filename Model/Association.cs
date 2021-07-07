using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model
{
    internal class Association
    {
        [JsonProperty("name")]
        public string Name { get; internal set; }

        [JsonProperty("command")]
        public string Path { get; internal set; }

        [JsonProperty("arguments")]
        public string Arguments { get; internal set; }

        public Association(string name, string command, string arguments)
        {
            this.Name = name;
            this.Path = command;
            this.Arguments = arguments;
        }
    }
}
