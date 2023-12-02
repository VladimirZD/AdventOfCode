using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal class CmdLineOptions
    {
        [Option('y', "year", Required = false, HelpText = "Year for which to run tasks")]
        public int Year { get; set; }
        [Option('d', "day", Required = false, HelpText = "Day for which to run tasks")]
        public int Day { get; set; }
        [Option('h', "help", Required = false, HelpText = "Show Help")]
        public bool ShowHelp { get; set; }
    }
}
