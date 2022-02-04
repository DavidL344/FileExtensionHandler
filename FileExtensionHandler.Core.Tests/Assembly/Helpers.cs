using FileExtensionHandler.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Core.Tests.Assembly
{
    internal class Helpers
    {
        internal static string StringifyIEnumerable<T>(IEnumerable<T> ts)
        {
            return string.Join(Environment.NewLine, ts);
        }

        internal static List<Association> SortList(List<Association> associations)
        {
            return associations.OrderBy(o => o.Node).ToList();
        }

        internal static List<FileExtension> SortList(List<FileExtension> fileExtensions)
        {
            return fileExtensions.OrderBy(o => o.Node ?? "null").ToList();
        }
    }
}
