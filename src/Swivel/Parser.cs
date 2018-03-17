using System;
using System.IO;
using Grease;
using GreaseModels = Grease.Identifiers;
using Swivel.Formatters;

namespace Swivel
{
    public class Parser
    {
        private const int FILE_DOES_NOT_EXIST = 2;
        private const int INVALID_FORMAT_OPTION = 3;
        private const int UNABLE_TO_DISPLAY_ASSESSMENT = 4;
        private const int SUCCESS = 0;

        private Options _options;

        public Parser(Options options) {
            _options = options;
        }

        public int Parse() {
            if (!File.Exists(_options.AssessmentFileName)) { return FILE_DOES_NOT_EXIST; }
            string assessmentRaw = this.readFile(_options.AssessmentFileName);

            GreaseModels.Assessment assessment = Grammar.ParseAssessment(assessmentRaw);

            IFormatter formatter = this.formatterFactory(_options.Format);
            if (formatter is null) { return INVALID_FORMAT_OPTION; }

            bool displayResult = this.outputResults(
                _options.OutputFileName,
                formatter.Format(assessment).ToString()
            );
            if (!displayResult) { return UNABLE_TO_DISPLAY_ASSESSMENT; }

            return SUCCESS;
        }

        private bool outputResults(string outputFileName, string formattedAssesment) {
            if (outputFileName == string.Empty) {
                Console.Write(formattedAssesment);
                return true;
            }

            return this.writeFile(outputFileName, formattedAssesment);
        }

        private IFormatter formatterFactory(string format) {
            if (_options.Format.ToLower() == "json") {
                return new JsonFormatter();
            } else if (_options.Format.ToLower() == "html") {
                return new HtmlFormatter();
            } else {
                return null;
            }
        }

        private string readFile(string inputFileName) {
            string assessmentRaw = "";
            using (StreamReader rdr = File.OpenText(_options.AssessmentFileName)) {
                assessmentRaw = rdr.ReadToEnd();
            }
            return assessmentRaw;
        }

        private bool writeFile(string outputFileName, string contents) {
            try {
                using(StreamWriter writer = File.CreateText(outputFileName)) {
                    writer.Write(contents);
                }
                return true;
            } catch (Exception e) {
                return false;
            }
        }
    }
}