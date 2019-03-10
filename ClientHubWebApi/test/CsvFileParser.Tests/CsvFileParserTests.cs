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
            
            var parsedInt = PipeParser.ParseInt(sequence);

            Assert.Equal(expectedValue, parsedInt);
        }

        [Fact]
        public void Should_parse_Int_with_leading_zero()
        {
            var expectedValue = 1;
            var sequence = "0000000001".ToReadOnlySequence();

            var parsedInt = PipeParser.ParseInt(sequence);

            Assert.Equal(expectedValue, parsedInt);
        }
    }
}
