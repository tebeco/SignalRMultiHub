using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HubHandlingWebClient
{
    public class TimedDataGenerator : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ClientGroupManager clientGroupManager;
        private readonly ILogger<TimedDataGenerator> logger;
        private Timer timer = null;

        public TimedDataGenerator(ClientGroupManager clientGroupManager, ILogger<TimedDataGenerator> logger)
        {
            this.clientGroupManager = clientGroupManager;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            timer = new Timer(Timer_Callback, null, 1000, 1000);
        }

        private void Timer_Callback(object state)
        {
            _ = CallbackAsync();
            timer.Change(1000, 1000);
        }

        private async Task CallbackAsync()
        {
            await clientGroupManager.SendDataAsync().ConfigureAwait(false);
        }
    }
}