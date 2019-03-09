using System;
using Xunit;
using ClientHubWebApi.DataSources.Kaggle;
using System.Text;
using ClientHubWebApi.DataSources;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using ClientHubWebApi.Configuration;
using ClientHubWebApi.DataSources.Parsers;

namespace ClientHubWebApi.Tests
{

    public class CsvLineParserTests
    {
        [Fact]
        public void Should_parse_stock_DateTime()
        {
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
            var expectedLong = 1371330506L;
            var line = "1371330506,1";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedLong = LineParser.ParseSectionAsLong(sb, ref startIndex);

            Assert.Equal(expectedLong, parsedLong);
        }

        [Fact]
        public void Should_parse_stock_Int()
        {
            var expectedLong = 1371330506;
            var line = "1371330506,a";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedLong = LineParser.ParseSectionAsInt(sb, ref startIndex);

            Assert.Equal(expectedLong, parsedLong);
        }

        [Fact]
        public void Should_parse_stock_Decimal()
        {
            var expectedDecimal = 0.0672M;
            var line = "0.0672,s";
            var sb = new StringBuilder(line);
            var startIndex = 0;

            var parsedDecimal = LineParser.ParseSectionAsDecimal(sb, ref startIndex);

            Assert.Equal(expectedDecimal, parsedDecimal);
        }

        [Fact]
        public void Should_parse_line()
        {
            var expectedStock = new Stock("", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0);
            var line = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0";
            var sb = new StringBuilder(line);

            var lineParser = new LineParser();
            var parsedStock = lineParser.ParseLine(sb);

            StockTestHelper.AssertEqual(expectedStock, parsedStock);
        }
    }
}
