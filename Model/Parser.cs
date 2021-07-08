using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace FileExtensionHandler.Model
{
    internal class Parser
    {
        internal Handler Handler = new Handler();
        internal List<Association> AssociationsList;
        internal FileExtension FileExtensionInfo;
        internal string FilePath;
        internal string FileName;
        internal string FileType;
        internal string FileExtension;

        internal Parser(string[] args)
        {
            string filePath = args[0];
            string fexthProtocol = "fexth://";
            if (filePath.StartsWith(fexthProtocol))
            {
                // Remove the protocol at the beginning of the argument
                filePath = filePath.Remove(0, fexthProtocol.Length);

                // If the file path contains spaces, it is split across multiple arguments
                for (int i = 1; i < args.Length; i++)
                    filePath += $" {args[i]}";

                // Support URL-encoded parameters
                filePath = HttpUtility.UrlDecode(filePath);

                // Browsers might add a character at the end of the URL - this removes it
                switch (filePath.Last())
                {
                    case '/':
                    case '\\':
                        filePath = filePath.Remove(filePath.Length - 1);
                        break;
                    default:
                        break;
                }
            }

            if (filePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("The path contains invalid characters!");

            if (Path.GetFileName(filePath).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new ArgumentException("The file name contains invalid characters!");

            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            this.FileExtension = Path.GetExtension(filePath);

            if (Handler.Data == null) Handler.GenerateSomeAssociations();
            if (Handler.Data.ContainsKey(FileExtension))
            {
                FileExtensionInfo = Handler.Data[FileExtension];
                AssociationsList = Handler.Data[FileExtension].Associations;

                this.FileType = !string.IsNullOrWhiteSpace(FileExtensionInfo.Type) ? FileExtensionInfo.Type : null;
            }
            else
            {
                throw new FileFormatException($"The there's no app associated with {FileExtension}!");
            }
        }

        internal void RunApp(int id = -1)
        {
            if (id == -1) return;
            Association association = Handler.GetData(FileExtension, id);
            string arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{FilePath}\"");

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = association.Path,
                Arguments = arguments,
                WorkingDirectory = Path.GetDirectoryName(association.Path)
            };
            Process.Start(processStartInfo);
            Application.Current.Shutdown();
        }

        internal void ParseFile()
        {
            if (Handler.GetCount(Path.GetExtension(FileExtension)) == 1)
            {
                List<Association> associationsList = Handler.Data[FileExtension].Associations;
                switch (associationsList.Count)
                {
                    case 0:
                        break;
                    case 1:
                        RunApp(0);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
