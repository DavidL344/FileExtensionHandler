using FileExtensionHandler.Core.Tests.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestArguments = FileExtensionHandler.Core.Tests.Samples.Arguments;

namespace FileExtensionHandler.Core.Tests
{
    [TestClass]
    public class ArgumentsTest
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            new Prerequisites.SaveData();
        }

        [TestMethod]
        public void GetFileInformation()
        {
            foreach (KeyValuePair<string, string[]> validArgument in TestArguments.Valid)
            {
                FileInformation fileInformation = new FileInformation(validArgument.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                Assert.AreEqual(Path.GetExtension(validArgument.Value.Last()), fileInformation.FileExtension.Node);
            }
        }

        [TestMethod]
        public void ReadFileAssociations()
        {
            foreach (KeyValuePair<string, string[]> validArgument in TestArguments.Valid)
            {
                FileInformation fileInformation = new FileInformation(validArgument.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                Assert.IsNotNull(fileInformation.Associations);
                Assert.IsTrue(fileInformation.Associations.Count > 0);
            }
        }

        [TestMethod]
        public void CatchInvalidArguments()
        {
            foreach (KeyValuePair<string, string[]> invalidArgument in TestArguments.Invalid)
            {
                Assert.ThrowsException<ArgumentException>(() => new FileInformation(invalidArgument.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions));
            }
        }

        [TestMethod]
        public void CatchNoFileExtension()
        {
            FileInformation fileInformation = new FileInformation(TestArguments.FileWithNoFileExtension, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.AreEqual(null, fileInformation.FileExtension.Node);
        }

        [TestMethod]
        public void CatchNoAssociatedFileExtension()
        {
            FileInformation fileInformation = new FileInformation(TestArguments.FileWithNoFileExtensionInfo, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.IsNull(fileInformation.FileExtension);
        }

        [TestMethod]
        public void CatchNoFileAssociations()
        {
            FileInformation fileInformation = new FileInformation(TestArguments.FileWithNoAssociations, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.AreEqual(0, fileInformation.Associations.Count);
            Assert.IsNull(fileInformation.DefaultAssociation);
            Assert.AreEqual(-1, fileInformation.DefaultAssociationIndex);
        }

        [TestMethod]
        public void DetectCallsFromAppProtocol()
        {
            foreach (KeyValuePair<string, string[]> validArgument in TestArguments.Valid)
            {
                FileInformation fileInformation = new FileInformation(validArgument.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                if (validArgument.Key.Contains("Disk") && !validArgument.Key.Contains("Fexth")) Assert.IsFalse(fileInformation.Streamed);
                if (validArgument.Key.Contains("Streamed")) Assert.IsTrue(fileInformation.Streamed);

                if (validArgument.Key.Contains("Fexth"))
                {
                    Assert.IsTrue(fileInformation.CalledFromAppProtocol);
                    Assert.IsTrue(fileInformation.ProtocolsUsed.Contains("fexth://") || fileInformation.ProtocolsUsed.Contains("fexth:"));
                }

                if (validArgument.Key.Contains("FileProtocol"))
                {
                    Assert.IsTrue(fileInformation.ProtocolsUsed.Contains("file:///") || fileInformation.ProtocolsUsed.Contains("file://"));
                    Assert.IsFalse(fileInformation.Streamed);
                }
            }
        }
    }
}
