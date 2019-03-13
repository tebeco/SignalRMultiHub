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
                SequencePosition? position;

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

        public static void ProcessLine(ReadOnlySequence<byte> _) { }

        public static ReadOnlySequence<byte>? GetNextSequence(PipeReader reader)
        {
            reader.TryRead(out var result);
            ReadOnlySequence<byte> buffer = result.Buffer;

            var position = buffer.PositionOf((byte)',');
            if (position != null)
            {
                reader.AdvanceTo(position.Value);
                return buffer.Slice(0, position.Value);
            }

            return null;
        }

        public static bool TryParsePositiveInteger(PipeReader reader, out int value)
        {
            var sequence = GetNextSequence(reader);
            if (sequence != null)
            {
                return TryParsePositiveInteger(sequence.Value, out value);
            }

            value = default;
            return false;
        }

        public static bool TryParsePositiveInteger(ReadOnlySequence<byte> sequence, out int value)
        {
            int val = 0;

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

        public static bool TryParsePositiveLong(ReadOnlySequence<byte> sequence, out long value)
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

        public static bool TryParsePositiveDecimal(ReadOnlySequence<byte> sequence, out decimal value)
        {
            decimal val = 0;
            bool seenDot = false;
            int fractionCounter = 10;

            foreach (var memory in sequence)
            {
                foreach (var c in memory.Span)
                {
                    if (c != '.' && (c < '0' || c > '9'))
                    {
                        value = default;
                        return false;
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
            }

            value = val;
            return true;
        }
    }
}
