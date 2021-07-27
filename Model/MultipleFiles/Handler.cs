using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExtensionHandler.Model.MultipleFiles
{
    internal class Handler
    {
        internal FileExtensionInfo FileExtensionEntry;
        internal List<AssociationInfo> Associations = new List<AssociationInfo>();
        internal readonly string FilePath;
        internal readonly string FileName;
        internal readonly string FileType;
        internal readonly string FileExtension;

        public Handler(string filePath)
        {
            string[] appDirs = new string[] { Vars.DefaultSaveLocation, Vars.Dir_Associations, Vars.Dir_FileExtensions };
            bool generateAssociations = false;
            
            foreach (string appDir in appDirs)
                if (!Directory.Exists(appDir))
                {
                    Directory.CreateDirectory(appDir);
                    generateAssociations = true;
                }
            if (generateAssociations) GenerateAssociations();

            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
            this.FileExtension = Path.GetExtension(filePath);
            Load();
            this.FileType = !string.IsNullOrWhiteSpace(this.FileExtensionEntry.Data.Name) ? this.FileExtensionEntry.Data.Name : null;
        }

        internal List<Association> GetAssociations()
        {
            List<Association> list = new List<Association>();
            list.Clear();
            foreach (AssociationInfo associationInfo in Associations)
                list.Add(associationInfo.Data);
            return list;
        }

        internal Association GetData(int selectedIndex) => Associations[selectedIndex].Data;
        internal int GetCount() => Associations.Count;

        internal void GenerateAssociations()
        {
            Associations.Add(new AssociationInfo("fexth.foobar2000.play")
            {
                Data = new Association
                {
                    Name = "Foobar2000 (Play)",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                    Arguments = "%1"
                }
            });

            Associations.Add(new AssociationInfo("fexth.foobar2000.add")
            {
                Data = new Association
                {
                    Name = "Foobar2000 (Add to queue)",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\foobar2000\foobar2000.exe",
                    Arguments = "/add %1"
                }
            });

            Associations.Add(new AssociationInfo("fexth.wmplayer.play")
            {
                Data = new Association
                {
                    Name = "Windows Media Player",
                    Icon = null,
                    IconIndex = 0,
                    Command = @"%ProgramFiles(x86)%\Windows Media Player\wmplayer.exe",
                    Arguments = "/prefetch:6 /Open %1"
                }
            });

            FileExtensionEntry = new FileExtensionInfo(".mp3")
            {
                Data = new FileExtension
                {
                    Name = "MP3 Format Sound",
                    Icon = @"%SystemRoot%\system32\wmploc.dll",
                    IconIndex = -732,
                    Associations = new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    }
                }
            };
            Save(); // Present multiple times to save the file extension information

            FileExtensionEntry = new FileExtensionInfo(".flac")
            {
                Data = new FileExtension
                {
                    Name = "FLAC Audio",
                    Icon = @"%SystemRoot%\system32\wmploc.dll",
                    IconIndex = -738,
                    Associations = new string[]
                    {
                        "fexth.foobar2000.play",
                        "fexth.foobar2000.add",
                        "fexth.wmplayer.play"
                    }
                }
            };
            Save();
        }

        internal void Load()
        {
            FileExtensionEntry = new FileExtensionInfo(FileExtension);

            if (FileExtensionEntry.Data == null)
                throw new FileFormatException($"The there's no app associated with {FileExtension}!");

            foreach (string associationInfo in FileExtensionEntry.Data.Associations)
                Associations.Add(new AssociationInfo(associationInfo));
        }

        internal void Save()
        {
            foreach (AssociationInfo associationInfo in Associations)
                associationInfo.Save();
            FileExtensionEntry.Save();
        }

        internal void ParseFile()
        {
            if (this.GetCount() == 1)
            {
                switch (this.Associations.Count)
                {
                    case 0:
                        break;
                    case 1:
                        RunApp(0);
                        break;
                    default:
                        break;
                }
            }
        }

        internal void RunApp(int id = -1)
        {
            if (id == -1) return;
            Association association = this.GetData(id);
            string fileName = Environment.ExpandEnvironmentVariables(association.Command);
            string arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{FilePath}\"");

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = Path.GetDirectoryName(fileName)
            };
            Process.Start(processStartInfo);
            Application.Current.Shutdown();
        }
    }
}
