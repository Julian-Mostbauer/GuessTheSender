using GTS.Core;

namespace GTS.AppWeb.Services
{
    public class MessageService
    {
        public Message[]? Messages { get; private set; }

        public void SetMessages(Message[]? messages)
        {
            Messages = messages;
        }
    }
}
