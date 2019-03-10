using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace CsvFileParser
{
    public class PipeParser
    {
        public static int ParsePipe(PipeReader reader)
        {
            while (true)
            {
                reader.TryRead(out var result);

                ReadOnlySequence<byte> buffer = result.Buffer;
                SequencePosition? position = null;

                do
                {
                    // Look for a comma in the buffer
                    position = buffer.PositionOf((byte)',') ?? buffer.End;

                    if (position != null)
                    {
                        // Process the line
                        ProcessLine(buffer.Slice(0, position.Value));

                        // Skip the line + the \n character (basically position)
                        buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                    }
                }
                while (position != null);

                // Tell the PipeReader how much of the buffer we have consumed
                reader.AdvanceTo(buffer.Start, buffer.End);

                // Stop reading if there's no more data coming
                if (result.IsCompleted)
                {
                    break;
                }
            }

            // Mark the PipeReader as complete
            reader.Complete();

            return 0;
        }

        public static void ProcessLine(ReadOnlySequence<byte> readOnlySequence) { }

        public static bool TryParsePositiveInteger(ReadOnlySequence<char> sequence, out int value)
        {
            int val = 0;

            foreach (var memory in sequence)
            {
                foreach (var c in memory.Span)
                {
                    if(c <'0' || c > '9')
                    {
                        value = default;
                        return false;
                    }
                    val *= 10;
                    val += c - '0';
                }
            }

            value = val;
            return true;
        }

        public static bool TryParsePositiveLong(ReadOnlySequence<char> sequence, out long value)
        {
            long val = 0;

            foreach (var memory in sequence)
            {
                foreach (var c in memory.Span)
                {
                    if (c < '0' || c > '9')
                    {
                        value = default;
                        return false;
                    }
                    val *= 10;
                    val += c - '0';
                }
            }

            value = val;
            return true;
        }
    }
}
