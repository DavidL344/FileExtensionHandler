using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExtensionHandler.Core.Tests
{
    [TestClass]
    public class ControllerTest
    {
        public readonly string FileExtension = ".mp3";
        public readonly string Association = "fexth.wmplayer.play";

        [TestMethod]
        public void LoadFileExtensionInformation()
        {
            FileExtension fileExtensionData = FileExtensionsController.LoadFromJson(FileExtension, Vars.Dir_FileExtensions);
            Assert.AreEqual(FileExtension, fileExtensionData.Node);
        }

        [TestMethod]
        public void LoadAssociation()
        {
            Association associationData = AssociationsController.LoadFromJson(Association, Vars.Dir_Associations);
            Assert.AreEqual(Association, associationData.Node);
        }

        [TestMethod]
        public void CreateAssociationsList()
        {
            FileExtensions fileExtensions = new FileExtensions();
            FileExtension fileExtensionData = FileExtensionsController.LoadFromJson(FileExtension, Vars.Dir_FileExtensions);
            List<Association> associationsList = AssociationsController.MakeList(fileExtensionData, Vars.Dir_Associations);

            List<string> associationsStringList = new List<string>();
            foreach (Association entry in associationsList) associationsStringList.Add(entry.Node);
            string[] arrayToCompare = associationsStringList.ToArray();
            CollectionAssert.AreEqual(fileExtensions.Collection[FileExtension].Associations, arrayToCompare);
        }
    }
}
