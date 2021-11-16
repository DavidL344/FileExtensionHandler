using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileExtensionHandler.Core.Model.Common
{
    public class Arguments
    {
        public readonly string[] RawArgs;
        public readonly bool IsFromProtocol;
        public string FilePath => IsFromProtocol ? ProtocolParse() : RawArgs[0];
        public readonly string Protocol = "fexth://";

        public Arguments(string[] args)
        {
            this.RawArgs = args;
            this.IsFromProtocol = ProtocolCheck();
            ValidateCharacters();
        }

        private bool ProtocolCheck()
        {
            return this.RawArgs[0].StartsWith(this.Protocol);
        }

        private string ProtocolParse()
        {
            // Remove the protocol at the beginning of the argument
            string PathParsed = RawArgs[0].Remove(0, this.Protocol.Length);

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

            if (Path.GetFileName(this.FilePath).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new ArgumentException("The file name contains invalid characters!");
        }
    }
}
