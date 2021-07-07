﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExtensionHandler.Model
{
    internal class Parser
    {
        internal Handler Handler = new Handler();
        internal List<Association> AssociationsList;
        internal string FilePath;
        internal string FileExtension;

        internal Parser(string filePath)
        {
            this.FilePath = filePath;
            this.FileExtension = Path.GetExtension(filePath);

            if (Handler.Data == null) Handler.GenerateSomeAssociations();
            if (Handler.Data.ContainsKey(FileExtension))
            {
                AssociationsList = Handler.Data[FileExtension].Associations;
            }
            else
            {
                MessageBox.Show($"The there's no app associated with {FileExtension}!", "fexth", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal void RunApp(int id = -1)
        {
            if (id == -1) return;
            Association association = Handler.GetData(FileExtension, id);
            string arguments = Environment.ExpandEnvironmentVariables(association.Arguments).Replace("%1", $"\"{FilePath}\"");

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = association.Path,
                Arguments = arguments,
                WorkingDirectory = Path.GetDirectoryName(association.Path)
            };
            Process.Start(processStartInfo);
            Application.Current.Shutdown();
        }

        internal void ParseArgs(string[] args)
        {
            if (Handler.GetCount(Path.GetExtension(args[0])) == 1)
            {
                List<Association> associationsList = Handler.Data[FileExtension].Associations;

                switch (associationsList.Count)
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
    }
}
