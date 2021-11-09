using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.Shared
{
    internal class OpenedFile
    {
        public string Path { get; protected set; }
        internal string Name => System.IO.Path.GetFileName(Path);
        internal string Extension => System.IO.Path.GetExtension(Path);
        internal string Type => !string.IsNullOrWhiteSpace(this.FileExtensionInfo.Name) ? this.FileExtensionInfo.Name : null;

        internal readonly List<Association> AssociationsList;
        internal readonly FileExtension FileExtensionInfo;

        internal OpenedFile(string filePath)
        {
            this.Path = filePath;
            FileExtensionInfo = LoadFileExtensionInfo();
            AssociationsList = LoadAssociationsList();
        }

        protected virtual FileExtension LoadFileExtensionInfo()
        {
            string jsonData = File.ReadAllText(Vars.Dir_FileExtensions + $@"\{this.Extension}.json");
            return JsonConvert.DeserializeObject<FileExtension>(jsonData);
        }

        protected virtual List<Association> LoadAssociationsList()
        {
            List<Association> list = new List<Association>();
            list.Clear();
            foreach (string associationName in FileExtensionInfo.Associations)
            {
                string jsonData = File.ReadAllText(Vars.Dir_Associations + $@"\{associationName}.json");
                Association association = JsonConvert.DeserializeObject<Association>(jsonData);
                list.Add(association);
            }
            return list;
        }

        internal Association GetData(int selectedIndex) => AssociationsList[selectedIndex];

        internal void OpenWith(int id)
        {
            if (id == -1) return;
            Association association = this.GetData(id);
            string fileName = Environment.ExpandEnvironmentVariables(association.Command);
            string arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{this.Path}\"");

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = System.IO.Path.GetDirectoryName(fileName)
            };
            Process.Start(processStartInfo);
            //Application.Current.Shutdown();
        }
    }
}
