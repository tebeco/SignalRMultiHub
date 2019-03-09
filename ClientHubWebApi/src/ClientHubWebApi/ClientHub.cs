using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ClientHubWebApi
{
    public class ClientHub : Hub
    {
        private readonly SubscribtionManager _subscribtionManager;

        public ClientHub(SubscribtionManager subscribtionManager)
        {
            _subscribtionManager = subscribtionManager;
        }

        public async Task GetStream(RequestStream requestStream)
        {
            await Task.Yield();
           //var channel = _subscribtionManager.GetChannel(requestStream);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.CompletedTask;
        }
    }

    public class RequestStream
    {
        string Underlying { get; set; }
    }
}
