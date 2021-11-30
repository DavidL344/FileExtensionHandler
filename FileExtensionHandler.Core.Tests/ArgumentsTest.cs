using FileExtensionHandler.Core.Common;
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
                Assert.AreEqual(Path.GetExtension(Path.GetExtension(validArgument.Value.Last())), fileInformation.Data.Extension);
            }
        }

        [TestMethod]
        public void ReadFileAssociations()
        {
            foreach (KeyValuePair<string, string[]> validArgument in TestArguments.Valid)
            {
                FileInformation fileInformation = new FileInformation(validArgument.Value, Vars.Dir_Associations, Vars.Dir_FileExtensions);
                Assert.IsNotNull(fileInformation.Data.AssociationsList);
                Assert.IsTrue(fileInformation.Data.AssociationsList.Count > 0);
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
        public void CatchNotAssociatedFileExtension()
        {
            Assert.ThrowsException<AssociationsNotFoundException>(() => new FileInformation(TestArguments.FileWithNoFileExtensionInfo, Vars.Dir_Associations, Vars.Dir_FileExtensions));
        }
    }
}
