using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Common
{
    public class DataDirectoryNotFoundException : Exception
    {
        public DataDirectoryNotFoundException()
        {

        }

        public DataDirectoryNotFoundException(string message) : base(message)
        {

        }

        public DataDirectoryNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
