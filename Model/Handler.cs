﻿using Newtonsoft.Json;
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
        internal Dictionary<string, FileExtension> Data {
            get
            {
                if (File.Exists(FilePath))
                {
                    string jsonData = File.ReadAllText(FilePath);
                    return JsonConvert.DeserializeObject<Dictionary<string, FileExtension>>(jsonData);
                }
                return null;
            }
        }
        internal string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\fexth\associations.json";

        public Handler()
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath))) Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
        }

        internal void Save(Dictionary<string, FileExtension> data = null)
        {
            if (data == null) data = Data;
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(FilePath, jsonData);
        }

        internal Association GetData(string fileExtension, int selectedIndex)
        {
            return Data[fileExtension].Associations[selectedIndex];
        }

        internal int GetCount(string fileExtension)
        {
            return Data[fileExtension].Associations.Count;
        }

        internal void GenerateSomeAssociations()
        {
            Dictionary<string, FileExtension> CustomData = new Dictionary<string, FileExtension>
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
            Save(CustomData);
        }
    }
}
