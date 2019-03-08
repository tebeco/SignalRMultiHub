using System.Collections.Concurrent;
using System.Threading.Channels;

namespace ClientHubWebApi
{
    public class SubscribtionManager
    {
        //private readonly ConcurrentDictionary<string, Channel> _channels = new ConcurrentDictionary<string, Channel>();

        public Channel<T> GetChannel<T>(RequestStream request)
        {
            return Channel.CreateUnbounded<T>();
        }
    }
}
