using FileExtensionHandler.Core.Model.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Model.Common
{
    public class OpenedFile
    {
        internal readonly string AssociationsDir;
        internal readonly string FileExtensionsDir;

        public readonly string Location;
        private readonly bool IsFromProtocol;
        public string Name => Path.GetFileName(Location);
        public string Extension
        {
            get
            {
                if (!IsFromProtocol) return Path.GetExtension(Location);
                string urlNoArgs = Location.Split('?')[0];
                string urlLastParam = urlNoArgs.Split(new char[] { '/', '\\' }).Last();
                return urlLastParam.Contains('.') ? urlLastParam.Substring(urlLastParam.LastIndexOf('.')) : throw new Exception("No file extension!");
            }
        }
        public string Type => !string.IsNullOrWhiteSpace(this.FileExtensionInfo.Name) ? this.FileExtensionInfo.Name : null;
        public bool Exists => File.Exists(Location);
        public string DefaultAssociation { get; private set; }
        public int DefaultAssociationIndex
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DefaultAssociation))
                    for (int i = 0; i < AssociationsList.Count; i++)
                        if (AssociationsList[i].Node == DefaultAssociation) return i;
                return -1;
            }
        }

        public List<Association> AssociationsList { get; private set; }
        public FileExtension FileExtensionInfo { get; private set; }

        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        internal OpenedFile(string filePath, string associationsDir, string fileExtensionsDir, bool isFromProtocol)
        {
            if (!Directory.Exists(associationsDir)) throw new DirectoryNotFoundException("The directory with associations doesn't exist!");
            if (!Directory.Exists(fileExtensionsDir)) throw new DirectoryNotFoundException("The directory with file extension information doesn't exist!");

            this.Location = filePath;
            this.IsFromProtocol = isFromProtocol;

            this.AssociationsDir = associationsDir;
            this.FileExtensionsDir = fileExtensionsDir;

            if (!isFromProtocol)
            {
                this.Location = Path.GetFullPath(filePath);
                if (!this.Exists) throw new FileNotFoundException("The file you're trying to open doesn't exist!");
            }
        }

        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        internal void LoadInfo()
        {
            FileExtensionInfo = LoadFileExtensionInfo();
            if (FileExtensionInfo.Associations.Length == 0) throw new IndexOutOfRangeException($"The there's no app associated with {this.Extension}!");
            this.DefaultAssociation = FileExtensionInfo.DefaultAssociation;

            AssociationsList = LoadAssociationsList();
        }

        private FileExtension LoadFileExtensionInfo()
        {
            string fileExtensionDefinitionPath = this.FileExtensionsDir + $@"\{this.Extension}.json";

            if (!File.Exists(fileExtensionDefinitionPath))
                throw new FileNotFoundException($"The there's no app associated with {this.Extension}!");
            string jsonData = File.ReadAllText(fileExtensionDefinitionPath);
            return JsonConvert.DeserializeObject<FileExtension>(jsonData);
        }

        private List<Association> LoadAssociationsList()
        {
            List<Association> list = new List<Association>();
            list.Clear();
            foreach (string associationName in FileExtensionInfo.Associations)
            {
                string fileAssociationPath = this.AssociationsDir + $@"\{associationName}.json";

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
