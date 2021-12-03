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
        public bool Streamed => ProtocolsUsed.Intersect(CommunicationProtocols).Any();

        public string[] ProtocolsUsed { get; private set; }
        public bool CalledFromAppProtocol => ProtocolsUsed.Contains(AppProtocol);

        private readonly string AppProtocol;
        private readonly string[] Protocols;
        private readonly string[] CommunicationProtocols;

        public Arguments(string[] args, string appProtocol = "fexth://")
        {
            this.AppProtocol = appProtocol;
            this.Protocols = new string[] { appProtocol, "file:///", "http://", "https://" };
            this.CommunicationProtocols = new string[] { "http://", "https://" };

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
                if (!CommunicationProtocols.Contains(protocol))
                    pathParsed = pathParsed.Remove(protocolIndex, protocol.Length);
            }
            this.ProtocolsUsed = protocolsUsed.ToArray();

            // Support URL-encoded parameters
            pathParsed = HttpUtility.UrlDecode(pathParsed);

            // Browsers might add a character at the end of the URL - this removes it
            switch (pathParsed.Last())
            {
                case '/':
                case '\\':
                    pathParsed = pathParsed.Remove(pathParsed.Length - 1);
                    break;
                default:
                    break;
            }
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
