using FileExtensionHandler.Core.Tests.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileExtensionHandler.Core.Tests
{
    [TestClass]
    public class FilePathsTest
    {
        [TestMethod]
        public void GetFileInformation()
        {
            foreach (KeyValuePair<string, string> validFilePath in FilePaths.Valid)
            {
                FileInformation fileInformation = new FileInformation(validFilePath.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                Assert.AreEqual(Path.GetExtension(Path.GetExtension(validFilePath.Value)), fileInformation.Data.Extension);
            }
        }

        [TestMethod]
        public void ReadFileAssociations()
        {
            foreach (KeyValuePair<string, string> validArgument in FilePaths.Valid)
            {
                FileInformation fileInformation = new FileInformation(validArgument.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                Assert.IsNotNull(fileInformation.Data.AssociationsList);
                Assert.IsTrue(fileInformation.Data.AssociationsList.Count > 0);
            }
        }

        [TestMethod]
        public void CatchInvalidFilePaths()
        {
            foreach (KeyValuePair<string, string> invalidFilePath in FilePaths.Invalid)
            {
                Assert.ThrowsException<ArgumentException>(() => new FileInformation(invalidFilePath.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions));
            }
        }

        [TestMethod]
        public void CatchNotAssociatedFileExtension()
        {
            Assert.ThrowsException<FileNotFoundException>(() => new FileInformation(FilePaths.FileWithNoFileExtensionInfo, Vars.Dir_Associations, Vars.Dir_FileExtensions));
        }
    }
}
