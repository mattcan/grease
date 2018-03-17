using System.Collections.Generic;
using System.Linq;

namespace Grease.Identifiers
{
    public class MultipleChoiceItem
    {
        public MultipleChoiceItem(string stem, IEnumerable<Alternative> alternatives) {
            this.Stem = stem;
            this.Type = alternatives.ElementAt(0).Type;
            this.Alternatives = alternatives.Where(a => a.Type == this.Type);
        }

        public AlternativeType Type { get; private set; }

        public string Stem { get; private set; }

        public IEnumerable<Alternative> Alternatives { get; private set; }

        public IEnumerable<Alternative> Answers {
            get {
                return this.Alternatives.Where(alt => alt.IsAnswer == true);
            }
        }

        public IEnumerable<Alternative> Distractors {
            get {
                return this.Alternatives.Where(alt => alt.IsAnswer == false);
            }
        }
    }
}