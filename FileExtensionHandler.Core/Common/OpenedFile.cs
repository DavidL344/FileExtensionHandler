﻿using FileExtensionHandler.Core.Model;
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

        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        internal OpenedFile(string filePath, string associationsDir, string fileExtensionsDir, bool isFromProtocol)
        {
            if (!Directory.Exists(associationsDir)) throw new DirectoryNotFoundException("The directory with associations doesn't exist!");
            if (!Directory.Exists(fileExtensionsDir)) throw new DirectoryNotFoundException("The directory with file extension information doesn't exist!");

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

        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IndexOutOfRangeException"/>
        internal void LoadInfo()
        {
            FileExtensionInfo = LoadFileExtensionInfo();
            if (FileExtensionInfo.Associations.Length == 0) throw new IndexOutOfRangeException($"The there's no app associated with {Extension}!");
            DefaultAssociation = FileExtensionInfo.DefaultAssociation;

            AssociationsList = LoadAssociationsList();
        }

        private FileExtension LoadFileExtensionInfo()
        {
            string fileExtensionDefinitionPath = FileExtensionsDir + $@"\{Extension}.json";
            if (File.Exists(fileExtensionDefinitionPath))
            {
                string jsonData = File.ReadAllText(fileExtensionDefinitionPath);
                return JsonConvert.DeserializeObject<FileExtension>(jsonData);
            }
            //throw new FileNotFoundException($"The there's no app associated with {Extension}!");

            try
            {
                // Registry fallback
                OpenedKey openedFileExtensionKey = new OpenedKey($@"HKCR\{Extension}");
                string regFileExtension = openedFileExtensionKey.GetDefaultValue();

                OpenedKey openedFileAssociationKey = new OpenedKey($@"HKCR\{regFileExtension}");
                object[] icon = new OpenedKey($@"HKCR\{regFileExtension}\DefaultIcon").GetDefaultValue().Split(',');

                return new FileExtension()
                {
                    Name = openedFileAssociationKey.GetDefaultValue(),
                    Icon = (string)icon[0],
                    IconIndex = (int)icon[1],
                    Associations = new string[] { openedFileAssociationKey.GetDefaultValue() },
                    DefaultAssociation = openedFileAssociationKey.GetDefaultValue()
                };
            }
            catch
            {
                return new FileExtension()
                {
                    Associations = new string[] { }
                };
            };
        }

        private List<Association> LoadAssociationsList()
        {
            List<Association> list = new List<Association>();
            list.Clear();
            foreach (string associationName in FileExtensionInfo.Associations)
            {
                string fileAssociationPath = AssociationsDir + $@"\{associationName}.json";

                // Skip the current iteration if the association file doesn't exist
                if (!File.Exists(fileAssociationPath))
                {
                    OpenedKey openedKey = new OpenedKey($@"HKCR\{associationName}");
                    OpenedKey commandKey = new OpenedKey($@"HKCR\{associationName}\shell\open\command");

                    string name = openedKey.GetDefaultValue();
                    string command = commandKey.GetDefaultValue();
                    object[] icon = new OpenedKey($@"HKCR\{associationName}\DefaultIcon").GetDefaultValue().Split(',');

                    Association regAssociation = new Association()
                    {
                        Node = associationName,
                        Name = name,
                        Icon = (string)icon[0],
                        IconIndex = (int)icon[1],
                        Command = command,
                        Arguments = null
                    };
                    list.Add(regAssociation);
                    continue;
                }
                string jsonData = File.ReadAllText(fileAssociationPath);
                Association association = JsonConvert.DeserializeObject<Association>(jsonData);
                association.Node = associationName;
                list.Add(association);
            }

            if (list.Count == 0) throw new FileNotFoundException($"The there's no app associated with {Extension}!");
            return list;
        }
    }
}
