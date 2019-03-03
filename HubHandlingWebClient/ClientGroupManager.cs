using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HubHandlingWebClient
{
    public class ClientGroupManager
    {
        private readonly IHubContext<ClientHub> clientHubContext;
        private readonly IApplicationLifetime applicationLifetime;
        private readonly ILogger<ClientGroupManager> logger;
        private readonly ConcurrentDictionary<string, ConcurrentBag<string>> groupPerClient = new ConcurrentDictionary<string, ConcurrentBag<string>>();

        public ClientGroupManager(IHubContext<ClientHub> clientHubContext, IApplicationLifetime applicationLifetime, ILogger<ClientGroupManager> logger)
        {
            this.clientHubContext = clientHubContext;
            this.applicationLifetime = applicationLifetime;
            this.logger = logger;
        }

        public async Task SendDataAsync()
        {
            var distincGroups = groupPerClient.SelectMany(kv => kv.Value).Distinct().ToList();
            foreach (var groupName in distincGroups)
            {
                Guid guid = Guid.NewGuid();
                var group = clientHubContext.Clients.Group(groupName);
                if (group != null)
                {
                    var payload = new
                    {
                        groupId = groupName,
                        data = guid
                    };

                    logger.LogInformation($"Sending '{payload}' to group '{groupName}'");
                    await group.SendAsync("foo", payload, applicationLifetime.ApplicationStopping).ConfigureAwait(false);
                }
            }
        }

        public async Task AddClientAsync(string connectionId, string groupName)
        {
            logger.LogInformation($"Adding client '{connectionId}' to group '{groupName}'");

            var groups = groupPerClient.GetOrAdd(connectionId, (key) => new ConcurrentBag<string>());
            groups.Add(groupName);
            await clientHubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public void RemoveClient(string connectionId)
        {
            logger.LogInformation($"Removing client '{connectionId}'");

            groupPerClient.TryRemove(connectionId, out var _);
        }
    }
}