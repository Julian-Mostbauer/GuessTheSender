using GTS.Core;

var loader = MessageLoader.FromFile("/home/julian/Projects/GuessTheSender/GTS.AppConsole/data/chat.txt",
    MessageParserProvider.Bracketed, new MessageLoaderOptions { UnwantedSenders = ["Ireland Trifecta", "Meta AI"] });

var messages = loader.LoadMessages().ToList();
var random = new Random();

Console.WriteLine("found {0} messages", messages.Count);
while (true)
{
    var message = messages[random.Next(messages.Count)];
    Console.WriteLine("==========================");
    Console.WriteLine(message.Content);
    Console.WriteLine("==========================");

    Console.WriteLine("Press J, M, K to guess or 'q' to quit...");
    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.Q) break;

    Console.WriteLine(
        char.ToLower(key.KeyChar) == message.Sender.ToLower()[0]
            ? "Correct! The sender was: {0}"
            : "Wrong! The sender was: {0}", message.Sender);
}