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
        if (msg.Content.StartsWith('‎'))
            return false;
        if (msg.Content is "<Medien ausgeschlossen>" or "Diese Nachricht wurde gelöscht.")
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

    public Message[] LoadMessages()
    {
        // Try each parser until one succeeds
        foreach (var parser in MessageParserProvider.AllParsers)
        {
            try
            {
                var result = LoadMessages(parser).ToArray();
                if (result.Length > 0)
                    return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading messages with parser {parser.GetType().Name}: {e.Message}");
                Console.WriteLine("Trying next parser...");
            }
        }

        throw new InvalidOperationException("No suitable parser found for loading messages.");
    }

    public IEnumerable<Message> LoadMessages(IMessageParser parser)
    {
        var lines = _filePath != null
            ? File.ReadLines(_filePath)
            : _content?.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
              ?? throw new InvalidOperationException("No source specified for loading messages.");

        var buffer = new List<string>();

        foreach (var line in lines)
        {
            // status messages
            if (line.Contains('‎') || line.Contains(' ')) continue;

            var isLineStart = parser.IsNewMessageStart(line);

            // group name change message edge case
            if (isLineStart && !(line.Count(c => c == ':') > 1)) continue;

            if (isLineStart && buffer.Count > 0)
            {
                var msg = parser.ParseMessage(buffer);
                if (IsAllowed(msg)) yield return msg;
                buffer.Clear();
            }

            buffer.Add(line);
        }

        if (buffer.Count > 0)
        {
            var msg = parser.ParseMessage(buffer);
            if (IsAllowed(msg)) yield return msg;
        }
    }
}