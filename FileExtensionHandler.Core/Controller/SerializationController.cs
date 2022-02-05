using Newtonsoft.Json;

namespace FileExtensionHandler.Core.Controller
{
    public class SerializationController
    {
        /// <summary>
        /// Serializes a struct object into a readable JSON format.
        /// </summary>
        /// <param name="serializableType">The struct object to serialize.</param>
        /// <returns>A JSON serialized struct object.</returns>
        public static string Serialize<T>(T serializableType) where T : struct
        {
            return JsonConvert.SerializeObject(serializableType, Formatting.Indented);
        }

        /// <summary>
        /// Serializes a struct object into a readable JSON format asynchronously.
        /// </summary>
        /// <param name="serializableType">The struct object to serialize.</param>
        /// <returns>A JSON serialized struct object.</returns>
        public static async Task<string> SerializeAsync<T>(T serializableType, CancellationToken cancellationToken = default) where T : struct
        {
            return await Task.Run(() => Serialize(serializableType), cancellationToken);
        }

        /// <summary>
        /// Serializes a struct object into a readable JSON file.
        /// </summary>
        /// <param name="serializableType">The struct object to serialize.</param>
        /// <param name="filePath">A file path to serialize the struct object to.</param>
        public static void SerializeToFile<T>(T serializableType, string filePath) where T : struct
        {
            string jsonData = Serialize(serializableType);
            File.WriteAllText(filePath, jsonData);
        }

        /// <summary>
        /// Serializes a struct object into a readable JSON file asynchronously.
        /// </summary>
        /// <param name="serializableType">The struct object to serialize.</param>
        /// <param name="filePath">A file path to serialize the struct object to.</param>
        public static async Task SerializeToFileAsync<T>(T serializableType, string filePath, CancellationToken cancellationToken = default) where T : struct
        {
            string jsonData = await Task.Run(async () => await SerializeAsync(serializableType), cancellationToken);
            await File.WriteAllTextAsync(filePath, jsonData, cancellationToken);
        }

        /// <summary>
        /// Deserializes the JSON format into a struct object.
        /// </summary>
        /// <param name="jsonData">A JSON serialized struct object.</param>
        /// <returns>A deserialized struct object.</returns>
        public static T Deserialize<T>(string jsonData) where T : struct
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        /// <summary>
        /// Deserializes the JSON format into a struct object asynchronously.
        /// </summary>
        /// <param name="jsonData">A JSON serialized struct object.</param>
        /// <returns>A deserialized struct object.</returns>
        public static async Task<T> DeserializeAsync<T>(string jsonData, CancellationToken cancellationToken = default) where T : struct
        {
            return await Task.Run(() => Deserialize<T>(jsonData), cancellationToken);
        }

        /// <summary>
        /// Deserializes the JSON file into a struct object.
        /// </summary>
        /// <param name="filePath">A file path to a JSON serialized struct object.</param>
        /// <returns>A deserialized struct object.</returns>
        public static T DeserializeFile<T>(string filePath) where T : struct
        {
            string jsonData = File.ReadAllText(filePath);
            return Deserialize<T>(jsonData);
        }

        /// <summary>
        /// Deserializes the JSON file into a struct object asynchronously.
        /// </summary>
        /// <param name="filePath">A file path to a JSON serialized struct object.</param>
        /// <returns>A deserialized struct.</returns>
        public static async Task<T> DeserializeFileAsync<T>(string filePath, CancellationToken cancellationToken = default) where T : struct
        {
            string jsonData = await File.ReadAllTextAsync(filePath, cancellationToken);
            return await Task.Run(async () => await DeserializeAsync<T>(jsonData), cancellationToken);
        }

        /// <summary>
        /// Clones an existing struct object.
        /// </summary>
        /// <param name="serializableType">An existing struct object to clone.</param>
        /// <returns>A new struct object with the property values copied from an existing struct object.</returns>
        public static T Clone<T>(T serializableType) where T : struct
        {
            return Deserialize<T>(Serialize(serializableType));
        }

        /// <summary>
        /// Clones an existing struct object asynchronously.
        /// </summary>
        /// <param name="serializableType">An existing struct object to clone.</param>
        /// <returns>A new struct object with the property values copied from an existing struct object.</returns>
        public static async Task<T> CloneAsync<T>(T serializableType, CancellationToken cancellationToken = default) where T : struct
        {
            string serialized = await SerializeAsync(serializableType, cancellationToken);
            return await DeserializeAsync<T>(serialized, cancellationToken);
        }
    }
}
