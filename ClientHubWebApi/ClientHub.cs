using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ClientHubWebApi
{
    public class ClientHub : Hub
    {
        private readonly ILogger<ClientHub> logger;

        public ClientHub(ILogger<ClientHub> logger)
        {
            this.logger = logger;
        }

        public async Task GetStream(RequestStream requestStream)
        {
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}
