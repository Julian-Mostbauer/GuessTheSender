using GTS.Core;
namespace GTS.AppWeb.Services;

public class MessageService
{
    public Message[]? Messages { get; private set; }
    public string[]? Senders { get; private set; }

    public void SetMessages(Message[]? messages)
    {
        Messages = messages;
        Senders = messages?.Select(m => m.Sender).Distinct().ToArray();
    }

    public void ClearMessages()
    {
        Messages = null;
        Senders = null;
    }
}