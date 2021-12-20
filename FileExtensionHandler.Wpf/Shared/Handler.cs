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
            this.FileInformation = new FileInformation(Arguments, Vars.Dir_Associations, Vars.Dir_FileExtensions);

            if (!FileInformation.CalledFromAppProtocol && FileInformation.DefaultAssociationIndex != -1)
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
    }
}
