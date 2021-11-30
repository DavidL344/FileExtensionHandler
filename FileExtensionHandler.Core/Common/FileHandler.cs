using FileExtensionHandler.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Common
{
    class FileHandler
    {
        public OpenedFile OpenedFile;

        /// <summary>
        /// Loads the file information.
        /// </summary>
        /// <inheritdoc cref="OpenedFile.OpenedFile(string, string, string, bool)"/>
        /// <inheritdoc cref="OpenedFile.LoadInfo()"/>
        internal void OpenFile(string filePath, string associationsDir, string fileExtensionsDir, bool isFromProtocol)
        {
            OpenedFile = new OpenedFile(filePath, associationsDir, fileExtensionsDir, isFromProtocol);
            OpenedFile.LoadInfo();
        }

        /// <summary>
        /// Loads the file information from the arguments passed.
        /// </summary>
        /// <param name="args">Command line arguments passed to the app.</param>
        /// <inheritdoc cref="OpenedFile.OpenedFile(string, string, string, bool)"/>
        /// <inheritdoc cref="OpenedFile.LoadInfo()"/>
        internal void OpenFile(string[] args, string associationsDir, string fileExtensionsDir)
        {
            Arguments arguments = new Arguments(args);
            OpenedFile = new OpenedFile(arguments.FilePath, associationsDir, fileExtensionsDir, arguments.IsFromProtocol);
            OpenedFile.LoadInfo();
        }

        internal Association GetData(int selectedIndex) => OpenedFile.AssociationsList[selectedIndex];

        /// <exception cref="ArgumentOutOfRangeException"/>
        internal ProcessStartInfo GetProcessData(int id)
        {
            if (id >= OpenedFile.AssociationsList.Count || id < 0) throw new ArgumentOutOfRangeException();
            Association association = GetData(id);

            string command = Environment.ExpandEnvironmentVariables(association.Command);
            return new ProcessStartInfo()
            {
                FileName = command,
                Arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{OpenedFile.Location}\""),
                WorkingDirectory = Path.GetDirectoryName(command)
            };
        }
    }
}
