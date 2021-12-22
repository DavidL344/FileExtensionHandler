using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core
{
    public class AppRegistry
    {
        private readonly string Protocol;
        private string ProtocolFullPath => $@"{Protocol}\shell\open\command";
        private readonly string AssemblyLocation;
        private bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        public bool IsProtocolRegistered
        {
            get
            {
                try
                {
                    bool isRegistered = true;

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(Protocol))
                    {
                        if (key == null)
                        {
                            isRegistered = false;
                        }
                        else
                        {
                            if ((string)key.GetValue("URL Protocol") != "") isRegistered = false;
                        }
                    }

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(ProtocolFullPath))
                    {
                        if (key == null)
                        {
                            isRegistered = false;
                        }
                        else
                        {
                            string regPath = (string)key.GetValue("");
                            string[] delimiter = new string[] { " %1" };
                            string[] quotesDelimiter = new string[] { "\"" };
                            regPath = regPath.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[0].Split(quotesDelimiter, StringSplitOptions.RemoveEmptyEntries)[0];
                            if (regPath != AssemblyLocation) isRegistered = false;
                        }
                    }
                    return isRegistered;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public AppRegistry(string assemblyLocation, string protocolKeyName = "fexth")
        {
            this.AssemblyLocation = assemblyLocation;
            this.Protocol = protocolKeyName;
        }

        public void RegisterProtocol()
        {
            if (IsProtocolRegistered) return;
            string regCommand = $"\"{AssemblyLocation}\" %1";
            string regCommandCmd = $"\\\"{AssemblyLocation}\\\" %1";

            if (!IsAdministrator)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c reg add \"HKCR\\{Protocol}\" /v \"URL Protocol\" /t REG_SZ /f" +
                    $"&&reg add \"HKCR\\{ProtocolFullPath}\" /ve /d \"{regCommandCmd}\" /f",
                    Verb = "runas",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return;
            }
            RegistryKey fexth_root = Registry.ClassesRoot.CreateSubKey(Protocol);
            fexth_root.SetValue("URL Protocol", "");

            RegistryKey fexth_command = Registry.ClassesRoot.CreateSubKey(ProtocolFullPath);
            fexth_command.SetValue("", regCommand);
        }

        public void UnregisterProtocol()
        {
            if (!IsProtocolRegistered) return;
            if (!IsAdministrator)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c reg delete \"HKCR\\{Protocol}\" /f",
                    Verb = "runas",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return;
            }
            Registry.ClassesRoot.DeleteSubKeyTree(Protocol);
        }
    }
}
