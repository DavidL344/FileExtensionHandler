using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModelArguments = FileExtensionHandler.Core.Model.Common.Arguments;
using TestArguments = FileExtensionHandler.Core.Tests.Samples.Arguments;
using Vars = FileExtensionHandler.Core.Tests.Samples.Vars;

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
            FileInformation fileInformation = new FileInformation(TestArguments.Valid["Disk"][0], Vars.Dir_Associations, Vars.Dir_FileExtensions);
            Assert.IsNotNull(fileInformation.Data.AssociationsList);
            Assert.IsTrue(fileInformation.Data.AssociationsList.Count > 0);
        }

        [TestMethod]
        public void CatchInvalidArguments()
        {
            foreach (KeyValuePair<string, string[]> invalidArgument in TestArguments.Invalid)
            {
                Assert.ThrowsException<ArgumentException>(() => new ModelArguments(invalidArgument.Value));
            }
        }

        [TestMethod]
        public void CatchNotAssociatedFileExtension()
        {
            Assert.ThrowsException<FileNotFoundException>(() => new FileInformation(TestArguments.FileWithNoFileExtensionInfo, Vars.Dir_Associations, Vars.Dir_FileExtensions));
        }
    }
}
