using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.MultipleFiles
{
    internal class FileExtensionInfo
    {
        internal readonly string Location;
        internal FileExtension Data;

        public FileExtensionInfo(string fileExtension)
        {
            string fileDir = Vars.Dir_FileExtensions;
            if (!Directory.Exists(fileDir)) Directory.CreateDirectory(fileDir);

            Location = Vars.Dir_FileExtensions + $@"\{fileExtension}.json";
            if (File.Exists(Location)) Load();
        }

        internal void Load()
        {
            string jsonData = File.ReadAllText(Location);
            Data = JsonConvert.DeserializeObject<FileExtension>(jsonData);
        }

        internal void Save()
        {
            string jsonData = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Location, jsonData);
        }
    }
}
