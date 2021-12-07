using FileExtensionHandler.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core
{
    public class Samples
    {
        public static Dictionary<string, FileExtension> FileExtensions => new Dictionary<string, FileExtension>{
            {
                ".mp3", new FileExtension
                {
                    Node = ".mp3",
                    Name = "MP3 Format Sound",
                    Icon = @"%SystemRoot%\system32\wmploc.dll",
                    IconIndex = -732,
                    Associations = new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    },
                    DefaultAssociation = null
                }
            },
            {
                ".flac", new FileExtension
                {
                    Node = ".flac",
                    Name = "FLAC Audio",
                    Icon = @"%SystemRoot%\system32\wmploc.dll",
                    IconIndex = -738,
                    Associations = new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    },
                    DefaultAssociation = null
                }
            },
            {
                ".pdf", new FileExtension
                {
                    Node = ".pdf",
                    Name = "PDF File",
                    Icon = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                    IconIndex = 13,
                    Associations = new string[]
                    {
                        "fexth.msedge.open",
                        "fexth.msedge.open-private"
                    },
                    DefaultAssociation = null
                }
            }
        };

        public static Dictionary<string, Association> Associations => new Dictionary<string, Association>()
        {
            {
                "fexth.foobar2000.play", new Association
                {
                    Node = "fexth.foobar2000.play",
                    Name = "Foobar2000 (Play)",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                    Arguments = "%1"
                }
            },
            {
                "fexth.foobar2000.add", new Association
                {
                    Node = "fexth.foobar2000.add",
                    Name = "Foobar2000 (Add to queue)",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                    Arguments = "/add %1"
                }
            },
            {
                "fexth.wmplayer.play", new Association
                {
                    Node = "fexth.wmplayer.play",
                    Name = "Windows Media Player",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\Windows Media Player\wmplayer.exe",
                    Arguments = "/prefetch:6 /Open %1"
                }
            },
            {
                "fexth.msedge.open", new Association
                {
                    Node = "fexth.msedge.open",
                    Name = "Microsoft Edge",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                    Arguments = "%1"
                }
            },
            {
                "fexth.msedge.open-private", new Association
                {
                    Node = "fexth.msedge.open-private",
                    Name = "Microsoft Edge (Private Window)",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                    Arguments = "-inprivate %1"
                }
            }
        };

        public static void SaveToDisk(Dictionary<string, FileExtension> fileExtensionsList, string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            foreach (KeyValuePair<string, FileExtension> entry in fileExtensionsList)
            {
                string jsonData = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                File.WriteAllText($@"{directoryPath}\{entry.Key}.json", jsonData);
            }
        }

        public static void SaveToDisk(Dictionary<string, Association> associationsList, string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            foreach (KeyValuePair<string, Association> entry in associationsList)
            {
                string jsonData = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                File.WriteAllText($@"{directoryPath}\{entry.Key}.json", jsonData);
            }
        }
    }
}
