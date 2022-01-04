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
    public class AssociationsController
    {
        /// <summary>
        /// Saves the association information to the disk.
        /// </summary>
        /// <param name="associationClass">The association class to serialize.</param>
        /// <param name="filePath">The path where the file will be saved.</param>
        public static void SaveToJson(Association associationClass, string associationsDir)
        {
            string filePath = $@"{associationsDir}\{associationClass.Node}.json";
            string jsonData = Serialize(associationClass);
            File.WriteAllText(filePath, jsonData);
        }

        /// <summary>
        /// Loads the association information.
        /// </summary>
        /// <param name="association">The association name of the "<![CDATA[<association name>]]>.json" file.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>Desearialized association information.</returns>
        public static Association LoadFromJson(string association, string associationsDir)
        {
            string filePath = $@"{associationsDir}\{association}.json";
            string jsonData = File.ReadAllText(filePath);
            return Deserialize(jsonData, association);
        }

        /// <summary>
        /// Loads a list containing association information.
        /// </summary>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>A list of desearialized association information entries.</returns>
        public static List<Association> GetAssociations(string associationsDir)
        {
            List<Association> loadedAssociationsList = new List<Association>();
            DirectoryInfo directoryInfo = new DirectoryInfo(associationsDir);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string association = Path.GetFileNameWithoutExtension(file.Name);
                Association associationData = LoadFromJson(association, associationsDir);
                loadedAssociationsList.Add(associationData);
            }
            return loadedAssociationsList;
        }

        /// <summary>
        /// Creates a list of associations available to a file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension class with associations.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>A list of association classes.</returns>
        public static List<Association> MakeList(FileExtension fileExtension, string associationsDir)
        {
            List<Association> list = new List<Association>();
            if (fileExtension == null) return list;
            if (fileExtension.Associations.Length == 0)
            {
                if (fileExtension.Node != null) return list;

                // The file has no file extension and there's no JSON file to handle the case
                List<string> allAssociations = new List<string>();
                DirectoryInfo directoryInfo = new DirectoryInfo(associationsDir);
                foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
                {
                    allAssociations.Add(Path.GetFileNameWithoutExtension(file.FullName));
                }  
                fileExtension.Associations = allAssociations.ToArray();
            }
            
            foreach (string associationName in fileExtension.Associations)
            {
                string fileAssociationPath = associationsDir + $@"\{associationName}.json";

                // Skip the current iteration if the association file doesn't exist
                if (!File.Exists(fileAssociationPath)) continue;
                string jsonData = File.ReadAllText(fileAssociationPath);
                Association association = JsonConvert.DeserializeObject<Association>(jsonData);

                association.Node = associationName;
                list.Add(association);
            }
            return list;
        }

        /// <summary>
        /// Serializes the association into a readable JSON format.
        /// </summary>
        /// <param name="associationClass">The association class to serialize.</param>
        /// <returns>A JSON serialized class.</returns>
        public static string Serialize(Association associationClass)
        {
            return JsonConvert.SerializeObject(associationClass, Formatting.Indented);
        }

        /// <summary>
        /// Deserializes the JSON format into the association class.
        /// </summary>
        /// <param name="associationJson">A JSON serialized class.</param>
        /// <param name="node">The association name of the "<![CDATA[<association name>]]>.json" file.</param>
        /// <returns>A deserialized association class.</returns>
        public static Association Deserialize(string associationJson, string node = null)
        {
            Association data = JsonConvert.DeserializeObject<Association>(associationJson);
            data.Node = node;
            return data;
        }
    }
}
