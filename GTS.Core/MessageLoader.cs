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
    private readonly IMessageParser _parser;

    private MessageLoader(string? filePath, string? content, IMessageParser parser,
        MessageLoaderOptions? options = null)
    {
        _filePath = filePath;
        _content = content;
        _parser = parser;
        _options = options;
    }

    public static MessageLoader FromFile(string filePath, IMessageParser parser, MessageLoaderOptions? options = null)
        => new MessageLoader(filePath, null, parser, options);

    public static MessageLoader FromContent(string content, IMessageParser parser, MessageLoaderOptions? options = null)
        => new MessageLoader(null, content, parser, options);

    private bool IsAllowed(Message msg)
    {
        if (msg.Content.StartsWith('‎'))
            return false;
        if (msg.Content == "<Medien ausgeschlossen>")
            return false;
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
        var lines = _filePath != null
            ? File.ReadLines(_filePath)
            : _content?.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
              ?? throw new InvalidOperationException("No source specified for loading messages.");

        var buffer = new List<string>();

        foreach (var line in lines)
        {
            if (line.Contains('‎')) continue;

            if (_parser.IsNewMessageStart(line) && buffer.Count > 0)
            {
                var msg = _parser.ParseMessage(buffer);
                if (IsAllowed(msg)) yield return msg;
                buffer.Clear();
            }

            buffer.Add(line);
        }

        if (buffer.Count > 0)
        {
            var msg = _parser.ParseMessage(buffer);
            if (IsAllowed(msg)) yield return msg;
        }
    }
}