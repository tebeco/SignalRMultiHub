using System;
using Xunit;
using ClientHubWebApi.DataSources.Kaggle;
using System.Text;

namespace ClientHubWebApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var line = "1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0";
            var sb = new StringBuilder(line);

            var parser = new StockLineParser();
            var stock = parser.ParseLine(sb);

            Assert.Equal<long>(1371330506, stock.Volume);
        }
    }
}
