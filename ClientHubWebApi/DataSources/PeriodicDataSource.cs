using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HubHandlingWebClient
{
    public class PeriodicDataSource<T>
    {
        private Timer timer = null;
        private readonly IDataSource<T> _dataSource;
        private CancellationToken _cancellationToken;

        public PeriodicDataSource(IDataSource<T> dataSource)
        {
            _dataSource = dataSource;
        }

        protected void Subscribe(string topic, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            timer = new Timer(Timer_Callback, null, 1000, 1000);
        }

        private void Timer_Callback(object state)
        {
            _ = CallbackAsync();
            if (!_cancellationToken.IsCancellationRequested)
            {
                timer.Change(1000, 1000);
            }
        }

        private async Task CallbackAsync()
        {
            await Task.Yield();
            var data = _dataSource.GetNextData();
            
            //Push it
        }
    }
}