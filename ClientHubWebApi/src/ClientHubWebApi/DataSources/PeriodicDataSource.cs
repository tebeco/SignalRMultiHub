using ClientHubWebApi;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HubHandlingWebClient
{
    public class PeriodicDataSource<T>
    {
        private Timer timer = null;
        private readonly IDataSource<T> _dataSource;
        private readonly SubscribtionManager _subscribtionManager;
        private CancellationToken _cancellationToken;
        //private Channel<T> _channel;

        public PeriodicDataSource(IDataSource<T> dataSource, SubscribtionManager subscribtionManager)
        {
            _dataSource = dataSource;
            _subscribtionManager = subscribtionManager;
        }

        protected void Subscribe(string topic, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            //_channel = _subscribtionManager.GetChannel();
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

            //await _channel.Writer.WriteAsync(data, _cancellationToken);
        }
    }
}