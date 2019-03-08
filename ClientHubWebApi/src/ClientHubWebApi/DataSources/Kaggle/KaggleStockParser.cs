using ClientHubWebApi.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHubWebApi.DataSources.Kaggle
{

    public class KaggleStockParser
    {
        private readonly DataOptions _options;

        public KaggleStockParser(IOptions<DataOptions> options)
        {
            _options = options.Value;
        }

        public void ParseData()
        {
            var folders = new[] { _options.Stock.Folder, _options.Etf.Folder };
            Parallel.ForEach(folders, fileName => ParseFolder(fileName));
        }

        public void ParseFolder(string folder)
        {
            var files = Directory.EnumerateFiles(folder).ToArray();
            Parallel.ForEach(files, fileName =>
            {
                TryParseFile(fileName);
            });
        }

        public bool TryParseFile(string fileName)
        {
            using (var reader = new FileStream(Path.Combine(_options.Stock.Folder, fileName), FileMode.Open, FileAccess.Read, FileShare.None, 1024))
            {
                try
                {
                    ParseStream(reader);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public List<Stock> ParseStream(Stream stream)
        {
            var lineParser = new LineParser();
            var sb = new StringBuilder();
            var stocks = new List<Stock>(1000);

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
                        break;
                    }

                    sb.Append(character);
                }

                if (endOfFile)
                {
                    break;
                }

                stocks.Add(lineParser.ParseLine(sb));
            }

            return stocks;
        }
    }
}
