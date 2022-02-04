using FileExtensionHandler.Core.Model;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileExtensionHandler.Core.Tests.Assembly.Samples
{
    internal class Associations
    {
        internal static Dictionary<string, Association> Collection
        {
            get
            {
                Dictionary<string, Association> keyValuePairs = new();
                foreach (Association value in List)
                {
                    keyValuePairs.Add(value.Node ?? "null", value);
                }
                return keyValuePairs;
            }
        }

        internal static List<Association> List => new()
        {
            new Association()
            {
                Node = "fexth.foobar2000.play",
                Name = "Foobar2000 (Play)",
                Icon = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                IconIndex = 0,
                Command = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                Arguments = "%1"
            },
            new Association()
            {
                Node = "fexth.foobar2000.add",
                Name = "Foobar2000 (Add to queue)",
                Icon = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                IconIndex = 0,
                Command = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                Arguments = "/add %1"
            },
            new Association()
            {
                Node = "fexth.wmplayer.play",
                Name = "Windows Media Player",
                Icon = @"%ProgramFiles(x86)%\Windows Media Player\wmplayer.exe",
                IconIndex = 0,
                Command = @"%ProgramFiles(x86)%\Windows Media Player\wmplayer.exe",
                Arguments = "/prefetch:6 /Open %1"
            },
            new Association()
            {
                Node = "fexth.msedge.open",
                Name = "Microsoft Edge",
                Icon = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                IconIndex = 0,
                Command = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                Arguments = "%1"
            },
            new Association()
            {
                Node = "fexth.msedge.open-private",
                Name = "Microsoft Edge (Private Window)",
                Icon = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                IconIndex = 0,
                Command = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                Arguments = "-inprivate %1"
            }
        };

        internal static bool WriteToDisk()
        {
            foreach (KeyValuePair<string, Association> entry in Collection)
            {
                string jsonData = JsonSerializer.Serialize(entry.Value, new JsonSerializerOptions { WriteIndented = true });
                string jsonPath = $@"{Vars.Options.AssociationsDirectory}\{entry.Key}.json";

                File.WriteAllText(jsonPath, jsonData);
                if (File.Exists(jsonPath)) continue;
                return false;
            }
            return true;
        }
    }
}
