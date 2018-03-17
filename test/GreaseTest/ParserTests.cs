using System;
using Xunit;
using Grease;
using Sprache;
using System.Linq;

namespace GreaseTest
{
    public class ParserTests
    {
        [Theory]
        [InlineData("Simple CRLF\r\n", "Simple CRLF")]
        [InlineData("Double CRLF\r\n\r\n", "Double CRLF")]
        [InlineData("Simple LF\n", "Simple LF")]
        [InlineData("Double LF\n\n", "Double LF")]
        [InlineData(" Leading Space", "Leading Space")]
        public void OnlyTextOnLine(string input, string expected) {
            var parsed = Grammar.TextOnLine.Parse(input);
            Assert.Equal(expected, parsed);
        }

        [Fact]
        public void HeadingsStartWithAPound()
        {
            string input = "# A Heading\n";
            var parsed = Grammar.Heading.Parse(input);
            Assert.Equal("A Heading", parsed);
        }

        [Fact]
        public void HeadingsCannotBeMultiline()
        {
            string input = @"# First line
            and second line";
            var parsed = Grammar.Heading.Parse(input);
            Assert.Equal("First line", parsed);

            string lineFeedOnly = "# First line\nSecond Line";
            var lfParse = Grammar.Heading.Parse(lineFeedOnly);
            Assert.Equal("First line", lfParse);
        }

        [Fact]
        public void QuestionsTextStartWithANumber()
        {
            string input = "1. Is this a question?";
            var parsed = Grammar.StemText.Parse(input);
            Assert.Equal("Is this a question?", parsed);
        }

        [Fact]
        public void QuestionsCannotBeMultiLine()
        {
            string input = @"1. First line and
            Second line";
            var parsed = Grammar.StemText.Parse(input);
            Assert.Equal("First line and", parsed);
        }

        [Fact]
        public void AlternativesCanBeSingleSelect()
        {
            string input = "* ( ) an alternative";
            var parsed = Grammar.Alternative.Parse(input);
            Assert.Equal("an alternative", parsed.Text);
            Assert.Equal(AlternativeType.SingleSelect, parsed.Type);
        }

        [Fact]
        public void AlternativesCanBeMultiSelect()
        {
            string input = "* [ ] an alternative";
            var parsed = Grammar.Alternative.Parse(input);
            Assert.Equal("an alternative", parsed.Text);
            Assert.Equal(AlternativeType.MultiSelect, parsed.Type);
        }

        [Theory]
        [InlineData("* [x] an answer")]
        [InlineData("* (x) an answer")]
        public void AlternativesCanBeAnAnswer(string value)
        {
            var parsed = Grammar.Alternative.Parse(value);
            Assert.True(parsed.IsAnswer);
        }

        [Fact]
        public void AlternativeCannotBeMultiLine()
        {
            string input = @"* [ ] first portion
            and second portion";
            var parsed = Grammar.Alternative.Parse(input);
            Assert.Equal("first portion", parsed.Text);
        }

        [Fact]
        public void CreateAMultipleChoiceItem()
        {
            string input = @"1. This is the stem
            * [ ] First distractor
            * [x] First answer
            * [x] Second answer";

            var parsed = Grammar.MultipleChoiceItem.Parse(input);
            Assert.Equal("This is the stem", parsed.Stem);
            Assert.Equal(3, parsed.Alternatives.Count());
        }

        [Fact]
        public void MultipleChoiceItemHasOnlyOneTypeOfAlternative()
        {
            string input = @"1. This is the stem
            * [ ] First distractor
            * (x) First answer
            * [x] Second answer";

            var parsed = Grammar.MultipleChoiceItem.Parse(input);
            Assert.Equal(2, parsed.Alternatives.Count());
        }

        [Fact]
        public void MultipleChoiceItemIsOnlyTypeOfFirstAlternative()
        {
            string input = @"1. This is the stem
            * ( ) First distractor
            * [x] First answer
            * [x] Second answer";

            var parsed = Grammar.MultipleChoiceItem.Parse(input);
            Assert.Equal(1, parsed.Alternatives.Count());
        }

        [Fact]
        public void CreateAnAssessment()
        {
            string input = @"# The title
            1. Some item
            * [ ] dist
            * [x] answer
            1. The other item
            * (x) werd";

            var parsed = Grammar.Assesment.Parse(input);
            Assert.Equal("The title", parsed.Title);
            Assert.Equal(2, parsed.Items.Count);
        }

        [Fact]
        public void RealWorldAssessment() {
            string input = "# Unsegmented Worms\n\n1. What are the three classes of flatworm?\n  * [x] some words\n";

            var parsed = Grammar.Assesment.Parse(input);
            Assert.Equal("Unsegmented Worms", parsed.Title);
            Assert.Equal(1, parsed.Items.Count);
        }
    }
}
