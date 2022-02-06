using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Assembly.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileExtensionHandler.Core.Tests.Assembly
{
    [TestClass]
    public class Assembly
    {
        private static readonly string[] Directories = new string[] { Vars.Options.AssociationsDirectory, Vars.Options.FileExtensionsDirectory, Vars.Dir_Test_Files };
        private static List<string> FileExtensionNameList
        {
            get
            {
                List<string> list = new();
                foreach (KeyValuePair<string, FileExtension> fileExtension in FileExtensions.Collection)
                    list.Add(fileExtension.Key);
                list.Add(".unknownFileExtension");
                return list;
            }
        }

        [AssemblyInitialize]
        public static void InitAssembly(TestContext context)
        {
            CheckDirectories();
            CheckFiles();
        }

        [TestMethod]
        public void IsInitializedProperly()
        {
            foreach (string directory in Directories)
                Assert.IsTrue(Directory.Exists(directory));

            Assert.IsTrue(Directory.GetFiles(Vars.Options.AssociationsDirectory, "*.json", SearchOption.TopDirectoryOnly).Length == Associations.List.Count, "The association count doesn't match!");
            Assert.IsTrue(Directory.GetFiles(Vars.Options.FileExtensionsDirectory, "*.json", SearchOption.TopDirectoryOnly).Length == FileExtensions.List.Count, "The file extension count doesn't match!");
            Assert.IsTrue(Directory.GetFiles(Vars.Dir_Test_Files, "*", SearchOption.TopDirectoryOnly).Length == FileExtensionNameList.Count, "The sample file count doesn't match!" + $"{Directory.GetFiles(Vars.Dir_Test_Files, "*", SearchOption.TopDirectoryOnly).Length} {FileExtensionNameList.Count}");

            foreach (Association association in Associations.List)
            {
                string joinedPath = Path.Join(Vars.Options.AssociationsDirectory, $"{association.Node}.json");
                Assert.IsTrue(File.Exists(joinedPath), $"Association '{association.Node}' doesn't exist!\r\nPath: {joinedPath}");
            }
            
            foreach (FileExtension fileExtension in FileExtensions.List)
            {
                string joinedPath = Path.Join(Vars.Options.FileExtensionsDirectory, $"{fileExtension.Node}.json");
                Assert.IsTrue(File.Exists(joinedPath), $"File extension '{fileExtension.Node}' doesn't exist!\r\nPath: {joinedPath}");
            }

            foreach (string fileExtension in FileExtensionNameList)
            {
                string file = $"Sample{fileExtension}";
                string joinedPath = Path.Join(Vars.Dir_Test_Files, file);
                Assert.IsTrue(File.Exists(joinedPath), $"File '{file}' doesn't exist!\r\nPath: {joinedPath}");
            }
        }

        public static void CheckDirectories()
        {
            foreach (string directory in Directories)
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        static void CheckFiles()
        {
            if (!Directory.EnumerateFiles(Vars.Options.AssociationsDirectory).Any())
            {
                if (!Associations.WriteToDisk()) throw new System.Exception("One of the associations wasn't successfully written to disk!");
            }

            if (!Directory.EnumerateFiles(Vars.Options.FileExtensionsDirectory).Any())
            {
                if (!FileExtensions.WriteToDisk()) throw new System.Exception("One of the file extensions wasn't successfully written to disk!");
            }

            if (!Directory.EnumerateFiles(Vars.Dir_Test_Files).Any())
            {
                foreach (string file in FileExtensionNameList)
                {
                    if (!File.Exists(file))
                        File.WriteAllText($@"{Vars.Dir_Test_Files}\Sample{file}", $"This is a sample text in a '{file}' file.");
                }
            }
        }
    }
}