using System;
using Xunit;
using System.Linq;
using ClientHubWebApi.DataSources.Parsers;
using System.Collections.Generic;
using ClientHubWebApi.DataSources;
using System.IO;

namespace ClientHubWebApi.Tests
{
    public class CsvFileParserTests
    {
        [Fact]
        public void Should_parse_file()
        {
            var filePath = "msft.us.txt";
           
            var parsedStocks = CsvStockParser.ParseFile(filePath);

            Assert.Equal(7983, parsedStocks.Count);
        }


        [Fact]
        public void Should_parse_folder()
        {
            var fileName = "msft.us.txt";
            var parsedStocksPerFile = CsvStockParser.ParseFolder(Environment.CurrentDirectory, fileName);

            Assert.Single(parsedStocksPerFile);
            var parsedFileName = parsedStocksPerFile.Keys.First();
            Assert.Equal(fileName, parsedFileName);
            var parsedStocks = parsedStocksPerFile.Values.First();
            Assert.Equal(7983, parsedStocks.Count);
        }

        [Fact]
        public void Should_parse_three_line()
        {
            var expectedStocks = new List<Stock>
            {
                new Stock("some stock name", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0),
                new Stock("some stock name", new DateTime(1986, 03, 14), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0),
                new Stock("some stock name", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 1234.5678M, 1371330506, 0)
            };

            var line1 = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0\r\n";
            var line2 = "1986-03-14,0.0672,0.07533,0.0672,0.07533,1371330506,0\n";
            var line3 = "1986-03-13,0.0672,0.07533,0.0672,1234.5678,1371330506,0\n";

            List<Stock> parsedStocks = null;

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(line1);
                writer.Write(line2);
                writer.Write(line3);
                writer.Flush();

                memoryStream.Seek(0, SeekOrigin.Begin);
                parsedStocks = CsvStockParser.ParseStream(memoryStream, "some stock name");
            }

            Assert.Equal(3, parsedStocks.Count);
            for (int index = 0; index < parsedStocks.Count; index++)
            {
                StockTestHelper.AssertEqual(expectedStocks[index], parsedStocks[index]);
            }
        }

        [Fact]
        public void Should_parse_ignore_headers()
        {
            var expectedStocks = new List<Stock>
            {
                new Stock("some stock name", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0),
                new Stock("some stock name", new DateTime(1986, 03, 14), 0.0672M, 0.07533M, 0.0672M, 0.07533M, 1371330506, 0),
                new Stock("some stock name", new DateTime(1986, 03, 13), 0.0672M, 0.07533M, 0.0672M, 1234.5678M, 1371330506, 0)
            };

            var headers = "Date,Open,High,Low,Close,Volume,OpenInt\n";
            var line1 = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0\r\n";
            var line2 = "1986-03-14,0.0672,0.07533,0.0672,0.07533,1371330506,0\n";
            var line3 = "1986-03-13,0.0672,0.07533,0.0672,1234.5678,1371330506,0\n";

            List<Stock> parsedStocks = null;

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(headers);
                writer.Write(line1);
                writer.Write(line2);
                writer.Write(line3);
                writer.Flush();

                memoryStream.Seek(0, SeekOrigin.Begin);
                parsedStocks = CsvStockParser.ParseStream(memoryStream, "some stock name");
            }

            Assert.Equal(3, parsedStocks.Count);
            for (int index = 0; index < parsedStocks.Count; index++)
            {
                StockTestHelper.AssertEqual(expectedStocks[index], parsedStocks[index]);
            }
        }
    }
}
