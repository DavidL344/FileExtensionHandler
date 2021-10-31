﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.Shared
{
    internal class FileExtension
    {
        /*
        [JsonIgnore] internal string Location => Vars.Dir_FileExtensions + $@"\{this.Extension}.json";
        [JsonIgnore] internal string Extension; // Will be defined on file load
        */

        /// <summary>
        /// The file type description.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// The file type icon.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; internal set; }

        /// <summary>
        /// The index of the icon file (or an embedded icon) to be shown in the handler.
        /// </summary>
        [JsonProperty("iconIndex")]
        public int IconIndex { get; internal set; }

        /// <summary>
        /// A list of associated apps. To modify, convert to List<string> first.
        /// </summary>
        [JsonProperty("associations")]
        public string[] Associations { get; internal set; }

        /// <summary>
        /// A default association to use if forced selection is disabled (forced selection is not yet implemented).
        /// </summary>
        [JsonProperty("defaultAssociation")]
        public string DefaultAssociation { get; internal set; }
    }
}
