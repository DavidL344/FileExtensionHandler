using FileExtensionHandler.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Controller
{
    public class FileExtensionsController
    {
        /// <summary>
        /// Saves the file extension information to the disk.
        /// </summary>
        /// <param name="fileExtensionClass">The file extension class to serialize.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        public static void SaveToJson(FileExtension fileExtensionClass, string fileExtensionsDir)
        {
            string filePath = $@"{fileExtensionsDir}\{fileExtensionClass.Node}.json";
            string jsonData = Serialize(fileExtensionClass);
            File.WriteAllText(filePath, jsonData);
        }

        /// <summary>
        /// Loads the file extension information.
        /// </summary>
        /// <param name="fileExtension">The file extension name of the "<![CDATA[<file extension>]]>.json" file.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <returns>Desearialized file extension information.</returns>
        public static FileExtension LoadFromJson(string fileExtension, string fileExtensionsDir)
        {
            string filePath = $@"{fileExtensionsDir}\{fileExtension}.json";
            if (!File.Exists(filePath))
                return fileExtension == "" ? new FileExtension
                {
                    Node = null,
                    Name = "",
                    Icon = null,
                    IconIndex = 0,
                    Associations = new string[] { },
                    DefaultAssociation = null
                } : null;
            string jsonData = File.ReadAllText(filePath);
            return Deserialize(jsonData, fileExtension);
        }

        /// <summary>
        /// Loads a list containing file extension information.
        /// </summary>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <returns>A list of desearialized file extension information entries.</returns>
        public static List<FileExtension> GetFileExtensions(string fileExtensionsDir)
        {
            List<FileExtension> loadedFileExtensionsList = new List<FileExtension>();
            DirectoryInfo directoryInfo = new DirectoryInfo(fileExtensionsDir);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string fileExtension = Path.GetFileNameWithoutExtension(file.Name);
                FileExtension fileExtensionData = LoadFromJson(fileExtension, fileExtensionsDir);
                loadedFileExtensionsList.Add(fileExtensionData);
            }
            return loadedFileExtensionsList;
        }

        /// <summary>
        /// Serializes the file extension information into a readable JSON format.
        /// </summary>
        /// <param name="fileExtensionClass">The file extension class to serialize.</param>
        /// <returns>A JSON serialized class.</returns>
        public static string Serialize(FileExtension fileExtensionClass)
        {
            return JsonConvert.SerializeObject(fileExtensionClass, Formatting.Indented);
        }

        /// <summary>
        /// Deserializes the JSON format into the file extension class.
        /// </summary>
        /// <param name="fileExtensionJson">A JSON serialized class.</param>
        /// <param name="node">The file extension name of the "<![CDATA[<file extension>]]>.json" file.</param>
        /// <returns>A deserialized file extension class.</returns>
        public static FileExtension Deserialize(string fileExtensionJson, string node = null)
        {
            FileExtension data = JsonConvert.DeserializeObject<FileExtension>(fileExtensionJson);
            data.Node = node;
            return data;
        }
    }
}
