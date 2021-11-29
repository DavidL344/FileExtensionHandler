using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Common
{
    class OpenedKey
    {
        RegistryKey Key;
        internal OpenedKey(string regPath)
        {
            string[] regPathArray = regPath.Split('\\');
            string parsedPath = regPath.Substring(regPathArray[0].Length + 1);
            switch (regPathArray[0].ToUpper())
            {
                case "HKLM":
                case "HKEY_LOCAL_MACHINE":
                    Key = Registry.LocalMachine.OpenSubKey(parsedPath);
                    break;
                case "HKCU":
                case "HKEY_CURRENT_USER":
                    Key = Registry.CurrentUser.OpenSubKey(parsedPath);
                    break;
                case "HKCR":
                case "HKEY_CLASSES_ROOT":
                    Key = Registry.ClassesRoot.OpenSubKey(parsedPath);
                    break;
                default:
                    break;
            }
        }

        internal object GetValue(string name) => Key.GetValue(name);
        internal string GetDefaultValue() => Key.GetValue(String.Empty) as string;
    }
}
