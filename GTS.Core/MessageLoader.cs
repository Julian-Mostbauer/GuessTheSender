namespace GTS.Core;

public class MessageLoader
{
    private readonly string? _filePath;
    private readonly string? _content;
    private readonly string[]? _unwantedSenders;

    private MessageLoader(string? filePath, string? content, string[]? unwantedSenders)
    {
        _filePath = filePath;
        _content = content;
        _unwantedSenders = unwantedSenders;
    }

    public static MessageLoader FromFile(string filePath, params string[]? unwantedSenders)
        => new MessageLoader(filePath, null, unwantedSenders);

    public static MessageLoader FromContent(string content, params string[]? unwantedSenders)
        => new MessageLoader(null, content, unwantedSenders);

    private bool IsAllowed(string sender)
    {
        if (_unwantedSenders == null || _unwantedSenders.Length == 0)
            return true;

        return !_unwantedSenders.Contains(sender, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<Message> LoadMessages()
    {
        IEnumerable<string> lines;
        if (_filePath != null)
            lines = File.ReadLines(_filePath);
        else if (_content != null)
            lines = _content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        else
            throw new InvalidOperationException("No source specified for loading messages.");

        var buffer = new List<string>();

        foreach (var line in lines)
        {
            if (line.StartsWith('‎')) continue; // Skip lines starting with a zero-width space

            if (line.StartsWith('[') && buffer.Count > 0)
            {
                var msg = Message.ParseSingle(string.Join('\n', buffer));
                if (IsAllowed(msg.Sender) && !msg.Content.StartsWith('‎')) yield return msg;
                buffer.Clear();
            }

            buffer.Add(line);
        }

        if (buffer.Count > 0)
        {
            var msg = Message.ParseSingle(string.Join('\n', buffer));
            if (IsAllowed(msg.Sender)) yield return msg;
        }
    }
}