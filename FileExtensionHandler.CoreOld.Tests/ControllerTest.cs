using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
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

        [TestMethod]
        public void HandleNoFileExtension()
        {
            FileExtension noFileExtension = FileExtensionsController.LoadFromJson("", Vars.Dir_FileExtensions);
            FileExtension expectedData = new FileExtension
            {
                Node = null,
                Name = "",
                Icon = null,
                IconIndex = 0,
                Associations = new string[] { },
                DefaultAssociation = null
            };

            Assert.AreEqual(expectedData.Node, noFileExtension.Node);
            Assert.AreEqual(expectedData.Name, noFileExtension.Name);
            Assert.AreEqual(expectedData.Icon, noFileExtension.Icon);
            Assert.AreEqual(expectedData.IconIndex, noFileExtension.IconIndex);
            CollectionAssert.AreEqual(expectedData.Associations, noFileExtension.Associations);
            Assert.AreEqual(expectedData.DefaultAssociation, noFileExtension.DefaultAssociation);
        }

        [TestMethod]
        public void GetNoFileExtensionAssociations()
        {
            List<string> allAssociationsList = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(Vars.Dir_Associations);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                allAssociationsList.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
            string[] allAssociationsArray = allAssociationsList.ToArray();

            FileExtension noFileExtension = FileExtensionsController.LoadFromJson("", Vars.Dir_FileExtensions);
            List<Association> parsedAssociationsList = AssociationsController.MakeList(noFileExtension, Vars.Dir_Associations);

            List<string> parsedAssociationsNodeList = new List<string>();
            foreach (Association entry in parsedAssociationsList) parsedAssociationsNodeList.Add(entry.Node);
            string[] parsedAssociationsNodeArray = parsedAssociationsNodeList.ToArray();
            CollectionAssert.AreEqual(allAssociationsArray, parsedAssociationsNodeArray);
        }
    }
}
