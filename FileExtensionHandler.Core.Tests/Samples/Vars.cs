using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.Samples
{
    class Vars
    {
        internal static string Dir_Test_Root => Environment.ExpandEnvironmentVariables(@"%localappdata%\fexth\.Tests");
        internal static string Dir_Associations => Dir_Test_Root + @"\Associations";
        internal static string Dir_FileExtensions => Dir_Test_Root + @"\File Extensions";
        internal static string Dir_Files => Dir_Test_Root + @"\TestFiles";
    }
}
