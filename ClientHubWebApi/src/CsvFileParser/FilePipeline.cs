using System;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace CsvFileParser
{
    public class FilePipe
    {
        private static async Task<PipeReader> GetFilePipeReader(string fileName, CancellationTokenSource cancellationTokenSource)
        {
            var pipe = new Pipe();
            var writer = pipe.Writer;

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                Memory<byte> buffer = pipe.Writer.GetMemory((int)fileStream.Length);
                int bytes = await fileStream.ReadAsync(buffer, cancellationTokenSource.Token);
                pipe.Writer.Advance(bytes);

                await pipe.Writer.FlushAsync(cancellationTokenSource.Token);

                pipe.Writer.Complete();
            }

            return pipe.Reader;
        }
    }
}
