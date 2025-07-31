using System;
using GTS.Core;
using GTS.Core.MessageParserImplementations;
using JetBrains.Annotations;
using Xunit;

namespace GTS.Core.Tests;

[TestSubject(typeof(BracketedMessageParser))]
public class BracketedMessageParserTest
{
    [Fact]
    public void IsNewMessageStart_ReturnsTrue_ForValidStart()
    {
        var parser = new BracketedMessageParser();
        Assert.True(parser.IsNewMessageStart("[01.01.22, 12:00:00] Sender: Message content"));
    }

    [Fact]
    public void IsNewMessageStart_ReturnsFalse_ForInvalidStart()
    {
        var parser = new BracketedMessageParser();
        Assert.False(parser.IsNewMessageStart("01.01.22, 12:00:00] Sender: Message content"));
    }

    [Fact]
    public void ParseMessage_ParsesValidMessage()
    {
        var parser = new BracketedMessageParser();
        var message = parser.ParseMessage("[01.01.22, 12:00:00] Sender: Message content");

        Assert.Equal(new DateTime(2022, 1, 1, 12, 0, 0), message.Time);
        Assert.Equal("Sender", message.Sender);
        Assert.Equal("Message content", message.Content);
    }

    [Fact]
    public void ParseMessage_ThrowsFormatException_ForInvalidFormat()
    {
        var parser = new BracketedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("Invalid message format"));
    }
    
    [Fact]
    public void ParseMessage_ThrowsFormatException_ForMissingSender()
    {
        var parser = new BracketedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("[01.01.22, 12:00:00] : Message content"));
    }
    
    [Fact]
    public void ParseMessage_ThrowsFormatException_ForMissingContent()
    {
        var parser = new BracketedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("[01.01.22, 12:00:00] Sender: "));
    }
    
    [Fact]
    public void ParseMessage_ThrowsFormatException_ForMissingTime()
    {
        var parser = new BracketedMessageParser();
        Assert.Throws<FormatException>(() => parser.ParseMessage("Sender: Message content"));
    }
    
    [Fact]
    public void ParseMessage_ValidMultilineMessage()
    {
        var parser = new BracketedMessageParser();
        var message = parser.ParseMessage([
            "[01.01.22, 12:00:00] Sender:",
            "This is a multiline",
            "message content."
        ]);

        Assert.Equal(new DateTime(2022, 1, 1, 12, 0, 0), message.Time);
        Assert.Equal("Sender", message.Sender);
        Assert.Equal("This is a multiline\nmessage content.", message.Content);
    }
}