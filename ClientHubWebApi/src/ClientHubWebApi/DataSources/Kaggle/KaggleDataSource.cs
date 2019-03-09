using ClientHubWebApi.Configuration;
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

    public class KaggleDataSource
    {
        private readonly DataOptions _options;

        public KaggleDataSource(IOptions<DataOptions> options)
        {
            _options = options.Value;
        }

        public void ParseStocks()
        {
            //ParseFolder(_options.Etf.Folder, _options.Etf.GlobbingPattern);
        }
    }
}
