using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Samples;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.Prerequisites
{
    class SaveData
    {
        internal SaveData()
        {
            CheckDirectories();
            CheckFiles();
        }

        static void CheckDirectories()
        {
            string[] directories = new string[] { Vars.Dir_Associations, Vars.Dir_FileExtensions, Vars.Dir_Files };
            foreach (string directory in directories)
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }
        static void CheckFiles()
        {
            FileExtensions fileExtensions = new FileExtensions();

            if (!Directory.EnumerateFiles(Vars.Dir_Associations).Any())
            {
                Associations associations = new Associations();
                associations.WriteToDisk();
            }

            if (!Directory.EnumerateFiles(Vars.Dir_FileExtensions).Any())
            {
                fileExtensions.WriteToDisk();
            }

            List<string> files = new List<string>();
            foreach (KeyValuePair<string, FileExtension> fileExtension in fileExtensions.Collection)
                files.Add(fileExtension.Key);
            files.Add(".unknownFileExtension");

            foreach (string file in files)
            {
                if (!File.Exists(file))
                    File.WriteAllText($@"{Vars.Dir_Files}\Sample{file}", $"This is a sample text in a '{file}' file.");
            }
        }
    }
}
