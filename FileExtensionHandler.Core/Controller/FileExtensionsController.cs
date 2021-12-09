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
    class FileExtensionsController
    {
        internal static void SaveToJson(FileExtension fileExtensionData, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(fileExtensionData, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }

        internal static FileExtension LoadFromJson(string fileExtension, string fileExtensionsDir)
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
            FileExtension data = JsonConvert.DeserializeObject<FileExtension>(jsonData);
            data.Node = fileExtension;
            return data;
        }
    }
}
