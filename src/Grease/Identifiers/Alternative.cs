using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Grease.Identifiers
{
    public class Alternative
    {
        public Alternative(AlternativeType type, string text, bool isAnswer) {
            this.Type = type;
            this.Text = text;
            this.IsAnswer = isAnswer;
            this.Id = System.Guid.NewGuid().ToString();
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public AlternativeType Type { get; private set; }

        public string Text { get; set; } 

        public bool IsAnswer { get; private set; }

        public string Id { get; private set; }
    }
}