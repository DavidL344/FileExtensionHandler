using FileExtensionHandler.Core.Common;
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
        internal FileHandler FileHandler = new FileHandler();
        public OpenedFile Data;

        /// <summary>
        /// Loads the file information.
        /// </summary>
        /// <inheritdoc cref="FileHandler.OpenFile(string, string, string, bool)"/>
        public FileInformation(string filePath, string associationsDir, string fileExtensionsDir)
        {
            Arguments arguments = new Arguments(new string[] { filePath });
            FileHandler.OpenFile(arguments.FilePath, associationsDir, fileExtensionsDir, arguments.IsFromProtocol);
            Data = FileHandler.OpenedFile;
        }

        /// <summary>
        /// Loads the file information from the arguments passed.
        /// </summary>
        /// <param name="args">Command line arguments passed to the app.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="FileNotFoundException"/>
        public FileInformation(string[] args, string associationsDir, string fileExtensionsDir)
        {
            FileHandler.OpenFile(args, associationsDir, fileExtensionsDir);
            Data = FileHandler.OpenedFile;
        }

        /// <summary>
        /// Gets the associated app's information for opening the file.
        /// </summary>
        /// <param name="id">ID of the association from the file extension's list.</param>
        /// <returns>ProcessStartInfo for the selected association.</returns>
        public ProcessStartInfo GetProcessData(int id)
        {
            return FileHandler.GetProcessData(id);
        }

        /// <summary>
        /// Opens the file with a selected application
        /// </summary>
        /// <param name="id">ID of the association from the file extension's list.</param>
        public void OpenWith(int id)
        {
            if (id == -1)
            {
                if (Data.DefaultAssociationIndex == -1) return;
                id = Data.DefaultAssociationIndex;
            }
            ProcessStartInfo processStartInfo = FileHandler.GetProcessData(id);

            if (processStartInfo.Arguments == null)
            {
                // Did I just really spend my time parsing the registry values when I just could call the file?
            }
            Process.Start(processStartInfo);
        }
    }
}
