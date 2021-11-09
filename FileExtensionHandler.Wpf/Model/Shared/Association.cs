using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.Shared
{
    internal class Association
    {
        /// <summary>
        /// The associated app's name to be shown in the handler.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// The associated app's icon file (or an embedded icon) to be shown in the handler.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; internal set; }

        /// <summary>
        /// The index of the icon file (or an embedded icon) to be shown in the handler.
        /// </summary>
        [JsonProperty("iconIndex")]
        public int IconIndex { get; internal set; }

        /// <summary>
        /// The associated app's path.
        /// </summary>
        [JsonProperty("command")]
        public string Command { get; internal set; }

        /// <summary>
        /// The arguments to be passed to the associated app. 
        /// </summary>
        [JsonProperty("arguments")]
        public string Arguments { get; internal set; }
    }
}
