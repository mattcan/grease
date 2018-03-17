using Grease.Identifiers;
using System.IO;
using RazorLight;
using System;

namespace Swivel.Formatters
{
    public class HtmlFormatter : IFormatter
    {
        private string _result;

        public HtmlFormatter() { _result = ""; }

        public IFormatter Format(Assessment assessment)
        {
            string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string templates = Path.Combine($"{solutionDirectory}/src/Swivel/Formatters/Html");
            var engine = new RazorLightEngineBuilder()
                .UseFilesystemProject(templates)
                .UseMemoryCachingProvider()
                .Build();
            
            _result = engine.CompileRenderAsync("Main.cshtml", assessment).Result;

            return this;
        }

        public override string ToString() {
            return _result;
        }
    }
}