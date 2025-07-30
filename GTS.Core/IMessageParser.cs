namespace GTS.Core;

public interface IMessageParser
{
    bool IsNewMessageStart(string line);
    Message ParseMessage(IEnumerable<string> lines);
    Message ParseMessage(string line);
}

public static class MessageParserProvider
{
    public static IMessageParser Dashed { get; } = new DashSeparatedMessageParser();
    public static IMessageParser Bracketed { get; } = new BracketedMessageParser();
}