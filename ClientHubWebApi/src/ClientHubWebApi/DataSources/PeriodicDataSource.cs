using ClientHubWebApi;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HubHandlingWebClient.DataSources
{
    public class PeriodicDataSource<T>
    {
        private Timer _timer = null;
        private readonly IDataSource<T> _dataSource;
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
            _timer = new Timer(Timer_Callback, null, 100, 100);
        }

        private void Timer_Callback(object state)
        {
            _ = CallbackAsync();
            if (!_cancellationToken.IsCancellationRequested)
            {
                _timer.Change(100, 100);
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