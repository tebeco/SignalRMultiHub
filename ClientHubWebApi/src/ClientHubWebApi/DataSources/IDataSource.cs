using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubHandlingWebClient.DataSources
{
    public interface IDataSource<T>
    {
        T GetNextData();
    }
}
