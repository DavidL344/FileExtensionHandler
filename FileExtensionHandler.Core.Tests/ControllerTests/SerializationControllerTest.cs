using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Tests.Assembly;
using FileExtensionHandler.Core.Tests.Assembly.Samples;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectsComparer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.ControllerTests
{
    [TestClass]
    public class SerializationControllerTest
    {
        private readonly SampleModel _sampleModel = new() { Name = "Test Name", Description = "Test Description" };
        private readonly string _sampleModelSerialized = "{\r\n  \"name\": \"Test Name\",\r\n  \"description\": \"Test Description\"\r\n}";
        private readonly ObjectsComparer.Comparer<SampleModel> _comparer = new();
        private IEnumerable<Difference>? _differences;


        [TestMethod]
        public void Serialize()
        {
            string serialized = SerializationController.Serialize(_sampleModel);
            Assert.AreEqual(_sampleModelSerialized, serialized);
        }

        [TestMethod]
        public async Task SerializeAsync()
        {
            string serialized = await SerializationController.SerializeAsync(_sampleModel);
            Assert.AreEqual(_sampleModelSerialized, serialized);
        }

        [TestMethod]
        public void Deserialize()
        {
            SampleModel deserialized = SerializationController.Deserialize<SampleModel>(_sampleModelSerialized);
            bool isEqual = _comparer.Compare(_sampleModel, deserialized, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public async Task DeserializeAsync()
        {
            SampleModel deserialized = await SerializationController.DeserializeAsync<SampleModel>(_sampleModelSerialized);
            bool isEqual = _comparer.Compare(_sampleModel, deserialized, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public void Clone()
        {
            SampleModel cloned = SerializationController.Clone(_sampleModel);
            bool isEqual = _comparer.Compare(_sampleModel, cloned, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }

        [TestMethod]
        public async Task CloneAsync()
        {
            SampleModel cloned = await SerializationController.CloneAsync(_sampleModel);
            bool isEqual = _comparer.Compare(_sampleModel, cloned, out _differences);
            Assert.IsTrue(isEqual, Helpers.ParseCompareDiff(_differences));
        }
    }
}
