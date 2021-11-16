using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Model.Shared
{
    public class Association
    {
        /// <summary>
        /// The association file name.
        /// </summary>
        /// <remarks>Used for targetting the association by its file name.</remarks>
        [JsonIgnore]
        public string Node { get; internal set; }

        /// <summary>
        /// The name of the association.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// The icon used for the association in the File Explorer.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; internal set; }

        /// <summary>
        /// The index of the specified icon for the File Explorer.
        /// </summary>
        [JsonProperty("iconIndex")]
        public int IconIndex { get; internal set; }

        /// <summary>
        /// The associated app's path.
        /// </summary>
        /// <remarks>Environment variables are expanded during runtime.</remarks>
        [JsonProperty("command")]
        public string Command { get; internal set; }

        /// <summary>
        /// The arguments passed to the associated app. 
        /// </summary>
        /// <remarks>Environment variables are expanded during runtime.</remarks>
        [JsonProperty("arguments")]
        public string Arguments { get; internal set; }
    }
}
