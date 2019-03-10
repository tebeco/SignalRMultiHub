using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CsvFileParser
{
    public class FilePipeOptions
    {
        public string FileName { get; set; }
    }

    public class FilePipe
    {
        public FilePipe(FilePipeOptions options)
        {

        }

        public PipeReader Reader { get; }

        private static async Task ProcessFile(string fileName, CancellationTokenSource cancellationTokenSource)
        {
            var pipe = new Pipe();
            var writer = pipe.Writer;

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var buffer = writer.GetMemory((int)fileStream.Length);
                int bytes = await fileStream.ReadAsync(buffer, cancellationTokenSource.Token);

            }
        }

        private static async Task FillPipeAsync(object socket, PipeWriter write) { }

        //private static async Task ReadPipeAsync(object socket, PipeWriter write) { }



        public async Task Stream(Pipe pipe, CancellationTokenSource cancellationTokenSource)
        {
            if (pipe == null)
            {
                cancellationTokenSource.Cancel();
                throw new ArgumentException(
                    message: $"[{nameof(pipe)}] is not provided."
                    , paramName: nameof(pipe));
            }

            using (var fileStream = new FileStream("", FileMode.Open, FileAccess.Read))
            {
                while (true)
                {
                    Memory<byte> buffer = pipe.Writer.GetMemory(1);
                    int bytes = await fileStream.ReadAsync(buffer, cancellationTokenSource.Token);
                    pipe.Writer.Advance(bytes);

                    if (bytes == 0)
                    {
                        // source EOF
                        break;
                    }

                    var flush = await pipe.Writer.FlushAsync(cancellationTokenSource.Token);
                    if (flush.IsCompleted || flush.IsCanceled)
                    {
                        break;
                    }
                }

                pipe.Writer.Complete();
            }
        }
    }
}
