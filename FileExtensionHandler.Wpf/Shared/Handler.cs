using FileExtensionHandler.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileExtensionHandler.Shared
{
    class Handler
    {
        private readonly string[] Arguments;
        private FileInformation FileInformation;

        internal Handler(string[] args)
        {
            this.Arguments = args;
        }

        internal void Start()
        {
            this.FileInformation = GetFileInformation();
            if (this.FileInformation == null) return;

            if (FileInformation.DefaultAssociationIndex != -1)
            {
                FileInformation.OpenWith(FileInformation.DefaultAssociationIndex);
                return;
            }

            MainWindow window = new MainWindow("AppPicker", this.FileInformation)
            {
                Title = ""
            };
            window.ShowDialog();
        }

        internal FileInformation GetFileInformation()
        {
            FileInformation fileInformation = new FileInformation(Arguments, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            if (fileInformation.Associations.Count > 0) return fileInformation;
            if (fileInformation.FileExtension != null && fileInformation.FileExtension.Node == null) return fileInformation;

            Arguments arguments = new Arguments(Arguments);
            Process.Start(arguments.ParsedNoParameters);
            return null;
        }
    }
}
