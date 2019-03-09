using ClientHubWebApi.DataSources.Kaggle;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHubWebApi.DataSources.Parsers
{
    public class CsvStockParser
    {
        public static ConcurrentDictionary<string, List<Stock>> ParseFolder(string folder, string searchPattern)
        {
            var files = Directory.GetFiles(folder, searchPattern);
            var stocksPerFile = new ConcurrentDictionary<string, List<Stock>>(Environment.ProcessorCount, files.Length);

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, fileName =>
            {
                var stocks = ParseFile(fileName);
                stocksPerFile.TryAdd(Path.GetFileName(fileName).Split('.')[0], stocks);
            });

            return stocksPerFile;
        }

        public static List<Stock> ParseFile(string fileName)
        {
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, 1024))
            {
                return ParseStream(reader, Path.GetFileName(fileName));
            }
        }

        public static List<Stock> ParseStream(Stream stream, string stockName)
        {
            var sb = new StringBuilder();
            var stocks = new List<Stock>(1000);
            var ignoringLine = false;
            bool endOfFile = false;
            while (stream.CanRead)
            {
                sb.Clear();

                while (endOfFile == false)
                {
                    var readByte = stream.ReadByte();

                    if (readByte == -1)
                    {
                        endOfFile = true;
                        break;
                    }

                    var character = (char)readByte;

                    if (character == '\r')
                    {
                        continue;
                    }

                    if (character == '\n')
                    {
                        ignoringLine = false;
                        break;
                    }

                    if ((character < '0' || character > '9') && character != '-' && character != '.' && character != ',')
                    {
                        ignoringLine = true;
                    }

                    if (ignoringLine) { continue; }

                    sb.Append(character);
                }

                if (endOfFile)
                {
                    break;
                }

                if (sb.Length != 0)
                {
                    stocks.Add(StockLineParser.ParseLine(sb, stockName));
                }
            }

            return stocks;
        }

    }
}
