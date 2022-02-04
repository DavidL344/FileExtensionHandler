using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Controller
{
    public class ProtocolController
    {
        private static bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        private static string GetOpenPath(string protocol) => $@"{protocol}\shell\open\command";
        private static string ParseCommand(string executable, string arguments, bool cmdFormat = false)
            => cmdFormat ? $"\\\"{executable}\\\" {arguments}" : $"\"{executable}\" {arguments}";

        /// <summary>
        /// Registers a file protocol.
        /// </summary>
        /// <param name="protocol">A protocol to be registered with the application.</param>
        /// <param name="executable">The application path to be registered.</param>
        /// <param name="arguments">Arguments to be passed to the application.</param>
        public static void Register(string protocol, string executable, string arguments = "%1", bool forceOverwrite = false)
        {
            if (IsRegistered(protocol, executable, arguments) && !forceOverwrite) return;

            if (!IsAdministrator)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c reg add \"HKCR\\{protocol}\" /v \"URL Protocol\" /t REG_SZ /f" +
                    $"&&reg add \"HKCR\\{GetOpenPath(protocol)}\" /ve /d \"{ParseCommand(executable, arguments, true)}\" /f",
                    Verb = "runas",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return;
            }

            using (RegistryKey rootSubKey = Registry.ClassesRoot.CreateSubKey(protocol))
                rootSubKey.SetValue("URL Protocol", "");

            using (RegistryKey appCommand = Registry.ClassesRoot.CreateSubKey(GetOpenPath(protocol)))
                appCommand.SetValue("", $"\"{executable}\" {arguments}");
        }

        /// <summary>
        /// Unregisteres the file protocol.
        /// </summary>
        /// <param name="protocol">A protocol to be unregistered.</param>
        public static void Unregister(string protocol)
        {
            if (!EntryExists(protocol)) return;

            if (!IsAdministrator)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c reg delete \"HKCR\\{protocol}\" /f",
                    Verb = "runas",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return;
            }
            Registry.ClassesRoot.DeleteSubKeyTree(protocol);
        }

        /// <summary>
        /// Checks if a protocol entry exists in the registry.
        /// </summary>
        /// <param name="protocol">A protocol to check.</param>
        /// <returns>If the protocol has a key in HKCR</returns>
        public static bool EntryExists(string protocol)
        {
            try
            {
                using (RegistryKey rootSubKey = Registry.ClassesRoot.OpenSubKey(protocol))
                    return rootSubKey != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if an application is correctly registered with the protocol.
        /// </summary>
        /// <param name="protocol">A protocol to check.</param>
        /// <param name="executable">A specified application's executable path.</param>
        /// <param name="arguments">A specified application's arguments passed.</param>
        /// <returns>If the application is registered with the specified protocol.</returns>
        public static bool IsRegistered(string protocol, string executable, string arguments = "%1")
        {
            try
            {
                using (RegistryKey rootSubKey = Registry.ClassesRoot.OpenSubKey(protocol))
                {
                    if (rootSubKey == null) return false;
                    if ((string)rootSubKey.GetValue("URL Protocol") != "") return false;
                }

                using (RegistryKey appCommand = Registry.ClassesRoot.OpenSubKey(GetOpenPath(protocol)))
                {
                    if (appCommand == null) return false;

                    string regCommand = (string)appCommand.GetValue("");
                    string parsedCommand = ParseCommand(executable, arguments);
                    if (regCommand != parsedCommand) return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
