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
}