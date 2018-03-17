using CommandLine;
using System;

namespace Swivel
{
    class Program
    {
        static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (Options opts) => RunParserAndFormat(opts),
                    errs => 1);
        }

        static int RunParserAndFormat(Options options)
        {
            var p = new Swivel.Parser(options);
            return p.Parse();
        }
    }
}
