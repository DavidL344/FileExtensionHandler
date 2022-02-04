using FileExtensionHandler.Core.Model;

namespace FileExtensionHandler.Core.Tests.Assembly.Samples
{
    internal class Shared
    {
        internal static readonly string[] AudioAssociations = new string[]
        {
            "fexth.foobar2000.play",
            "fexth.foobar2000.add",
            "fexth.wmplayer.play"
        };

        internal static readonly string[] BrowserAssociations = new string[]
        {
            "fexth.msedge.open",
            "fexth.msedge.open-private"
        };

        internal static readonly Association SampleAssociation = new() { Node = ".sample" };
        internal static readonly Association SampleMp3Association = new() { Node = ".mp3" };
        internal static readonly FileExtension SampleFileExtension = new() { Node = "fexth.sample.open" };
        internal static readonly FileExtension SampleMp3FileExtension = FileExtensions.Collection[".mp3"];
    }
}
