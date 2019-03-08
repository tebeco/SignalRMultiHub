using System;
using Xunit;
using ClientHubWebApi.DataSources.Kaggle;
using System.Text;

namespace ClientHubWebApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Should_parse_stock_DateTime()
        {
            //var line = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0";

            var expectedDate = new DateTime(1986, 03, 13);
            var line = "1986-03-13,";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedDate = LineParser.ParseSectionAsDateTime(sb, ref startIndex);

            Assert.Equal(expectedDate, parsedDate);
        }

        [Fact]
        public void Should_parse_stock_Long()
        {
            //var line = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0";

            var expectedLong = 1371330506L;
            var line = ",1371330506,";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedLong = LineParser.ParseSectionAsLong(sb, ref startIndex);

            Assert.Equal(expectedLong, parsedLong);
        }

        [Fact]
        public void Should_parse_stock_Int()
        {
            //var line = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0";

            var expectedLong = 1371330506;
            var line = ",1371330506,";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedLong = LineParser.ParseSectionAsInt(sb, ref startIndex);

            Assert.Equal(expectedLong, parsedLong);
        }

        [Fact]
        public void Should_parse_stock_Decimal()
        {
            //var line = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0";

            var expectedDecimal = 0.0672M;
            var line = ",0.0672,";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedDecimal = LineParser.ParseSectionAsDecimal(sb, ref startIndex);

            Assert.Equal(expectedDecimal, parsedDecimal);
        }
    }
}
