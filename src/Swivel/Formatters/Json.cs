using Grease.Identifiers;
using Newtonsoft.Json;

namespace Swivel.Formatters
{
    public class JsonFormatter : IFormatter
    {
        private string _serialized;

        public IFormatter Format(Assessment assessment)
        {
            _serialized = JsonConvert.SerializeObject(assessment);

            return this;
        }

        public override string ToString() {
            return _serialized;
        }
    }
}