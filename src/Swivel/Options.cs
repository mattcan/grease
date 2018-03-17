using CommandLine;
using CommandLine.Text;

namespace Swivel
{
    public class Options
    {
        [Option('f', "format",
            Default = "json",
            HelpText = "Valid values are JSON or HTML",
            Required = false)]
        public string Format { get; set; }

        [Value(1, MetaName = "input file",
            HelpText = "Assessment file to be processed.",
            Required = true)]
        public string AssessmentFileName { get; set; }

        [Value(2, MetaName = "output file",
            HelpText = "Output file name.",
            Default = "",
            Required = false)]
        public string OutputFileName { get; set; }
    }
}