using System.Text;

public interface ILineParser<T>
{
    T ParseLine(StringBuilder line);

    void Dump();
}
