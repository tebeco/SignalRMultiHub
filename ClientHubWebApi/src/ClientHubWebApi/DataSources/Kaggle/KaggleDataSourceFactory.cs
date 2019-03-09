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
        private readonly DataOptions _options;
        private ConcurrentDictionary<string, List<Stock>> _kaggleStockDataSources;

        public KaggleDataSourceFactory(IOptions<DataOptions> options)
        {
            _options = options.Value;
        }

        public void Initialize()
        {
            _kaggleStockDataSources = CsvStockParser.ParseFolder(_options.Stock.Folder, _options.Stock.GlobbingPattern);
        }

        public IDataSource<Stock> CreateStockDataSource(RequestStream requestStream)
        {
            var source = _kaggleStockDataSources.GetValueOrDefault(requestStream.Underlying, new List<Stock>());
            return new InfiniteKaggleDataSource<Stock>(source);
        }
    }
}
