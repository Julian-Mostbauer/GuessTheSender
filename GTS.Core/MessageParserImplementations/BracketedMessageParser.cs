namespace GTS.Core.MessageParserImplementations;

using System.Globalization;

public class BracketedMessageParser : IMessageParser
{
    public bool IsNewMessageStart(string line)
    {
        if (!line.StartsWith('[')) return false;

        var endIdx = line.IndexOf(']');
        if (endIdx <= 1) return false; // empty or malformed

        var inside = line.Substring(1, endIdx - 1);

        // Only consider it a message start if the bracketed token parses as a timestamp
        // in the expected WhatsApp export format: "dd.MM.yy, HH:mm:ss".
        return DateTime.TryParseExact(inside, "dd.MM.yy, HH:mm:ss", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out _);
    }

    public Message ParseMessage(IEnumerable<string> lines) => ParseMessage(string.Join('\n', lines));

    public Message ParseMessage(string line)
    {
        var parts = line.Split([']'], 2);
        if (parts.Length < 2) throw new FormatException("Invalid message format in line: " + line);

        var timePart = parts[0].TrimStart('[');
        var rest = parts[1].Trim();
        var senderEnd = rest.IndexOf(':');
        if (senderEnd < 0) throw new FormatException("Missing sender separator ':' in message: " + line);

        var sender = rest[..senderEnd].Trim();
        if (string.IsNullOrWhiteSpace(sender)) throw new FormatException("Sender cannot be empty." + line);
        var content = rest[(senderEnd + 1)..].Trim();

        var time = DateTime.ParseExact(timePart, "dd.MM.yy, HH:mm:ss", null);

        return new Message(time, sender, content);
    }
}