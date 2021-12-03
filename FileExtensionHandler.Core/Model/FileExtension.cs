using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Model
{
    public class FileExtension
    {
        /// <summary>
        /// The file extension's file name.
        /// </summary>
        /// <remarks>Used for targetting the association by its file name.</remarks>
        [JsonIgnore]
        public string Node { get; internal set; }

        /// <summary>
        /// A short file type description.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        /// The icon used for the file extension in the handler.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; internal set; }

        /// <summary>
        /// The index of the specified icon for the handler.
        /// </summary>
        [JsonProperty("iconIndex")]
        public int IconIndex { get; internal set; }

        /// <summary>
        /// An array containing associations for the file extension.
        /// </summary>
        /// <remarks>The elements can be modified by converting them to <![CDATA[ List<string> ]]> first.</remarks>
        [JsonProperty("associations")]
        public string[] Associations { get; internal set; }

        /// <summary>
        /// A default association for the file extension.
        /// </summary>
        /// <remarks>Can be overriden by forced selection.</remarks>
        [JsonProperty("defaultAssociation")]
        public string DefaultAssociation { get; internal set; }
    }
}
