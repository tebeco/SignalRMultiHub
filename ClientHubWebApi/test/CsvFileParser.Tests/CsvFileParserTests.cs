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

        [Theory]
        [InlineData("-20")]
        [InlineData("1.2")]
        public void Should_not_parse_When_negative_or_decimal(string value)
        {
            var sequence = value.ToReadOnlySequence();

            var worked = PipeParser.TryParsePositiveInteger(sequence, out var _);

            Assert.False(worked);
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
    }
}
