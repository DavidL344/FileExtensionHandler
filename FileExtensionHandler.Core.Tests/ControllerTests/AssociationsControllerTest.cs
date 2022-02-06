using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Assembly;
using FileExtensionHandler.Core.Tests.Assembly.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectsComparer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.ControllerTests
{
    [TestClass]
    public class AssociationsControllerTest
    {
        private readonly List<Association> _associations = Helpers.SortList(Associations.List);
        private readonly ObjectsComparer.Comparer<Association> _comparerAssociation = new();
        private readonly ObjectsComparer.Comparer<List<Association>> _comparerList = new();
        private readonly List<FileExtension> _fileExtensions = Helpers.SortList(FileExtensions.List);
        private IEnumerable<Difference>? _differences;

        [TestMethod]
        public void Create()
        {
            Association newAssociation = AssociationsController.Create(Shared.SampleAssociation.Node ?? "null");
            Assert.AreEqual(Shared.SampleAssociation.Node, newAssociation.Node);
        }

        [TestMethod]
        public void CopyTo()
        {
            Association clonedAssociation = AssociationsController.CopyTo(Shared.SampleMp3Association.Node ?? "null", Shared.SampleAssociation);
            Assert.AreEqual(Shared.SampleMp3Association.Node, clonedAssociation.Node);
        }

        [TestMethod]
        public async Task CopyToAsync()
        {
            Association clonedAssociation = await AssociationsController.CopyToAsync(Shared.SampleMp3Association.Node ?? "null", Shared.SampleAssociation);
            Assert.AreEqual(Shared.SampleMp3Association.Node, clonedAssociation.Node);
        }

        [TestMethod]
        public void GetAssociations()
        {
            List<Association> associationsFromDisk = Helpers.SortList(AssociationsController.GetAssociations(Vars.Options.AssociationsDirectory));
            bool isEqual = _comparerList.Compare(_associations, associationsFromDisk, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }

        [TestMethod]
        public async Task GetAssociationsAsync()
        {
            List<Association> associationsFromDisk = Helpers.SortList(await AssociationsController.GetAssociationsAsync(Vars.Options.AssociationsDirectory));
            bool isEqual = _comparerList.Compare(_associations, associationsFromDisk, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }

        [TestMethod]
        public void GetAssociationsForFileExtension()
        {
            List<Association> associations = AssociationsController.GetAssociations(Shared.SampleMp3FileExtension, Vars.Options.AssociationsDirectory);

            for (int i = 0; i < associations.Count; i++)
            {
                Association association = associations[i];
                Assert.IsNotNull(association);
                Assert.AreEqual(Shared.AudioAssociations[i], association.Node, $"'{Shared.AudioAssociations[i]}' doesn't match '{association.Node}'!");
            }
        }

        [TestMethod]
        public async Task GetAssociationsForFileExtensionAsync()
        {
            List<Association> associations = await AssociationsController.GetAssociationsAsync(Shared.SampleMp3FileExtension, Vars.Options.AssociationsDirectory);

            for (int i = 0; i < associations.Count; i++)
            {
                Association association = associations[i];
                Assert.IsNotNull(association);
                Assert.AreEqual(Shared.AudioAssociations[i], association.Node, $"'{Shared.AudioAssociations[i]}' doesn't match '{association.Node}'!");
            }
        }

        [TestMethod]
        public void LoadFromJson()
        {
            //string jsonPath = Path.Join(Vars.Options.AssociationsDirectory, $"{node}.json"); <-- for SerializationControllerTest.cs:LoadFromJson()
            string associationNode = Shared.SampleMp3Association.Node ?? "null";
            Association association = AssociationsController.LoadFromJson(associationNode, Vars.Options.AssociationsDirectory);
            bool isEqual = _comparerAssociation.Compare(Associations.Collection[associationNode], association, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }

        [TestMethod]
        public async Task LoadFromJsonAsync()
        {
            string associationNode = Shared.SampleMp3Association.Node ?? "null";
            Association association = await AssociationsController.LoadFromJsonAsync(associationNode, Vars.Options.AssociationsDirectory);
            bool isEqual = _comparerAssociation.Compare(Associations.Collection[associationNode], association, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }
    }
}
