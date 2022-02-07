using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Core.Tests.Assembly.Samples;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileExtensionHandler.Core.Tests.Assembly
{
    internal class Helpers
    {
        private static readonly FileExtension _fileExtension = FileExtensionsController.LoadFromJson(Shared.SampleMp3FileExtension.Node ?? "null", Vars.Options.FileExtensionsDirectory);
        private static readonly List<Association> _associations = AssociationsController.GetAssociations(_fileExtension, Vars.Options.AssociationsDirectory);

        internal static string StringifyIEnumerable<T>(IEnumerable<T> ts)
        {
            return string.Join(Environment.NewLine, ts);
        }

        internal static string ParseCompareDiff<T>(IEnumerable<T> ts)
        {
            return $"There were the following differences in the structs:\r\n{StringifyIEnumerable(ts)}";
        }

        internal static List<Association> SortList(List<Association> associations)
        {
            return associations.OrderBy(o => o.Node).ToList();
        }

        internal static List<FileExtension> SortList(List<FileExtension> fileExtensions)
        {
            return fileExtensions.OrderBy(o => o.Node ?? "null").ToList();
        }

        internal static FileInformation GenerateSampleFileInformation(string appProtocol, string location, params string[] protocolsUsed)
        {
            return new FileInformation()
            {
                AppProtocol = appProtocol,
                Associations = _associations,
                FileExtension = _fileExtension,
                Location = location,
                Protocols = protocolsUsed
            };
        }
    }
}
