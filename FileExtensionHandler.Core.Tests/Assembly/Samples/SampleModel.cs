using Newtonsoft.Json;

namespace FileExtensionHandler.Core.Tests.Assembly.Samples
{
    internal struct SampleModel
    {
        [JsonProperty("name")]
        internal string Name { get; set; }

        [JsonProperty("description")]
        internal string Description { get; set; }
    }
}
