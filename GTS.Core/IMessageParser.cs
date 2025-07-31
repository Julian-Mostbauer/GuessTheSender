using System.Reflection;
using GTS.Core.MessageParserImplementations;

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

    public static IEnumerable<IMessageParser> AllParsers { get; } = LoadParsers().ToArray();
    
    private static IEnumerable<IMessageParser> LoadParsers()
    {
        var parserType = typeof(IMessageParser);
        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false } && parserType.IsAssignableFrom(t));

        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is IMessageParser parser)
                yield return parser;
        }
    }
}