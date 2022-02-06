using FileExtensionHandler.Core.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileExtensionHandler.Core.Tests.Assembly.Samples
{
    internal class FileExtensions
    {
        internal static Dictionary<string, FileExtension> Collection
        {
            get
            {
                Dictionary<string, FileExtension> keyValuePairs = new();
                foreach (FileExtension value in List)
                {
                    keyValuePairs.Add(value.Node ?? "null", value);
                }
                return keyValuePairs;
            }
        }

        internal static List<FileExtension> List => new()
        {
            new FileExtension()
            {
                Node = ".mp3",
                Name = "MP3 Format Sound",
                Icon = @"%SystemRoot%\system32\wmploc.dll",
                IconIndex = -732,
                Associations = Shared.AudioAssociations,
                DefaultAssociation = Shared.AudioAssociations[0]
            },
            new FileExtension()
            {
                Node = ".flac",
                Name = "FLAC Audio",
                Icon = @"%SystemRoot%\system32\wmploc.dll",
                IconIndex = -738,
                Associations = Shared.AudioAssociations,
                DefaultAssociation = Shared.AudioAssociations[0]
            },
            new FileExtension()
            {
                Node = ".pdf",
                Name = "PDF File",
                Icon = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                IconIndex = 13,
                Associations = Shared.BrowserAssociations,
                DefaultAssociation = Shared.BrowserAssociations[0]
            },
            new FileExtension()
            {
                Node = ".noassociations",
                Name = "A file extension with no associations"
            },
            new FileExtension()
            {
                Node = ""
            }
        };

        internal static bool WriteToDisk()
        {
            foreach (KeyValuePair<string, FileExtension> entry in Collection)
            {
                string jsonData = JsonSerializer.Serialize(entry.Value, new JsonSerializerOptions { WriteIndented = true });
                string jsonPath = $@"{Vars.Options.FileExtensionsDirectory}\{entry.Key}.json";

                File.WriteAllText(jsonPath, jsonData);
                if (File.Exists(jsonPath)) continue;
                return false;
            }
            return true;
        }
    }
}