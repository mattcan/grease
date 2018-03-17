using System;
using Sprache;
using System.Linq;
using System.Collections.Generic;

namespace Grease
{
    public enum AlternativeType {
        SingleSelect,
        MultiSelect
    }

    public class Grammar
    {
        public static Identifiers.Assessment ParseAssessment(string input) {
            return Grammar.Assesment.Parse(input);
        }

         public static readonly Parser<string> TextOnLine = (
             from leading in Parse.WhiteSpace.Except(Parse.LineEnd).Optional()
             from item in Parse.AnyChar.Except(Parse.LineEnd).AtLeastOnce().Text()
             select item);

        public static readonly Parser<string> Heading = (
            from h in Parse.Char('#').Token()
            from text in TextOnLine
            select text).Token();

        public static readonly Parser<string> StemText = (
            from q in Parse.Digit.Then(_ => Parse.Char('.')).Token()
            from text in TextOnLine
            select text).Token();

        private static readonly Parser<string> AlternativeStart = (
            from a in Parse.Char('*').Then(_ => Parse.Char(' ')).Token()
            select $"{a}").Token();

        private static readonly Parser<bool> AlternativeIsAnswer =
            Parse.Char(' ' ).Return(false)
            .Or(Parse.Char('x').Return(true));
        
        private static readonly Parser<Identifiers.Alternative> SingleSelectAlternative = (
            from start in Parse.Char('(')
            from isAnswer in AlternativeIsAnswer
            from end in Parse.Char(')')
            select new Identifiers.Alternative(
                AlternativeType.SingleSelect,
                "",
                isAnswer
            ));

        private static readonly Parser<Identifiers.Alternative> MultiSelectAlternative = (
            from start in Parse.Char('[')
            from isAnswer in AlternativeIsAnswer
            from end in Parse.Char(']')
            select new Identifiers.Alternative(
                AlternativeType.MultiSelect,
                "",
                isAnswer
            ));

        public static readonly Parser<Identifiers.Alternative> Alternative = (
            from start in AlternativeStart
            from alternative in SingleSelectAlternative.Or(MultiSelectAlternative)
            from text in TextOnLine.Token()
            let x = alternative.Text = text // TODO something nicer?
            select alternative);

        public static readonly Parser<Identifiers.MultipleChoiceItem> MultipleChoiceItem = (
            from stem in StemText
            from alternatives in Alternative.Many()
            select new Identifiers.MultipleChoiceItem(stem, alternatives));

        public static readonly Parser<Identifiers.Assessment>  Assesment = (
            from heading in Heading
            from items in MultipleChoiceItem.Many()
            select new Identifiers.Assessment(heading, items));
    }
}