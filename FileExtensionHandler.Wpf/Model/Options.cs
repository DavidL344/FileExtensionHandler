using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Model
{
    
    class Options
    {
        [Option("open")]
        public IEnumerable<string> Open { get; set; }

        public string Dump()
        {
            return Open == null ? null : $"Parsed: {String.Join(" ", Open.ToArray())}";
        }
    }
}
