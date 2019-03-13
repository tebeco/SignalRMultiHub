using System;
using System.Buffers;
using System.Threading.Tasks;
using Xunit;

namespace CsvFileParser.Tests
{
    public class CsvFileParserTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        [InlineData(5123)]
        public void Should_parse_Int(int expectedValue)
        {
            var sequence = expectedValue.ToString().ToReadOnlySequence();

            var worked = PipeParser.TryParsePositiveInteger(sequence, out var parsedInt);

            Assert.True(worked);
            Assert.Equal(expectedValue, parsedInt);
        }

        [Fact]
        public void Should_parse_Int_with_leading_zero()
        {
            var expectedValue = 1;
            var sequence = "0000000001".ToReadOnlySequence();

            var worked = PipeParser.TryParsePositiveInteger(sequence, out var parsedInt);

            Assert.True(worked);
            Assert.Equal(expectedValue, parsedInt);
        }

        [Fact]
        public void Should_not_parse_Int_when_not_digit()
        {
            for (int i = 0; i < char.MaxValue; i++)
            {
                char c = (char)i;
                var charAsString = c.ToString().ToReadOnlySequence();

                if (c >= '0' && c <= '9')
                {
                    var worked = PipeParser.TryParsePositiveInteger(charAsString, out var _);
                    Assert.True(worked);
                }
                else
                {
                    var worked = PipeParser.TryParsePositiveInteger(charAsString, out var _);
                    Assert.False(worked);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(123)]
        [InlineData(5123)]
        public void Should_parse_Long(long expectedValue)
        {
            var sequence = expectedValue.ToString().ToReadOnlySequence();

            var worked = PipeParser.TryParsePositiveLong(sequence, out var parsedValue);

            Assert.True(worked);
            Assert.Equal(expectedValue, parsedValue);
        }

        [Fact]
        public void Should_parse_Decimal()
        {
            foreach (var expectedValue in new[] { 0M, 123M, 5123M, 51.23M })
            {
                var sequence = expectedValue.ToString().ToReadOnlySequence();

                var worked = PipeParser.TryParsePositiveDecimal(sequence, out var parsedValue);

                Assert.True(worked);
                Assert.Equal(expectedValue, parsedValue);
            }
        }

        [Theory]
        [InlineData("-20")]
        [InlineData("a")]
        public void Should_not_parse_When_negative_or_letter(string value)
        {
            var sequence = value.ToReadOnlySequence();

            var worked = PipeParser.TryParsePositiveDecimal(sequence, out var _);
            Assert.False(worked);

            worked = PipeParser.TryParsePositiveLong(sequence, out var _);
            Assert.False(worked);

            worked = PipeParser.TryParsePositiveInteger(sequence, out var _);
            Assert.False(worked);
        }

        [Fact]
        public void Should_stop_at_comma()
        {
            var input = "12345,6789";
            var reader = input.ToPipeReader();

            var possibleSequence = PipeParser.GetNextSequence(reader);
            Assert.NotNull(possibleSequence);
            var sequence = possibleSequence.Value;
            Assert.Equal(5, sequence.Length);

            var worked = PipeParser.TryParsePositiveInteger(sequence, out var parsed);
            Assert.True(worked);
            Assert.Equal(12345, parsed);
        }

        [Fact]
        public void Should_advance_by_sequence_by_comma()
        {
            var input = "12,34,56,78,9";
            var reader = input.ToPipeReader();

            var worked = PipeParser.TryParsePositiveInteger(reader, out var parsed);
            Assert.True(worked);
            Assert.Equal(12, parsed);

            worked = PipeParser.TryParsePositiveInteger(reader, out parsed);
            Assert.True(worked);
            Assert.Equal(34, parsed);
        }
    }
}
