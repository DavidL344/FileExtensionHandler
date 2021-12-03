﻿using FileExtensionHandler.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core
{
    public class FileInformation
    {
        private readonly Arguments Arguments;
        private readonly string Dir_Associations;
        private readonly string Dir_FileExtensions;

        public readonly List<Association> Associations;
        public readonly FileExtension FileExtension;
        public readonly Association DefaultAssociation;
        public readonly int DefaultAssociationIndex;

        public string Name => Arguments.Name;
        public string Location => Arguments.Parsed;
        public bool Streamed => Arguments.Streamed;
        public string Type => !string.IsNullOrWhiteSpace(FileExtension.Name) ? FileExtension.Name : null;

        public string[] ProtocolsUsed => Arguments.ProtocolsUsed;
        public bool CalledFromAppProtocol => Arguments.CalledFromAppProtocol;

        /// <summary>
        /// Loads the file information from its path.
        /// </summary>
        /// <param name="filePath">The path containing the file.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <inheritdoc cref="CheckEnv()"/>
        public FileInformation(string filePath, string associationsDir, string fileExtensionsDir)
        {
            Dir_Associations = associationsDir;
            Dir_FileExtensions = fileExtensionsDir;
            CheckEnv();

            Arguments = new Arguments(new string[] { filePath });
            FileExtension = GetFileExtensionInfo();
            Associations = GetAssociations();

            DefaultAssociationIndex = Associations.FindIndex(x => x.Node.Equals(FileExtension.DefaultAssociation));
            DefaultAssociation = DefaultAssociationIndex != -1 ? GetData(DefaultAssociationIndex) : null;
        }

        /// <summary>
        /// Loads the file information from the arguments passed.
        /// </summary>
        /// <param name="args">The command line arguments passed to the app.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <inheritdoc cref="CheckEnv()"/>
        public FileInformation(string[] args, string associationsDir, string fileExtensionsDir)
        {
            Dir_Associations = associationsDir;
            Dir_FileExtensions = fileExtensionsDir;
            CheckEnv();

            Arguments = new Arguments(args);
            FileExtension = GetFileExtensionInfo();
            Associations = GetAssociations();

            DefaultAssociationIndex = Associations.FindIndex(x => x.Node.Equals(FileExtension.DefaultAssociation));
            DefaultAssociation = DefaultAssociationIndex != -1 ? GetData(DefaultAssociationIndex) : null;
        }

        /// <summary>
        /// Loads the file information from pre-parsed arguments.
        /// </summary>
        /// <param name="arguments">Parsed command line arguments passed to the app.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <inheritdoc cref="CheckEnv()"/>
        public FileInformation(Arguments arguments, string associationsDir, string fileExtensionsDir)
        {
            Dir_Associations = associationsDir;
            Dir_FileExtensions = fileExtensionsDir;
            CheckEnv();

            Arguments = arguments;
            FileExtension = GetFileExtensionInfo();
            Associations = GetAssociations();

            DefaultAssociationIndex = Associations.FindIndex(x => x.Node.Equals(FileExtension.DefaultAssociation));
            DefaultAssociation = DefaultAssociationIndex != -1 ? GetData(DefaultAssociationIndex) : null;
        }

        /// <exception cref="DirectoryNotFoundException"/>
        private void CheckEnv()
        {
            if (!Directory.Exists(Dir_Associations)) throw new DirectoryNotFoundException("The directory with associations doesn't exist!");
            if (!Directory.Exists(Dir_FileExtensions)) throw new DirectoryNotFoundException("The directory with file extension information doesn't exist!");
        }

        private FileExtension GetFileExtensionInfo()
        {
            string fileExtension = Path.GetExtension(Arguments.ParsedNoParameters);
            string fileExtensionJson = Dir_FileExtensions + $@"\{fileExtension}.json";
            if (!File.Exists(fileExtensionJson)) return null;

            string jsonData = File.ReadAllText(fileExtensionJson);
            FileExtension data = JsonConvert.DeserializeObject<FileExtension>(jsonData);
            data.Node = fileExtension;
            return data;
        }

        private List<Association> GetAssociations()
        {
            List<Association> list = new List<Association>();
            if (FileExtension == null || FileExtension.Associations.Length == 0) return list;

            foreach (string associationName in FileExtension.Associations)
            {
                string fileAssociationPath = Dir_Associations + $@"\{associationName}.json";

                // Skip the current iteration if the association file doesn't exist
                if (!File.Exists(fileAssociationPath)) continue;
                string jsonData = File.ReadAllText(fileAssociationPath);
                Association association = JsonConvert.DeserializeObject<Association>(jsonData);

                association.Node = associationName;
                list.Add(association);
            }
            return list;
        }

        private Association GetData(int selectedIndex)
        {
            return Associations[selectedIndex];
        }

        /// <summary>
        /// Gets the associated app's information for opening the file.
        /// </summary>
        /// <param name="id">ID of the association from the file extension's list.</param>
        /// <returns>ProcessStartInfo for the selected association.</returns>
        private ProcessStartInfo GetProcessData(int id)
        {
            if (id >= Associations.Count || id < 0) throw new ArgumentOutOfRangeException();
            Association association = GetData(id);

            string command = Environment.ExpandEnvironmentVariables(association.Command);
            return new ProcessStartInfo()
            {
                FileName = command,
                Arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{Location}\""),
                WorkingDirectory = Path.GetDirectoryName(command)
            };
        }


        /// <summary>
        /// Opens the file with a selected application
        /// </summary>
        /// <param name="id">ID of the association from the file extension's list.</param>
        public void OpenWith(int id)
        {
            if (id == -1)
            {
                if (DefaultAssociationIndex == -1) return;
                id = DefaultAssociationIndex;
            }
            ProcessStartInfo processStartInfo = GetProcessData(id);
            Process.Start(processStartInfo);
        }
    }
}
