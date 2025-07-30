namespace GTS.Core;

public record Message(DateTime Time, string Sender, string Content)
{
    public static Message ExampleMessage => new(
        new DateTime(2000, 1, 1, 9, 30, 0),
        "Alice",
        "Hello, Bob!"
    );

    public override string ToString()
        => $"|[{Time:yyyy-MM-dd HH:mm:ss}] by {Sender}: {Content}|";

    public static Message ParseSingle(string line)
    {
        // [15.05.25, 22:11:51] Alice: Hello, Bob!
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentException("Line cannot be null or empty.", nameof(line));

        var parts = line.Split(new[] { ']' }, 2);

        if (parts.Length < 2)
            throw new FormatException("Line format is invalid. Expected format: [Time] Sender: Content");

        var timePart = parts[0].TrimStart('[');
        var senderContentPart = parts[1].Trim();
        var senderEndIndex = senderContentPart.IndexOf(':');

        if (senderEndIndex < 0)
            throw new FormatException("Line format is invalid. Expected format: [Time] Sender: Content");

        var time = DateTime.ParseExact(timePart, "dd.MM.yy, HH:mm:ss", null);

        var sender = senderContentPart.Substring(0, senderEndIndex).Trim();
        var content = senderContentPart.Substring(senderEndIndex + 1).Trim();

        return new Message(time, sender, content);
    }
}