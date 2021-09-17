using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.MultipleFiles
{
    internal class AssociationInfo
    {
        internal string Node
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.Location);
            }
        }
        internal readonly string Location;
        internal Association Data;

        public AssociationInfo(string association)
        {
            string fileDir = Vars.Dir_Associations;
            if (!Directory.Exists(fileDir)) Directory.CreateDirectory(fileDir);

            Location = Vars.Dir_Associations + $@"\{association}.json";
            if (File.Exists(Location)) Load();
        }

        internal void Load()
        {
            string jsonData = File.ReadAllText(Location);
            Data = JsonConvert.DeserializeObject<Association>(jsonData);
        }

        internal void Save()
        {
            string jsonData = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(Location, jsonData);
        }
    }
}
