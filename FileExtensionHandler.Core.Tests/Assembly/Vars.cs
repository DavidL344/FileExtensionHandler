using FileExtensionHandler.Core.Model;
using System;
using System.IO;

namespace FileExtensionHandler.Core.Tests.Assembly
{
    internal class Vars
    {
        internal static readonly Options Options = new()
        {
            AssociationsDirectory = Path.Join(Dir_Test_Root, "Associations"),
            FileExtensionsDirectory = Path.Join(Dir_Test_Root, "File Extensions")
        };
        internal static string Dir_Test_Root => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"fexth\.Tests");
        internal static string Dir_Test_Files => Path.Join(Dir_Test_Root, "TestFiles");
    }
}
