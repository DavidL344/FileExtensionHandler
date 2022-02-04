using FileExtensionHandler.Core.Model;

namespace FileExtensionHandler.Core.Controller
{
    public class SamplesController
    {
        public static Dictionary<string, FileExtension> FileExtensions => new()
        {
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
            },
            {
                ".wav",
                new FileExtension
                {
                    Node = ".wav",
                    Name = "Wave Sound",
                    Icon = @"%SystemRoot%\system32\wmploc.dll",
                    IconIndex = -734,
                    Associations = new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    },
                    DefaultAssociation = null
                }
            }
        };

        public static Dictionary<string, Association> Associations => new()
        {
            {
                "fexth.foobar2000.play", new Association
                {
                    Node = "fexth.foobar2000.play",
                    Name = "Foobar2000 (Play)",
                    Icon = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
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
                    Icon = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
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
                    Icon = @"%ProgramFiles(x86)%\Windows Media Player\wmplayer.exe",
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
                    Icon = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
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
                    Icon = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe",
                    Arguments = "-inprivate %1"
                }
            }
        };

        public static void Write<T>(Dictionary<string, T> keyValuePairs, string directoryPath) where T : struct
        {
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            foreach (KeyValuePair<string, T> entry in keyValuePairs)
                SerializationController.SerializeToFile(entry.Value, $@"{directoryPath}\{entry.Key}.json");
        }
    }
}