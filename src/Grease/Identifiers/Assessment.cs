using System.Collections.Generic;
using System.Linq;

namespace Grease.Identifiers
{
    public class Assessment
    {
        public Assessment(string title, IEnumerable<MultipleChoiceItem> items) {
            this.Title = title;
            this.Items = new List<MultipleChoiceItem>(items);
        }

        public string Title { get; private set; }

        public IList<MultipleChoiceItem> Items { get; private set; }

        public IEnumerable<Alternative> AllAnswers {
            get {
                return (
                    from item in this.Items
                    from ans in item.Answers
                    select ans
                );
            }
        }
    }
}