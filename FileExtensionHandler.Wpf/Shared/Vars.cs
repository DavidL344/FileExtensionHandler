using FileExtensionHandler.Core.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace FileExtensionHandler.Wpf.Shared
{
    internal class Vars
    {
        internal static string LocalAppData => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string WorkingDirectory => Path.GetDirectoryName((Assembly.GetEntryAssembly() ?? (Assembly.GetCallingAssembly())).Location) ?? LocalAppData + @"\Temp";
        internal static string Protocol => "fexth";

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

        internal static readonly Options DefaultOptions = new()
        {
            AssociationsDirectory = Dir_Associations,
            FileExtensionsDirectory = Dir_FileExtensions
        };

        internal static string Dir_Associations => DefaultSaveLocation + @"\Associations";
        internal static string Dir_FileExtensions => DefaultSaveLocation + @"\File Extensions";

        #region Assembly Attribute Accessors
        private static readonly string _assemblyLocation = Assembly.GetExecutingAssembly().Location;
        private static readonly FileVersionInfo _fileVersionInfo = FileVersionInfo.GetVersionInfo(_assemblyLocation);
        internal static string ProductName => _fileVersionInfo.ProductName ?? "<Product Name>";
        internal static string ProductVersion => _fileVersionInfo.ProductVersion ?? "<Product Version>";
        #endregion
    }
}
