using System;
using System.Collections.Generic;
using System.Text;

namespace ClientHubWebApi.DataSources.Kaggle
{
    public sealed class LineParser : ILineParser
    {
        private List<Stock> list = new List<Stock>();

        public void ParseLine(StringBuilder line)
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

            var valueHolder = new Stock(name, date, open, high, low, close, volume, openInt);
        }

        private static decimal ParseSectionAsDecimal(StringBuilder line, ref int startIndex)
        {
            decimal val = 0;
            bool seenDot = false;
            int fractionCounter = 10;
            int counter = 0;
            bool flip = false;

            for (var index = startIndex; index < line.Length; index++)
            {
                // move along the line until we have skipped the required amount of commas
                var c = line[index];
                if (c == ',')
                {
                    counter++;

                    if (counter == 2)
                    {
                        startIndex = index;
                        break;
                    }
                    continue;
                }

                // we have skipped enough commas, the next section before the upcoming comma is what we are interested in
                // the number is a negative means we have to flip it at the end.
                if (c == '-')
                {
                    flip = true;
                    continue;
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

            return flip ? -val : val;
        }

        private static int ParseSectionAsInt(StringBuilder line, ref int startIndex)
        {
            int val = 0;
            int counter = 0;
            bool flip = false;

            for (var index = startIndex; index < line.Length; index++)
            {
                var c = line[index];
                if (c == ',')
                {
                    counter++;

                    if (counter == 2)
                    {
                        startIndex = index;
                        break;
                    }
                    continue;
                }

                // the number is a negative means we have to flip it at the end.
                if (c == '-')
                {
                    flip = true;
                    continue;
                }

                val *= 10;
                val += c - '0';
            }

            return flip ? -val : val;
        }

        private static DateTime ParseSectionAsDateTime(StringBuilder line, ref int startIndex)
        {
            int commaCounter = 0;
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
                    commaCounter++;

                    if (commaCounter == 2)
                    {
                        startIndex = index;
                        break;
                    }
                    continue;
                }

                if (c == '-')
                {
                    dashCounter++;

                    if(dashCounter == 0)
                    {
                        day = val;
                    }
                    else if (dashCounter == 1)
                    {
                        month = val;
                    }
                    else
                    {
                        year = val;
                    }
                    continue;
                }

                val *= 10;
                val += c - '0';

            }
            return DateTime.Now;
        }

        public void ParseLine(string line)
        {
            throw new NotImplementedException();
        }

        public void ParseLine(char[] line)
        {
            throw new NotImplementedException();
        }

        public void Dump()
        {
            throw new NotImplementedException();
        }
    }
}
