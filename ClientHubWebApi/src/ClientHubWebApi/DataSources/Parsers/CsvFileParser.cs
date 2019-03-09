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
    public class CsvParser
    {
        public ConcurrentDictionary<string, List<Stock>> ParseFolder(string folder, string searchPattern)
        {
            var files = Directory.GetFiles(folder, searchPattern);
            var stocksPerFile = new ConcurrentDictionary<string, List<Stock>>(Environment.ProcessorCount, files.Length);

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, fileName =>
            {
                var stocks = ParseFile(fileName);
                stocksPerFile.TryAdd(Path.GetFileName(fileName), stocks);
            });

            return stocksPerFile;
        }

        public List<Stock> ParseFile(string fileName)
        {
            using (var reader = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None, 1024))
            {
                return ParseStream(reader);
            }
        }

        public List<Stock> ParseStream(Stream stream)
        {
            var lineParser = new LineParser();
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
                    stocks.Add(lineParser.ParseLine(sb));
                }
            }

            return stocks;
        }

    }
}
