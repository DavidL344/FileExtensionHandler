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

            if (!File.Exists(Vars.Dir_Associations))
            {
                Associations associations = new Associations();
                associations.WriteToDisk();
            }

            if (!File.Exists(Vars.Dir_FileExtensions))
            {
                fileExtensions.WriteToDisk();
            }

            List<string> files = new List<string>();
            foreach (KeyValuePair<string, Model.Shared.FileExtension> fileExtension in fileExtensions.Collection)
                files.Add(fileExtension.Key);

            foreach (string file in files)
            {
                if (!File.Exists(file))
                    File.WriteAllText($@"{Vars.Dir_Files}\Sample{file}", $"This is a sample text in a '{file}' file.");
            }
        }
    }
}
