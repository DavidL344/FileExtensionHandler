using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileExtensionHandler.Core.Model.Common
{
    class Arguments
    {
        public readonly string[] RawArgs;
        public readonly bool IsFromProtocol;
        public string FilePath => IsFromProtocol ? ProtocolParse() : RawArgs[0];

        public readonly string[] Protocol = new string[] { "fexth://", "file:///", "http://", "https://" };
        public readonly string[] ImmutableProtocol = new string[] { "http://", "https://" };

        public Arguments(string[] args)
        {
            this.RawArgs = args;
            this.IsFromProtocol = ProtocolCheck();
            ValidateCharacters();
        }

        private bool ProtocolCheck()
        {
            foreach (string protocol in Protocol)
                if (this.RawArgs[0].StartsWith(protocol)) return true;
            return false;
        }

        private string GetProtocol()
        {
            string value = "";
            foreach (string protocol in Protocol)
                if (this.RawArgs[0].Contains(protocol) && !ImmutableProtocol.Contains(protocol)) value += protocol;
            return value;
        }

        private string ProtocolParse()
        {
            // Get the protocol length to be removed from the first element
            int ProtocolLength = GetProtocol().Length;

            // Remove the protocol at the beginning of the argument
            string PathParsed = RawArgs[0].Remove(0, ProtocolLength);

            // If the file path contains spaces, it is split across multiple arguments
            for (int i = 1; i < this.RawArgs.Length; i++)
                PathParsed += $" {this.RawArgs[i]}";

            // Support URL-encoded parameters
            PathParsed = HttpUtility.UrlDecode(PathParsed);

            // Browsers might add a character at the end of the URL - this removes it
            switch (PathParsed.Last())
            {
                case '/':
                case '\\':
                    PathParsed = PathParsed.Remove(PathParsed.Length - 1);
                    break;
                default:
                    break;
            }
            return PathParsed;
        }

        private void ValidateCharacters()
        {
            if (this.FilePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("The file path contains invalid characters!");

            if (!IsFromProtocol)
                if (Path.GetFileName(this.FilePath).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                    throw new ArgumentException("The file name contains invalid characters!");
        }
    }
}
