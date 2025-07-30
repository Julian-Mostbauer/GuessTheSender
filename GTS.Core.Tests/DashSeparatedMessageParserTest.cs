using System;
using GTS.Core;
using JetBrains.Annotations;
using Xunit;

namespace GTS.Core.Tests;

[TestSubject(typeof(DashSeparatedMessageParser))]
public class DashSeparatedMessageParserTest
{
    [Fact]
    public void IsNewMessageStart_ReturnsTrue_ForValidStart()
    {
        var parser = new DashSeparatedMessageParser();
        Assert.True(parser.IsNewMessageStart("22.07.25, 15:16 - Almir Hadžić: e"));
    }

    [Fact]
    public void IsNewMessageStart_ReturnsFalse_ForInvalidStart()
    {
        var parser = new DashSeparatedMessageParser();
        Assert.False(parser.IsNewMessageStart("Message content"));
    }

    [Fact]
    public void ParseMessage_ParsesValidMessage()
    {
        var parser = new DashSeparatedMessageParser();
        var message = parser.ParseMessage("22.07.25, 15:16 - Sender: e");

        Assert.Equal(new DateTime(2025, 7, 22, 15, 16, 0), message.Time);
        Assert.Equal("Sender", message.Sender);
        Assert.Equal("e", message.Content);
    }

    [Fact]
    public void ParseMessage_ThrowsFormatException_ForInvalidFormat()
    {
        var parser = new DashSeparatedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("Invalid message format"));
    }

    [Fact]
    public void ParseMessage_ThrowsFormatException_ForMissingSender()
    {
        var parser = new DashSeparatedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("01.01.22, 12:00:00 - : Message content"));
    }

    [Fact]
    public void ParseMessage_ThrowsFormatException_ForMissingContent()
    {
        var parser = new DashSeparatedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("01.01.22, 12:00:00 - Sender: "));
    }

    [Fact]
    public void ParseMessage_ThrowsFormatException_ForMissingTime()
    {
        var parser = new DashSeparatedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("Sender: Message content"));
    }

    [Fact]
    public void ParseMessage_ValidMultilineMessage()
    {
        var parser = new DashSeparatedMessageParser();
        var message = parser.ParseMessage([
            "20.07.25, 23:32 - Sender: Ich kannt einmal der hat scheiße gelabert.",
            "Was mit dem jetzt?",
            "Der ist jetzt tot.",
            "Achso.",
            "Ok."
        ]);

        Assert.Equal(new DateTime(2025, 7, 20, 23, 32, 0), message.Time);
        Assert.Equal("Sender", message.Sender);
        Assert.Equal("Ich kannt einmal der hat scheiße gelabert.\nWas mit dem jetzt?\nDer ist jetzt tot.\nAchso.\nOk.",
            message.Content);
    }
}