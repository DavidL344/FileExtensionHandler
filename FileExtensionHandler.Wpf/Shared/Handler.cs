using FileExtensionHandler.Core;
using System;
using System.Collections.Generic;
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

            int defaultAssociationIndex = FileInformation.Data.DefaultAssociationIndex;
            if (defaultAssociationIndex != -1)
            {
                FileInformation.OpenWith(defaultAssociationIndex);
                return;
            }

            AppPicker window = new AppPicker(this.FileInformation)
            {
                Title = ""
            };
            window.ShowDialog();
        }

        internal FileInformation GetFileInformation()
        {
            try
            {
                return new FileInformation(Arguments, Vars.Dir_Associations, Vars.Dir_FileExtensions);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
