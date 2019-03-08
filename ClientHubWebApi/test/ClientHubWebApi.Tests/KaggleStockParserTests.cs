using System;
using Xunit;
using ClientHubWebApi.DataSources.Kaggle;
using System.Text;
using ClientHubWebApi.DataSources;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using ClientHubWebApi.Configuration;

namespace ClientHubWebApi.Tests
{
    public class UnitTest1
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

            CompareStock(expectedStock, parsedStock);
        }

        [Fact]
        public void Should_parse_three_line()
        {
            //public void ParseStream(Stream stream)
            var expectedStocks = new List<Stock>
            {
                new Stock("", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0),
                new Stock("", new DateTime(1986, 03, 14), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0),
                new Stock("", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 1234.5678M, 1371330506, 0)
            };

            var line1 = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0\r\n";
            var line2 = "1986-03-14,0.0672,0.07533,0.0672,0.07533,1371330506,0\n";
            var line3 = "1986-03-13,0.0672,0.07533,0.0672,1234.5678,1371330506,0\n";

            List<Stock> parsedStocks = null;
            var kaggleParser = new KaggleStockParser(Options.Create(new DataOptions()));

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(line1);
                writer.Write(line2);
                writer.Write(line3);
                writer.Flush();

                memoryStream.Seek(0, SeekOrigin.Begin);
                parsedStocks = kaggleParser.ParseStream(memoryStream);
            }

            Assert.Equal(3, parsedStocks.Count);
            for (int index = 0; index < parsedStocks.Count; index++)
            {
                CompareStock(expectedStocks[index], parsedStocks[index]);
            }
        }

        [Fact]
        public void Should_parse_file()
        {
            var filePath = "";
            var kaggleParser = new KaggleStockParser(Options.Create(new DataOptions()));

            kaggleParser.TryParseFile(filePath);
        }

        private static void CompareStock(Stock expectedStock, Stock parsedStock)
        {
            Assert.Equal(expectedStock.Date, parsedStock.Date);
            Assert.Equal(expectedStock.Open, parsedStock.Open);
            Assert.Equal(expectedStock.Low, parsedStock.Low);
            Assert.Equal(expectedStock.High, parsedStock.High);
            Assert.Equal(expectedStock.Close, parsedStock.Close);
            Assert.Equal(expectedStock.Volume, parsedStock.Volume);
            Assert.Equal(expectedStock.OpenInt, parsedStock.OpenInt);
        }
    }
}
