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
    class AssociationsController
    {
        internal static void SaveToJson(Association associationData, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(associationData, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        internal static Association LoadFromJson(string association, string associationsDir)
        {
            string filePath = $@"{associationsDir}\{association}.json";
            string jsonData = File.ReadAllText(filePath);
            Association data = JsonConvert.DeserializeObject<Association>(jsonData);
            data.Node = association;
            return data;
        }

        internal static List<Association> MakeList(FileExtension fileExtension, string associationsDir)
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
    }
}
