using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ClientHubWebApi.DataSources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ClientHubWebApi
{
    public class ClientHub : Hub
    {
        private readonly SubscribtionManager<Stock> _stockSubscribtionManager;

        public ClientHub(SubscribtionManager<Stock> stockSubscribtionManager)
        {
            _stockSubscribtionManager = stockSubscribtionManager;
        }

        public ChannelReader<Stock> GetStockStreamAsync(RequestStream requestStream, CancellationToken cancelationToken)
        {
            var channelReader = _stockSubscribtionManager.GetChannelReader(requestStream);

            return channelReader;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}
