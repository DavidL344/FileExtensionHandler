using FileExtensionHandler.Core.Model;
using System.Diagnostics;
using System.Web;

namespace FileExtensionHandler.Core.Controller
{
    public class FileInformationController
    {
        public string AppProtocol { get; set; } = "fexth";
        public string[] CommunicationProtocols { get; set; } = { "http", "https", "ftp", "ftps", "smb" };
        public bool RemoveTrailingCharacter { get; set; } = true;
        public bool ThrowOnInvalidCharacters { get; set; } = true;
        public bool UseUrlDecode { get; set; } = true;

        private readonly string[] _protocols;
        private List<Association> _associations;
        private FileExtension _fileExtension;
        private readonly Options _options;

        public FileInformationController(Options options)
        {
            _options = options;
            CheckEnv();

            List<string> protocolList = new()
            {
                $"{AppProtocol}://", $"{AppProtocol}:", "file:///", "file://"
            };
            foreach (string communicationProtocol in CommunicationProtocols)
            {
                protocolList.Add($"{communicationProtocol}://");
                protocolList.Add($"{communicationProtocol}:");
            }

            _protocols = protocolList.ToArray();
            _associations = new List<Association>();
        }

        public FileInformation Parse(params string[] args)
        {
            string pathParsed = String.Join(" ", args);
            List<string> protocolsUsed = new();

            foreach (string protocol in _protocols)
            {
                // When a protocol doesn't get removed from the parsed string,
                // its substring version would be recognized as a unique protocol again
                // Example: protocol "https://" would be recognized as both "https://" and "https:"
                bool protocolAlreadyAdded = false;
                foreach (string protocolUsed in protocolsUsed)
                {
                    if (protocol.Contains(protocolUsed))
                    {
                        protocolAlreadyAdded = true;
                        break;
                    }
                }
                if (protocolAlreadyAdded) continue;

                // Take note of the protocols used
                int protocolIndex = pathParsed.IndexOf(protocol);
                if (protocolIndex < 0) continue;
                string protocolName = protocol.Replace(":///", String.Empty).Replace("://", String.Empty).Replace(":", String.Empty);
                protocolsUsed.Add(protocolName);

                // Remove all protocols except the ones used for client-server communication
                string[] protocolEndings = new string[] { ":///", "://", ":" };
                string rawProtocol = string.Empty;

                foreach (string protocolEnding in protocolEndings)
                {
                    if (!protocol.EndsWith(protocolEnding)) continue;
                    rawProtocol = protocol.Substring(0, protocol.Length - protocolEnding.Length);
                    if (CommunicationProtocols.Contains(rawProtocol)) continue;
                    pathParsed = pathParsed.Remove(protocolIndex, protocol.Length);
                }
            }

            // Support URL-encoded parameters
            if (UseUrlDecode) pathParsed = HttpUtility.UrlDecode(pathParsed);

            // Browsers might add a character at the end of the URL - this removes it
            if (RemoveTrailingCharacter && pathParsed.Length > 0)
            {
                char[] trailingCharacters = { '/', '\\' };
                if (trailingCharacters.Contains(pathParsed.Last()))
                    pathParsed = pathParsed.Remove(pathParsed.Length - 1);
            }

            // Streamed files might have URL parameters in them, making the file name seem invalid
            // Example: https://www.example.com/file.pdf?ver=1.2
            // This is now handled directly in the FileInformation.LocationNoParameters
            string pathParsedNoParameters = pathParsed.Split('?')[0];

            // Throwing an exception allows to pass the details of the error
            // rather than just bool like FileInformation.IsLocationValid does
            if (ThrowOnInvalidCharacters) ValidateCharacters(pathParsedNoParameters);

            string fileExtensionNode = Path.GetExtension(pathParsedNoParameters);
            _fileExtension = FileExtensionsController.LoadFromJson(fileExtensionNode, _options.FileExtensionsDirectory);
            _associations = AssociationsController.GetAssociations(_fileExtension, _options.AssociationsDirectory);

            return new FileInformation
            {
                AppProtocol = AppProtocol,
                Location = pathParsed,
                Protocols = protocolsUsed.ToArray(),
                FileExtension = _fileExtension,
                Associations = _associations
            };
        }

        public static void ValidateCharacters(string parsedPathNoParameters)
        {
            if (parsedPathNoParameters.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("The file path contains invalid characters!");

            if (Path.GetFileName(parsedPathNoParameters).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new ArgumentException("The file name contains invalid characters!");
        }

        public void OpenFile(string filePath, FileInformation fileInformation, int id = -1)
        {
            if (id == -1)
            {
                int defaultAssociationIndex = fileInformation.DefaultAssociationIndex;
                if (defaultAssociationIndex == -1)
                {
                    string location = fileInformation.Streamed ? fileInformation.Location : fileInformation.LocationNoParameters;
                    Process.Start(location);
                    return;
                }
                id = defaultAssociationIndex;
            }
            if (id >= _associations.Count || id < 0) throw new ArgumentOutOfRangeException();
            OpenFile(filePath, _associations[id]);
        }

        public static void OpenFile(string filePath, Association association)
        {
            string command = Environment.ExpandEnvironmentVariables(association.Command);
            ProcessStartInfo processStartInfo = new()
            {
                FileName = command,
                Arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{filePath}\""),
                WorkingDirectory = Path.GetDirectoryName(command)
            };
            Process.Start(processStartInfo);
        }

        private void CheckEnv()
        {
            if (!Directory.Exists(_options.AssociationsDirectory)) throw new DirectoryNotFoundException("The directory with associations doesn't exist!");
            if (!Directory.Exists(_options.FileExtensionsDirectory)) throw new DirectoryNotFoundException("The directory with file extension information doesn't exist!");
        }
    }
}
