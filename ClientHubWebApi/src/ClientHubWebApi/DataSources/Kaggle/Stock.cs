using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientHubWebApi.DataSources
{
    public struct Stock
    {
        public Stock(string name, DateTime date, decimal open, decimal high, decimal low, decimal close, long volume, long openInt)
        {
            Name = name;
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
            OpenInt = openInt;
        }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long Volume { get; set; }

        public long OpenInt { get; set; }
    }
}
