using ClientHubWebApi;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HubHandlingWebClient.DataSources
{
    public class PeriodicDataSource<T>
    {
        private Timer timer = null;
        private readonly IDataSource<T> _dataSource;
        private readonly SubscribtionManager<T> _subscribtionManager;
        private CancellationToken _cancellationToken;
        private readonly ChannelWriter<T> _channelWriter;

        public PeriodicDataSource(IDataSource<T> dataSource, ChannelWriter<T> channelWriter)
        {
            _dataSource = dataSource;
            _channelWriter = channelWriter;
        }

        public void Subscribe(CancellationToken cancellationToken)
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

            await _channelWriter.WriteAsync(data, _cancellationToken);
        }
    }
}