using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileExtensionHandler.Core
{
    public class Arguments
    {
        public readonly string[] Raw;
        public string RawJoined => String.Join(" ", Raw);

        public readonly string Parsed;
        public readonly string ParsedNoParameters;

        public string Name => Path.GetFileName(ParsedNoParameters);
        public bool Streamed
        {
            get
            {
                int minimumProtocolsToQualify = 1;
                if (ProtocolsUsed.Contains("file:///") || ProtocolsUsed.Contains("file://")) minimumProtocolsToQualify++;
                if (ProtocolsUsed.Contains("fexth://") || ProtocolsUsed.Contains("fexth:")) minimumProtocolsToQualify++;
                return ProtocolsUsed.Count() >= minimumProtocolsToQualify;
            }
        }

        public string[] ProtocolsUsed { get; private set; }
        public bool CalledFromAppProtocol => ProtocolsUsed.Contains($"{AppProtocol}://") || ProtocolsUsed.Contains($"{AppProtocol}:");

        private readonly string AppProtocol;
        private readonly string[] Protocols;
        private readonly string[] CommunicationProtocols;

        public Arguments(string[] args, string appProtocol = "fexth")
        {
            this.AppProtocol = appProtocol;
            this.CommunicationProtocols = new string[] { "http", "https", "ftp", "ftps", "smb" };

            List<string> protocolList = new List<string>
            {
                $"{appProtocol}://", $"{appProtocol}:", "file:///", "file://"
            };
            foreach (string communicationProtocol in this.CommunicationProtocols)
            {
                protocolList.Add($"{communicationProtocol}://");
                protocolList.Add($"{communicationProtocol}:");
            }
            this.Protocols = protocolList.ToArray();

            this.Raw = args;
            this.Parsed = this.Parse();

            // Streamed files might have URL parameters in them, making the file name seem invalid
            // Example: https://www.example.com/file.pdf?ver=1.2
            this.ParsedNoParameters = this.Streamed ? this.Parsed.Split('?')[0] : this.Parsed;
            ValidateCharacters();
        }

        private string Parse()
        {
            string pathParsed = String.Join(" ", this.Raw);
            List<string> protocolsUsed = new List<string>();

            foreach (string protocol in Protocols)
            {
                // Take note of the protocols used
                int protocolIndex = pathParsed.IndexOf(protocol);
                if (protocolIndex < 0) continue;
                protocolsUsed.Add(protocol);

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
            this.ProtocolsUsed = protocolsUsed.ToArray();

            // Support URL-encoded parameters
            pathParsed = HttpUtility.UrlDecode(pathParsed);

            // Browsers might add a character at the end of the URL - this removes it
            if (pathParsed.Length == 0) return pathParsed;
            char[] trailingCharacters = { '/', '\\' };
            if (trailingCharacters.Contains(pathParsed.Last()))
                pathParsed = pathParsed.Remove(pathParsed.Length - 1);
            return pathParsed;
        }

        private void ValidateCharacters()
        {
            if (this.ParsedNoParameters.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("The file path contains invalid characters!");

            if (Path.GetFileName(this.ParsedNoParameters).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new ArgumentException("The file name contains invalid characters!");
        }
    }
}
