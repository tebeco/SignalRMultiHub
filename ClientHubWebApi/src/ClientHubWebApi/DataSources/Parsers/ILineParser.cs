using System.Text;

public interface ILineParser
{
    void ParseLine(string line);
    void ParseLine(char[] line);
    void Dump();
    void ParseLine(StringBuilder line);
}
}
