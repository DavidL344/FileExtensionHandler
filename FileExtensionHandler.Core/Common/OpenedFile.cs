using FileExtensionHandler.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Common
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
        public string Type => !string.IsNullOrWhiteSpace(FileExtensionInfo.Name) ? FileExtensionInfo.Name : null;
        public bool Exists => File.Exists(Location);
        public string DefaultAssociation { get; private set; }
        public int DefaultAssociationIndex
        {
            get
            {
                if (!string.IsNullOrEmpty(DefaultAssociation))
                    for (int i = 0; i < AssociationsList.Count; i++)
                        if (AssociationsList[i].Node == DefaultAssociation) return i;
                return -1;
            }
        }

        public List<Association> AssociationsList { get; private set; }
        public FileExtension FileExtensionInfo { get; private set; }

        /// <param name="filePath">The path containing the file.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <param name="isFromProtocol">Defines if the path comes from a protocol.</param>
        /// <exception cref="DataDirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        internal OpenedFile(string filePath, string associationsDir, string fileExtensionsDir, bool isFromProtocol)
        {
            if (!Directory.Exists(associationsDir)) throw new DataDirectoryNotFoundException("The directory with associations doesn't exist!");
            if (!Directory.Exists(fileExtensionsDir)) throw new DataDirectoryNotFoundException("The directory with file extension information doesn't exist!");

            Location = filePath;
            IsFromProtocol = isFromProtocol;

            AssociationsDir = associationsDir;
            FileExtensionsDir = fileExtensionsDir;

            if (!isFromProtocol)
            {
                Location = Path.GetFullPath(filePath);
                if (!Exists) throw new FileNotFoundException("The file you're trying to open doesn't exist!");
            }
        }

        /// <inheritdoc cref="LoadFileExtensionInfo()"/>
        /// <inheritdoc cref="LoadAssociationsList()"/>
        /// <exception cref="AssociationsNotFoundException"/>
        internal void LoadInfo()
        {
            FileExtensionInfo = LoadFileExtensionInfo();
            if (FileExtensionInfo.Associations.Length == 0) throw new AssociationsNotFoundException($"The there's no app associated with {Extension}!");
            DefaultAssociation = FileExtensionInfo.DefaultAssociation;
            AssociationsList = LoadAssociationsList();
        }

        /// <exception cref="AssociationsNotFoundException"/>
        private FileExtension LoadFileExtensionInfo()
        {
            string fileExtensionDefinitionPath = FileExtensionsDir + $@"\{Extension}.json";
            if (!File.Exists(fileExtensionDefinitionPath))
                throw new AssociationsNotFoundException($"The there's no app associated with {Extension}!");
            
            string jsonData = File.ReadAllText(fileExtensionDefinitionPath);
            return JsonConvert.DeserializeObject<FileExtension>(jsonData);
        }

        private List<Association> LoadAssociationsList()
        {
            List<Association> list = new List<Association>();
            list.Clear();
            foreach (string associationName in FileExtensionInfo.Associations)
            {
                string fileAssociationPath = AssociationsDir + $@"\{associationName}.json";

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
