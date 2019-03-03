using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HubHandlingWebClient
{
    public class ClientHub : Hub
    {
        private readonly ClientGroupManager clientGroupManager;
        private readonly ILogger<ClientHub> logger;

        public ClientHub(ClientGroupManager clientGroupManager, ILogger<ClientHub> logger)
        {
            this.clientGroupManager = clientGroupManager;
            this.logger = logger;
        }

        public async Task RequestGroup(string groupName)
        {
            logger.LogInformation($"Adding client '{Context.ConnectionId}' to group : '{groupName}'");

            var connectionId = this.Context.ConnectionId;
            await clientGroupManager.AddClientAsync(connectionId, groupName);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            clientGroupManager.RemoveClient(Context.ConnectionId);
            return Task.CompletedTask;
        }
    }
}
