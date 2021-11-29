using FileExtensionHandler.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.Samples
{
    class Associations
    {
        readonly Dictionary<string, Association> Collection = new Dictionary<string, Association>();

        internal Associations()
        {
            object[][] samples = new object[][]
            {
                new object[] {
                    "fexth.foobar2000.play", "Foobar2000 (Play)",
                    null, 0,
                    @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe", "%1"
                },
                new object[] {
                    "fexth.foobar2000.add", "Foobar2000 (Add to queue)",
                    null, 0,
                    @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe", "/add %1"
                },

                new object[] {
                    "fexth.wmplayer.play", "Windows Media Player",
                    null, 0,
                    @"%ProgramFiles(x86)%\Windows Media Player\wmplayer.exe", "/prefetch:6 /Open %1"
                },

                new object[] {
                    "fexth.msedge.open", "Microsoft Edge",
                    @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe", 0,
                    @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe", "%1"
                },

                new object[] {
                    "fexth.msedge.open-private", "Microsoft Edge (Private Window)",
                    @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe", 0,
                    @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe", "-inprivate %1"
                },
                
            };

            foreach (object[] sample in samples)
            {
                Collection.Add((string)sample[0], new Association
                {
                    Name = (string)sample[1],
                    Icon = (string)sample[2],
                    IconIndex = (int)sample[3],
                    Command = (string)sample[4],
                    Arguments = (string)sample[5]
                });
            }
        }

        internal void WriteToDisk()
        {
            foreach (KeyValuePair<string, Association> entry in Collection)
            {
                string jsonData = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                File.WriteAllText($@"{Vars.Dir_Associations}\{entry.Key}.json", jsonData);
            }
        }
    }
}
