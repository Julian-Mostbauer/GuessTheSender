namespace GTS.Core;

public class MessageLoaderOptions
{
    public string[]? UnwantedSenders { get; set; }
    public int? MinMessageLength { get; set; }
    public int? MaxMessageLength { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}

public class MessageLoader
{
    private readonly string? _filePath;
    private readonly string? _content;
    private readonly MessageLoaderOptions? _options;

    private MessageLoader(string? filePath, string? content, MessageLoaderOptions? options = null)
    {
        _filePath = filePath;
        _content = content;
        _options = options;
    }

    public static MessageLoader FromFile(string filePath, MessageLoaderOptions? options = null)
        => new MessageLoader(filePath, null, options);

    public static MessageLoader FromContent(string content, MessageLoaderOptions? options = null)
        => new MessageLoader(null, content, options);

    private bool IsAllowed(Message msg)
    {
        if (_options?.UnwantedSenders is { Length: > 0 } &&
            _options.UnwantedSenders.Contains(msg.Sender, StringComparer.OrdinalIgnoreCase))
            return false;
        if (_options?.MinMessageLength.HasValue == true && (msg.Content?.Length ?? 0) < _options.MinMessageLength.Value)
            return false;
        if (_options?.MaxMessageLength.HasValue == true && (msg.Content?.Length ?? 0) > _options.MaxMessageLength.Value)
            return false;
        if (_options?.StartTime.HasValue == true && msg.Time < _options.StartTime.Value)
            return false;
        if (_options?.EndTime.HasValue == true && msg.Time > _options.EndTime.Value)
            return false;

        return true;
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
                if (IsAllowed(msg) && !msg.Content.StartsWith('‎')) yield return msg;
                buffer.Clear();
            }

            buffer.Add(line);
        }

        if (buffer.Count > 0)
        {
            var msg = Message.ParseSingle(string.Join('\n', buffer));
            if (IsAllowed(msg)) yield return msg;
        }
    }
}