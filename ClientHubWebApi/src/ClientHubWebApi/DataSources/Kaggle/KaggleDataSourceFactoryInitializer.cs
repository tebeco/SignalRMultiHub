using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClientHubWebApi.DataSources.Kaggle
{
    public class KaggleDataSourceFactoryInitializer : IHostedService
    {
        private readonly KaggleDataSourceFactory _kaggleDataSourceFactory;

        public KaggleDataSourceFactoryInitializer(KaggleDataSourceFactory kaggleDataSourceFactory)
        {
            _kaggleDataSourceFactory = kaggleDataSourceFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _kaggleDataSourceFactory.Initialize();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
