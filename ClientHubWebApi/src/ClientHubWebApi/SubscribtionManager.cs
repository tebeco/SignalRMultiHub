using ClientHubWebApi.Configuration;
using ClientHubWebApi.DataSources;
using ClientHubWebApi.DataSources.Kaggle;
using HubHandlingWebClient;
using HubHandlingWebClient.DataSources;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;

namespace ClientHubWebApi
{
    public class SubscribtionManager<T>
    {
        private readonly ConcurrentDictionary<string, Channel<Stock>> _stockChannels = new ConcurrentDictionary<string, Channel<Stock>>();
        private readonly KaggleDataSourceFactory _kaggleDataSourceFactory;

        public SubscribtionManager(KaggleDataSourceFactory kaggleDataSourceFactory)
        {
            _kaggleDataSourceFactory = kaggleDataSourceFactory;
        }

        public ChannelReader<Stock> GetChannelReader(RequestStream requestStream)
        {
            var channelName = GetChannelNameFromRequest(requestStream);
            var channel = _stockChannels.GetOrAdd(channelName, (keyName, factoryArg) => CreateStockDataSource(keyName, factoryArg), requestStream);

            return channel.Reader;
        }

        private Channel<Stock> CreateStockDataSource(string channelName, RequestStream requestStream)
        {
            var kaggleDataSource = _kaggleDataSourceFactory.GetOrCreateStockDataSource(requestStream);
            var channel = Channel.CreateUnbounded<Stock>();

            var dataSource = new PeriodicDataSource<Stock>(kaggleDataSource, channel.Writer);
            dataSource.Subscribe(CancellationToken.None);

            return channel;
        }

        public string GetChannelNameFromRequest(RequestStream requestStream)
        {
            return requestStream.Underlying;
        }
    }
}
