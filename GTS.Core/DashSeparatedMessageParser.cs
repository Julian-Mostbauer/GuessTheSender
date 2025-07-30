using System.Globalization;

namespace GTS.Core;

public class DashSeparatedMessageParser : IMessageParser
{
    public bool IsNewMessageStart(string line)
    {
        // Format: dd.MM.yy, HH:mm - Sender: Message
        // Must be at least 18 characters long to contain a valid prefix
        if (line.Length < 18) return false;

        // Extract date and time prefix
        var prefix = line.Substring(0, 15); // "dd.MM.yy, HH:mm"
        var validTime = DateTime.TryParseExact(prefix, "dd.MM.yy, HH:mm", CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
        var startsWithDash = line[16] == '-';

        return validTime && startsWithDash;
    }

    public Message ParseMessage(IEnumerable<string> lines)
    {
        var enumerable = lines as string[] ?? lines.ToArray();

        var firstLine = enumerable.First();

        // Parse the timestamp
        var timestampEndIndex = firstLine.IndexOf(" - ", StringComparison.Ordinal);
        if (timestampEndIndex < 0)
            throw new FormatException("Message line does not contain expected ' - ' separator. Line: " + firstLine);

        var timePart = firstLine[..timestampEndIndex];
        var time = DateTime.ParseExact(timePart, "dd.MM.yy, HH:mm", CultureInfo.InvariantCulture);

        // Parse sender and first content line
        var senderContent = firstLine[(timestampEndIndex + 3)..];
        var senderEnd = senderContent.IndexOf(':');
        if (senderEnd < 0)
            throw new FormatException("Message does not contain expected ':' after sender. Line: " + firstLine);

        var sender = senderContent[..senderEnd].Trim();
        var contentLines = new List<string>
        {
            senderContent[(senderEnd + 1)..].Trim()
        };

        // Add remaining lines as content
        contentLines.AddRange(enumerable.Skip(1).Select(line => line.Trim()));

        var content = string.Join('\n', contentLines);

        return new Message(time, sender, content);
    }

    public Message ParseMessage(string line) => ParseMessage([line]);
}