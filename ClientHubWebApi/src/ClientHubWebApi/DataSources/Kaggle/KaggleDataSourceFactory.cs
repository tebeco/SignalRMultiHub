using ClientHubWebApi.Configuration;
using ClientHubWebApi.DataSources.Parsers;
using HubHandlingWebClient;
using HubHandlingWebClient.DataSources;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHubWebApi.DataSources.Kaggle
{

    public class KaggleDataSourceFactory
    {
        private readonly KaggleDataSourceFactoryOptions _options;
        private ConcurrentDictionary<string, List<Stock>> _kaggleStockDataSources;
        private static List<Stock> _defaultStocks = new List<Stock>
        {
            new Stock("Default StockName", new DateTime(1986, 03, 13), 1,1,1,1,1,1),
            new Stock("Default StockName", new DateTime(1986, 03, 13), 2, 2, 2, 2, 2, 2),
            new Stock("Default StockName", new DateTime(1986, 03, 13), 3, 3, 3, 3, 3, 3),
        };

        public KaggleDataSourceFactory(IOptions<KaggleDataSourceFactoryOptions> options)
        {
            _options = options.Value;
        }

        public void Initialize()
        {
            _kaggleStockDataSources = CsvStockParser.ParseFolder(_options.Stock.Folder, _options.Stock.GlobbingPattern);
        }

        public IDataSource<Stock> GetOrCreateStockDataSource(RequestStream requestStream)
        {
            var source = _kaggleStockDataSources.GetValueOrDefault(requestStream.Underlying, _defaultStocks);
            return new InfiniteKaggleDataSource<Stock>(source);
        }
    }
}
