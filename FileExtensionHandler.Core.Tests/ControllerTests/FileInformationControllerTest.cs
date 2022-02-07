using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Assembly;
using FileExtensionHandler.Core.Tests.Assembly.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectsComparer;
using System.Collections.Generic;
using System.IO;

namespace FileExtensionHandler.Core.Tests.ControllerTests
{
    [TestClass]
    public class FileInformationControllerTest
    {
        private string AppProtocolShort => $"{_appProtocol}:";
        private string AppProtocolLong => $"{_appProtocol}://";

        private readonly string _appProtocol = "fexth";
        private readonly ObjectsComparer.Comparer<FileInformation> _comparerFileInformation = new();
        private IEnumerable<Difference>? _differences;
        private readonly FileInformationController _fileInformationController = new(Vars.Options);
        private readonly string _randomFilePath = Path.Join(Vars.Dir_Test_Files, Path.ChangeExtension(Path.GetRandomFileName(), Shared.SampleMp3FileExtension.Node));

        [TestMethod]
        public void Parse()
        {
            FileInformation fileInformationExpected = Helpers.GenerateSampleFileInformation(_appProtocol, _randomFilePath);
            FileInformation fileInformationActual = _fileInformationController.Parse(_randomFilePath);

            bool isEqual = _comparerFileInformation.Compare(fileInformationExpected, fileInformationActual, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public void ParseWithAppProtocol()
        {
            string randomFilePathWithProtocol = $"{AppProtocolLong}{_randomFilePath}";
            FileInformation fileInformationExpected = Helpers.GenerateSampleFileInformation(_appProtocol, _randomFilePath, _appProtocol);
            FileInformation fileInformationActual = _fileInformationController.Parse(randomFilePathWithProtocol);

            bool isEqual = _comparerFileInformation.Compare(fileInformationExpected, fileInformationActual, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public void ParseWithAppProtocolShort()
        {
            string randomFilePathWithProtocol = $"{AppProtocolShort}{_randomFilePath}";
            FileInformation fileInformationExpected = Helpers.GenerateSampleFileInformation(_appProtocol, _randomFilePath, _appProtocol);
            FileInformation fileInformationActual = _fileInformationController.Parse(randomFilePathWithProtocol);

            bool isEqual = _comparerFileInformation.Compare(fileInformationExpected, fileInformationActual, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public void ParseWithMultipleProtocols()
        {
            string randomFilePathWithMultipleProtocols = $"{AppProtocolLong}file:///{_randomFilePath}";
            FileInformation fileInformationExpected = Helpers.GenerateSampleFileInformation(_appProtocol, _randomFilePath, _appProtocol, "file");
            FileInformation fileInformationActual = _fileInformationController.Parse(randomFilePathWithMultipleProtocols);

            bool isEqual = _comparerFileInformation.Compare(fileInformationExpected, fileInformationActual, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public void ParseStreamed()
        {
            string randomStreamedFilePath = $"https://{_randomFilePath}";

            FileInformation fileInformationExpected = Helpers.GenerateSampleFileInformation(_appProtocol, randomStreamedFilePath, "https");
            FileInformation fileInformationActual = _fileInformationController.Parse(randomStreamedFilePath);

            bool isEqual = _comparerFileInformation.Compare(fileInformationExpected, fileInformationActual, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public void ParseStreamedWithAppProtocol()
        {
            string randomStreamedFilePath = $"https://{_randomFilePath}";
            string randomStreamedFilePathWithProtocol = $"{AppProtocolLong}{randomStreamedFilePath}";

            FileInformation fileInformationExpected = Helpers.GenerateSampleFileInformation(_appProtocol, randomStreamedFilePath, _appProtocol, "https");
            FileInformation fileInformationActual = _fileInformationController.Parse(randomStreamedFilePathWithProtocol);

            bool isEqual = _comparerFileInformation.Compare(fileInformationExpected, fileInformationActual, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }
    }
}
