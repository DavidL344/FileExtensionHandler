﻿using Newtonsoft.Json;

namespace FileExtensionHandler.Core.Model
{
    public struct Association
    {
        /// <summary>
        /// The association's file name.
        /// </summary>
        /// <remarks>Used for targetting the association by its file name.</remarks>
        [JsonIgnore]
        public string? Node { get; internal set; }

        /// <summary>
        /// The name of the association.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// The icon used for the association in the File Explorer.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; } = String.Empty;

        /// <summary>
        /// The index of the specified icon for the File Explorer.
        /// </summary>
        [JsonProperty("iconIndex")]
        public int IconIndex { get; set; }

        /// <summary>
        /// The associated app's path.
        /// </summary>
        /// <remarks>Environment variables are expanded during runtime.</remarks>
        [JsonProperty("command")]
        public string Command { get; set; } = String.Empty;

        /// <summary>
        /// The arguments passed to the associated app. 
        /// </summary>
        /// <remarks>Environment variables are expanded during runtime.</remarks>
        [JsonProperty("arguments")]
        public string Arguments { get; set; } = "%1";
    }
}
