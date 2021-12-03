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
    class FileExtensions
    {
        internal readonly Dictionary<string, FileExtension> Collection = new Dictionary<string, FileExtension>();
        readonly Dictionary<string, FileExtension> CollectionNoDefaults = new Dictionary<string, FileExtension>();

        internal FileExtensions()
        {
            object[][] samples = new object[][]
            {
                new object[] {
                    ".mp3", "MP3 Format Sound",
                    @"%SystemRoot%\system32\wmploc.dll", -732,
                    new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    }, "fexth.foobar2000.play"
                },
                new object[] {
                    ".flac", "FLAC Audio",
                    @"%SystemRoot%\system32\wmploc.dll", -738,
                    new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    }, "fexth.foobar2000.play"
                },
                new object[] {
                    ".pdf", "PDF File",
                    @"%ProgramFiles(x86)%\Microsoft\Edge\Application\msedge.exe", 13,
                    new string[]
                    {
                        "fexth.msedge.open",
                        "fexth.msedge.open-private"
                    }, "fexth.msedge.open"
                },
                new object[] {
                    ".noassociations", "A file extension with no associations",
                    null, 0,
                    new string[] { }, null
                },
                new object[] {
                    "", "A file with no file extension",
                    null, 0,
                    new string[] { }, null
                }
            };

            foreach (object[] sample in samples)
            {
                Collection.Add((string)sample[0], new FileExtension
                {
                    Name = (string)sample[1],
                    Icon = (string)sample[2],
                    IconIndex = (int)sample[3],
                    Associations = (string[])sample[4],
                    DefaultAssociation = (string)sample[5]
                });

                CollectionNoDefaults.Add((string)sample[0], new FileExtension
                {
                    Name = (string)sample[1],
                    Icon = (string)sample[2],
                    IconIndex = (int)sample[3],
                    Associations = (string[])sample[4],
                    DefaultAssociation = null
                });
            }
        }

        internal void WriteToDisk(bool includeDefaults = true)
        {
            Dictionary<string, FileExtension> SerializableCollection = includeDefaults ? Collection : CollectionNoDefaults;
            foreach (KeyValuePair<string, FileExtension> entry in SerializableCollection)
            {
                string jsonData = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                File.WriteAllText($@"{Vars.Dir_FileExtensions}\{entry.Key}.json", jsonData);
            }
        }
    }
}
