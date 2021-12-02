using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.Samples
{
    class FilePaths
    {
        private static readonly string BaselinePath = $@"{Vars.Dir_Files}\Sample.mp3";
        public static readonly Dictionary<string, string> Valid = new Dictionary<string, string>()
        {
            { "Disk", BaselinePath },
            { "DiskFileProtocol", $"file:///{BaselinePath}".Replace('\\', '/') },
            { "FexthDisk", $"fexth://{BaselinePath}".Replace('\\', '/') },
            { "FexthDiskFileProtocol", $"fexth://file:///{BaselinePath}".Replace('\\', '/') }
        };
        public static readonly Dictionary<string, string> Invalid = new Dictionary<string, string>()
        {
            { "Disk_Path", Valid["Disk"].Insert(0, "|") },
            { "Disk_FileName", Valid["Disk"].Reverse().ToString().Insert(0, "|").Reverse().ToString() },

            { "DiskFileProtocol_Path", $"file:///{BaselinePath}".Replace('\\', '/').Insert(0, "|") },
            { "DiskFileProtocol_FileName", $"file:///{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() },

            { "FexthDisk_Path", $"fexth://{BaselinePath}".Replace('\\', '/').Insert(0, "|") },
            { "FexthDisk_FileName", $"fexth://{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() },

            { "FexthDiskFileProtocol_Path", $"fexth://file:///{BaselinePath}".Replace('\\', '/').Insert(0, "|") },
            { "FexthDiskFileProtocol_FileName", $"fexth://file:///{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() }
        };
        public static readonly string FileWithNoFileExtensionInfo = Path.ChangeExtension(BaselinePath, ".unknownFileExtension");
        public static readonly string FileWithNoAssociations = Path.ChangeExtension(BaselinePath, ".noassociations");
    }
}
