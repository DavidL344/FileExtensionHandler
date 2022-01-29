using FileExtensionHandler.Core.Tests.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                Assert.AreEqual(Path.GetExtension(validFilePath.Value), fileInformation.FileExtension.Node);
            }
        }

        [TestMethod]
        public void ReadFileAssociations()
        {
            foreach (KeyValuePair<string, string> validFilePath in FilePaths.Valid)
            {
                FileInformation fileInformation = new FileInformation(validFilePath.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                Assert.IsNotNull(fileInformation.Associations);
                Assert.IsTrue(fileInformation.Associations.Count > 0);
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
        public void CatchNoFileExtension()
        {
            FileInformation fileInformation = new FileInformation(FilePaths.FileWithNoFileExtension, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.AreEqual(null, fileInformation.FileExtension.Node);
        }

        [TestMethod]
        public void CatchNoAssociatedFileExtension()
        {
            FileInformation fileInformation = new FileInformation(FilePaths.FileWithNoFileExtensionInfo, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.IsNull(fileInformation.FileExtension);
        }

        [TestMethod]
        public void CatchNoFileAssociations()
        {
            FileInformation fileInformation = new FileInformation(FilePaths.FileWithNoAssociations, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.AreEqual(0, fileInformation.Associations.Count);
            Assert.IsNull(fileInformation.DefaultAssociation);
            Assert.AreEqual(-1, fileInformation.DefaultAssociationIndex);
        }

        [TestMethod]
        public void DetectCallsFromAppProtocol()
        {
            foreach (KeyValuePair<string, string> validFilePath in FilePaths.Valid)
            {
                FileInformation fileInformation = new FileInformation(validFilePath.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                if (validFilePath.Key.Contains("Disk") && !validFilePath.Key.Contains("Fexth")) Assert.IsFalse(fileInformation.Streamed);
                if (validFilePath.Key.Contains("Streamed")) Assert.IsTrue(fileInformation.Streamed);

                if (validFilePath.Key.Contains("Fexth"))
                {
                    Assert.IsTrue(fileInformation.CalledFromAppProtocol);
                    Assert.IsTrue(fileInformation.ProtocolsUsed.Contains("fexth://") || fileInformation.ProtocolsUsed.Contains("fexth:"));
                }

                if (validFilePath.Key.Contains("FileProtocol"))
                {
                    Assert.IsTrue(fileInformation.ProtocolsUsed.Contains("file:///"));
                    Assert.IsFalse(fileInformation.Streamed);
                }
            }
        }
    }
}
