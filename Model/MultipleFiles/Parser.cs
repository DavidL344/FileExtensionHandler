using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace FileExtensionHandler.Model.MultipleFiles
{
    internal class Parser
    {
        private readonly string[] Args;
        internal string FilePath;

        internal Parser(string[] args)
        {
            this.Args = args;
            this.FilePath = this.Args[0];
            this.FilePath = ProtocolParse();
            ValidateCharacters(this.FilePath);
        }

        private string ProtocolParse()
        {
            if (!this.Args[0].StartsWith(Vars.Protocol)) return this.Args[0];

            // Remove the protocol at the beginning of the argument
            FilePath = FilePath.Remove(0, Vars.Protocol.Length);

            // If the file path contains spaces, it is split across multiple arguments
            for (int i = 1; i < this.Args.Length; i++)
                FilePath += $" {this.Args[i]}";

            // Support URL-encoded parameters
            FilePath = HttpUtility.UrlDecode(FilePath);

            // Browsers might add a character at the end of the URL - this removes it
            switch (FilePath.Last())
            {
                case '/':
                case '\\':
                    FilePath = FilePath.Remove(FilePath.Length - 1);
                    break;
                default:
                    break;
            }
            return FilePath;
        }

        private void ValidateCharacters(string FilePath)
        {
            if (FilePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("The path contains invalid characters!");

            if (Path.GetFileName(FilePath).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new ArgumentException("The file name contains invalid characters!");
        }
    }
}
