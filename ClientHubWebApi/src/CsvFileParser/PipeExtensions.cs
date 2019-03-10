using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace System
{
    public static class PipeReaderExtensions
    {
        public static PipeReader ToPipeReader(this string value)
        {
            var pipe = new Pipe();
            var valueBytes = Encoding.UTF8.GetBytes(value);

            var pipeBuffer = pipe.Writer.GetMemory(valueBytes.Length);
            valueBytes.CopyTo(pipeBuffer);

            pipe.Writer.Advance(valueBytes.Length);
            pipe.Writer.Complete();

            return pipe.Reader;
        }

        public static ReadOnlySequence<char> ToReadOnlySequence(this string value)
        {
            return new ReadOnlySequence<char>(value.AsMemory());
        }
    }
}
