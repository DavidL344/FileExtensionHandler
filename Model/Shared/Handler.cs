using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model.Shared
{
    internal class Handler
    {
        // Might use this class for modifying file associations through the UI instead
        OpenedFile OpenedFile;
        internal Handler(string filePath)
        {
            // The file opening could be done directly from App.xaml.cs when an argument is passed to the app
            OpenedFile = new OpenedFile(filePath);
        }
    }
}
