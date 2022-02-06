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
    public class FileExtensionsControllerTest
    {
        private readonly List<FileExtension> _fileExtensions = Helpers.SortList(FileExtensions.List);
        private readonly ObjectsComparer.Comparer<FileExtension> _comparerFileExtension = new();
        private readonly ObjectsComparer.Comparer<List<FileExtension>> _comparerList = new();
        private IEnumerable<Difference>? _differences;

        [TestMethod]
        public void Create()
        {
            FileExtension newFileExtension = FileExtensionsController.Create(Shared.SampleFileExtension.Node ?? "null");
            Assert.AreEqual(Shared.SampleFileExtension.Node, newFileExtension.Node);
        }

        [TestMethod]
        public void CopyTo()
        {
            FileExtension clonedFileExtension = FileExtensionsController.CopyTo(Shared.SampleMp3FileExtension.Node ?? "null", Shared.SampleFileExtension);
            Assert.AreEqual(Shared.SampleMp3FileExtension.Node, clonedFileExtension.Node);
        }

        [TestMethod]
        public async Task CopyToAsync()
        {
            FileExtension clonedFileExtension = await FileExtensionsController.CopyToAsync(Shared.SampleMp3FileExtension.Node ?? "null", Shared.SampleFileExtension);
            Assert.AreEqual(Shared.SampleMp3FileExtension.Node, clonedFileExtension.Node);
        }

        [TestMethod]
        public void LoadFromJson()
        {
            string fileExtensionNode = Shared.SampleMp3FileExtension.Node ?? "null";
            FileExtension fileExtension = FileExtensionsController.LoadFromJson(fileExtensionNode, Vars.Options.FileExtensionsDirectory);
            bool isEqual = _comparerFileExtension.Compare(FileExtensions.Collection[fileExtensionNode], fileExtension, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }

        [TestMethod]
        public async Task LoadFromJsonAsync()
        {
            string fileExtensionNode = Shared.SampleMp3FileExtension.Node ?? "null";
            FileExtension fileExtension = await FileExtensionsController.LoadFromJsonAsync(fileExtensionNode, Vars.Options.FileExtensionsDirectory);
            bool isEqual = _comparerFileExtension.Compare(FileExtensions.Collection[fileExtensionNode], fileExtension, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }

        [TestMethod]
        public void GetAssociations()
        {
            List<FileExtension> fileExtensionsFromDisk = Helpers.SortList(FileExtensionsController.GetFileExtensions(Vars.Options.FileExtensionsDirectory));
            bool isEqual = _comparerList.Compare(_fileExtensions, fileExtensionsFromDisk, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }

        [TestMethod]
        public async Task GetAssociationsAsync()
        {
            List<FileExtension> fileExtensionsFromDisk = Helpers.SortList(await FileExtensionsController.GetFileExtensionsAsync(Vars.Options.FileExtensionsDirectory));
            bool isEqual = _comparerList.Compare(_fileExtensions, fileExtensionsFromDisk, out _differences);
            Assert.IsTrue(isEqual, $"There were the following differences in the structs:\r\n{Helpers.StringifyIEnumerable(_differences)}");
        }
    }
}
