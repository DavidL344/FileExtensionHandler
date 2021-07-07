using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model
{
    internal class Handler
    {
        internal Dictionary<string, FileExtension> Data;
        internal string FilePath = Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\associations.json");

        public Handler()
        {
            Load();
        }

        internal void Load()
        {
            Data = null;
            if (File.Exists(FilePath))
            {
                string jsonData = File.ReadAllText(FilePath);
                Data = JsonConvert.DeserializeObject<Dictionary<string, FileExtension>>(jsonData);
                return;
            }
            Data = null;
        }

        internal void Save()
        {
            string jsonData = JsonConvert.SerializeObject(Data, Formatting.Indented);
            File.WriteAllText(FilePath, jsonData);
        }

        internal void GenerateSomeAssociations()
        {
            Data = new Dictionary<string, FileExtension>
            {
                {
                    ".flac",
                    new FileExtension(new List<Association>
                    {
                        new Association("Foobar2000 (FLAC)", @"C:\Program Files (x86)\foobar2000\foobar2000.exe", "%1"),
                        new Association("Windows Media Player (FLAC)", @"C:\Program Files (x86)\Windows Media Player\wmplayer.exe", "/prefetch:6 /Open %1")
                    })
                },
                {
                    ".mp3",
                    new FileExtension(new List<Association>
                    {
                        new Association("Foobar2000", @"C:\Program Files (x86)\foobar2000\foobar2000.exe", "%1"),
                        new Association("Windows Media Player", @"C:\Program Files (x86)\Windows Media Player\wmplayer.exe", "/prefetch:6 /Open %1")
                    })
                }
            };
            Save();
        }
    }
}
