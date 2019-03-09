using ClientHubWebApi.DataSources;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ClientHubWebApi.Tests
{
    class StockTestHelper
    {
        public static void AssertEqual(Stock expectedStock, Stock parsedStock)
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
