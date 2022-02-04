using Newtonsoft.Json;

namespace FileExtensionHandler.Core.Model
{
    public struct Importable
    {
        /// <summary>
        /// An importable/exportable associations list
        /// </summary>
        [JsonProperty("associations")]
        public Dictionary<string, Association> Associations { get; internal set; } = new();

        /// <summary>
        /// An importable/exportable file extensions list
        /// </summary>
        [JsonProperty("fileExtensions")]
        public Dictionary<string, FileExtension> FileExtensions { get; internal set; } = new();
    }
}
