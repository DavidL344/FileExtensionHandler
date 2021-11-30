using System;

namespace FileExtensionHandler.Core.Common
{
    public class AssociationsNotFoundException : Exception
    {
        public AssociationsNotFoundException()
        {

        }

        public AssociationsNotFoundException(string message) : base(message)
        {

        }

        public AssociationsNotFoundException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
