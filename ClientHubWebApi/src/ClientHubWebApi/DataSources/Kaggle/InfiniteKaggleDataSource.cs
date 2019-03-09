using HubHandlingWebClient.DataSources;
using System.Collections.Generic;

namespace ClientHubWebApi.DataSources.Kaggle
{
    public class InfiniteKaggleDataSource<T> : IDataSource<T>
    {
        private readonly List<T> _source;
        private int currentIndex = 0;
        private int indexIncrement = 1;

        public InfiniteKaggleDataSource(List<T> source)
        {
            _source = source;
        }

        public T GetNextData()
        {
            var data = _source[currentIndex];
            currentIndex += indexIncrement;

            if (currentIndex == _source.Count)
            {
                indexIncrement = -1;
            }
            else if(currentIndex == -1)
            {
                indexIncrement = 1;
            }

            return data;
        }
    }
}
