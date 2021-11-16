using FileExtensionHandler.Core.Model.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Model.Common
{
    class FileHandler
    {
        public OpenedFile OpenedFile;

        /// <param name="filePath">The path containing the file.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <param name="isFromProtocol">Defines if the path comes from a protocol.</param>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        internal void OpenFile(string filePath, string associationsDir, string fileExtensionsDir, bool isFromProtocol)
        {
            OpenedFile = new OpenedFile(filePath, associationsDir, fileExtensionsDir, isFromProtocol);
            OpenedFile.LoadInfo();
        }

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
            Association association = this.GetData(id);

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
