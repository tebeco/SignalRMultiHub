using System;
using System.Threading.Tasks;
using ClientHubWebApi.DataSources;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ClientHubWebApi
{
    public class ClientHub : Hub
    {
        public ClientHub()
        {
        }

        public Task GetStockStreamAsync(RequestStream requestStream)
        {
            var subscribtionManager = new SubscribtionManager<Stock>();
            var channelReader = subscribtionManager.GetChannelReader(requestStream);

            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}
