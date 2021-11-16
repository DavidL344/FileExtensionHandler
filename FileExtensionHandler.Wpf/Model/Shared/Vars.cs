using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.Shared
{
    internal class Vars
    {
        internal static string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        internal static string DefaultSaveLocation
        {
            get
            {
                string Save_LocalAppData = LocalAppData + @"\fexth";
                string Save_WorkingDirectory = WorkingDirectory + @"\user";

                // Prioritize the working directory for more flexibility (otherwise default to LocalAppData)
                if (Directory.Exists(Save_WorkingDirectory)) return Save_WorkingDirectory;
                return Save_LocalAppData;
            }
        }

        internal static string Dir_Associations => DefaultSaveLocation + @"\Associations";
        internal static string Dir_FileExtensions => DefaultSaveLocation + @"\File Extensions";
    }
}
