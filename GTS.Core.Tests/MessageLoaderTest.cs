using System.Linq;
using GTS.Core;
using JetBrains.Annotations;
using Xunit;

namespace GTS.Core.Tests;

[TestSubject(typeof(MessageLoader))]
public class MessageLoaderTest
{
    [Fact]
    public void LoadsBracketMixedCorrectly()
    {
        var loader = MessageLoader.FromFile("/home/julian/Projects/GuessTheSender/GTS.Core.Tests/data/bracket_chat_mixed.txt", MessageParserProvider.Bracketed);
        var messages = loader.LoadMessages().ToArray();
        Assert.Equal(6, messages.Length);
        Assert.Equal("ASender", messages[0].Sender);
        Assert.Equal("A_1", messages[0].Content);
        Assert.Equal("ASender", messages[1].Sender);
        Assert.Equal("A_2_1\n\nA_2_2", messages[1].Content);
        Assert.Equal("BSender", messages[2].Sender);
        Assert.Equal("B_1", messages[2].Content);
        Assert.Equal("BSender", messages[3].Sender);
        Assert.Equal("B_2", messages[3].Content);
        Assert.Equal("BSender", messages[4].Sender);
        Assert.Equal("B_3", messages[4].Content);
        Assert.Equal("ASender", messages[5].Sender);
        Assert.Equal("A_3", messages[5].Content);
    }

    [Fact]
    public void LoadsBracketSingleOnlyCorrectly()
    {
        var loader = MessageLoader.FromFile("/home/julian/Projects/GuessTheSender/GTS.Core.Tests/data/bracket_chat_single_only.txt", MessageParserProvider.Bracketed);
        var messages = loader.LoadMessages().ToArray();
        Assert.Equal(4, messages.Length);
        Assert.Equal("ASender", messages[0].Sender);
        Assert.Equal("A_1", messages[0].Content);
        Assert.Equal("BSender", messages[1].Sender);
        Assert.Equal("B_1", messages[1].Content);
        Assert.Equal("ASender", messages[2].Sender);
        Assert.Equal("C_2", messages[2].Content);
        Assert.Equal("CSender", messages[3].Sender);
        Assert.Equal("C_1", messages[3].Content);
    }

    [Fact]
    public void LoadsDashedMixedCorrectly()
    {
        var loader = MessageLoader.FromFile("/home/julian/Projects/GuessTheSender/GTS.Core.Tests/data/dashed_chat_mixed.txt", MessageParserProvider.Dashed);
        var messages = loader.LoadMessages().ToArray();
        Assert.Equal(5, messages.Length);
        Assert.Equal("BSender Hadžić", messages[0].Sender);
        Assert.Equal("B_1", messages[0].Content);
        Assert.Equal("CSender", messages[1].Sender);
        Assert.Equal("C_1", messages[1].Content);
        Assert.Equal("BSender Hadžić", messages[2].Sender);
        Assert.Equal("B_2_1\nB_2_2\nB_2_3\nB_2_4\nB_2_5", messages[2].Content);
        Assert.Equal("CSender", messages[3].Sender);
        Assert.Equal("C_2_1", messages[3].Content);
        Assert.Equal("ASender", messages[4].Sender);
        Assert.Equal("A_1", messages[4].Content);
    }

    [Fact]
    public void LoadsDashedSingleOnlyCorrectly()
    {
        var loader = MessageLoader.FromFile("/home/julian/Projects/GuessTheSender/GTS.Core.Tests/data/dashed_chat_single_only.txt", MessageParserProvider.Dashed);
        var messages = loader.LoadMessages().ToArray();
        Assert.Equal(5, messages.Length);
        Assert.Equal("ASender", messages[0].Sender);
        Assert.Equal("A_1", messages[0].Content);
        Assert.Equal("ASender", messages[1].Sender);
        Assert.Equal("A_2", messages[1].Content);
        Assert.Equal("BSender", messages[2].Sender);
        Assert.Equal("B_1", messages[2].Content);
        Assert.Equal("BSender", messages[3].Sender);
        Assert.Equal("B_2", messages[3].Content);
        Assert.Equal("BSender", messages[4].Sender);
        Assert.Equal("B_3", messages[4].Content);
    }
}