using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.Samples
{
    class Arguments
    {
        private static readonly string BaselinePath = $@"{Vars.Dir_Files}\Sample.mp3";
        private static readonly string StreamablePath = $@"https://www.example.com/Sample.mp3";

        public static readonly Dictionary<string, string[]> Valid = new Dictionary<string, string[]>()
        {
            { "Disk", new string[] { BaselinePath } },
            { "DiskFileProtocol", new string[] { $"file:///{BaselinePath}".Replace('\\', '/') } },

            { "FexthDisk", new string[] { $"fexth://{BaselinePath}".Replace('\\', '/') } },
            { "FexthDiskFileProtocol", new string[] { $"fexth://file:///{BaselinePath}".Replace('\\', '/') } },

            { "Streamed", new string[] { StreamablePath } },
            { "FexthStreamedFileProtocol", new string[] { $"fexth://{StreamablePath}" } }
        };

        public static readonly Dictionary<string, string[]> Invalid = new Dictionary<string, string[]>()
        {
            // *
            { "Disk_Path", new string[] { Valid["Disk"][0].Insert(0, "|") } },
            { "Disk_FileName", new string[] { Valid["Disk"][0].Reverse().ToString().Insert(0, "|").Reverse().ToString() } },

            // file:///*
            { "DiskFileProtocol_Path", new string[] { $"file:///{BaselinePath}".Replace('\\', '/').Insert(0, "|") } },
            { "DiskFileProtocol_FileName", new string[] { $"file:///{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() } },
            
            // fexth://*
            { "FexthDisk_Path", new string[] { $"fexth://{BaselinePath}".Replace('\\', '/').Insert(0, "|") } },
            { "FexthDisk_FileName", new string[] { $"fexth://{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() } },

            // fexth://file:///*
            { "FexthDiskFileProtocol_Path", new string[] { $"fexth://file:///{BaselinePath}".Replace('\\', '/').Insert(0, "|") } },
            { "FexthDiskFileProtocol_FileName", new string[] { $"fexth://file:///{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() } },

            // https://*
            { "Streamed_Path", new string[] { Valid["Streamed"][0].Insert(0, "|") } },
            { "Streamed_FileName", new string[] { Valid["Streamed"][0].Reverse().ToString().Insert(0, "|").Reverse().ToString() } },

            // fexth://https://*
            { "FexthStreamed_Path", new string[] { $"fexth://{BaselinePath}".Replace('\\', '/').Insert(0, "|") } },
            { "FexthStreamed_FileName", new string[] { $"fexth://{BaselinePath}".Replace('\\', '/').Reverse().ToString().Insert(0, "|").Reverse().ToString() } }
        };
        public static readonly string[] FileWithNoFileExtensionInfo = new string[] { Path.ChangeExtension(BaselinePath, ".unknownFileExtension") };
        public static readonly string[] FileWithNoAssociations = new string[] { Path.ChangeExtension(BaselinePath, ".noassociations") };
    }
}
