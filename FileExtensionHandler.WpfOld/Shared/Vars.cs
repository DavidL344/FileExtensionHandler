using FileExtensionHandler.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Shared
{
    internal class Vars
    {
        internal static string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        internal static string Protocol = "fexth";

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

        #region Assembly Attribute Accessors
        internal static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }
        internal static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        internal static string AssemblyVersionShort
        {
            get
            {
                string[] versionArray = AssemblyVersion.Split('.');
                versionArray = versionArray.Take(versionArray.Count() - 1).ToArray();
                return String.Join(".", versionArray);
            }
        }

        internal static string AssemblyLocation => Assembly.GetExecutingAssembly().Location;
        #endregion
    }
}
