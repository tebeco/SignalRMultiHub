using System;
using System.Collections.Generic;
using System.Text;

namespace ClientHubWebApi.DataSources.Kaggle
{
    public class LineParser : ILineParser<Stock>
    {
        //private List<Stock> list = new List<Stock>();

        public Stock ParseLine(StringBuilder line)
        {
            var startIndex = 0;

            //Date,Open,High,Low,Close,Volume,OpenInt
            //1986-03-13,0.0672,0.07533,0.0672,0.07533,1371330506,0
            var name = "NEED TO CHANGE NAME";
            var date = ParseSectionAsDateTime(line, ref startIndex);
            var open = ParseSectionAsDecimal(line, ref startIndex);
            var high = ParseSectionAsDecimal(line, ref startIndex);
            var low = ParseSectionAsDecimal(line, ref startIndex);
            var close = ParseSectionAsDecimal(line, ref startIndex);
            var volume = ParseSectionAsInt(line, ref startIndex);
            var openInt = ParseSectionAsInt(line, ref startIndex);

            return new Stock(name, date, open, high, low, close, volume, openInt);
        }

        public static decimal ParseSectionAsDecimal(StringBuilder line, ref int startIndex)
        {
            decimal val = 0;
            bool seenDot = false;
            int fractionCounter = 10;

            for (var index = startIndex; index < line.Length; index++)
            {
                // move along the line until we have skipped the required amount of commas
                var c = line[index];
                if (c == ',')
                {
                    startIndex = index + 1;
                    break;
                }

                if (c == '.')
                {
                    seenDot = true;
                    continue;
                }

                // before the . eg; 12.34 this looks for the 12
                if (seenDot == false)
                {
                    val *= 10;
                    val += c - '0';
                }
                else
                {
                    val += decimal.Divide(c - '0', fractionCounter);
                    fractionCounter *= 10;
                }
            }

            return val;
        }

        public static int ParseSectionAsInt(StringBuilder line, ref int startIndex)
        {
            int val = 0;

            for (var index = startIndex; index < line.Length; index++)
            {
                var c = line[index];
                if (c == ',')
                {
                    startIndex = index + 1;
                    break;
                }

                val *= 10;
                val += c - '0';
            }

            return val;
        }

        public static long ParseSectionAsLong(StringBuilder line, ref int startIndex)
        {
            long val = 0;

            for (var index = startIndex; index < line.Length; index++)
            {
                var c = line[index];
                if (c == ',')
                {
                    startIndex = index + 1;
                    break;
                }

                val *= 10;
                val += c - '0';
            }

            return val;
        }

        public static DateTime ParseSectionAsDateTime(StringBuilder line, ref int startIndex)
        {
            int dashCounter = 0;
            int day = -1;
            int month = -1;
            int year = -1;
            int val = 0;

            for (var index = startIndex; index < line.Length; index++)
            {
                var c = line[index];
                if (c == ',')
                {
                    startIndex = index + 1;
                    break;
                }

                if (c == '-')
                {
                    dashCounter++;

                    if (dashCounter == 1)
                    {
                        year = val;
                    }
                    else if (dashCounter == 2)
                    {
                        month = val;
                    }
                    val = 0;
                    continue;
                }

                val *= 10;
                val += c - '0';
            }

            day = val;

            return new DateTime(year, month, day);
        }
    }
}
