using PubSubPatternModels;

namespace ReverseNumberPubSub
{
    public class ReverseNumberNotificationEvent : INotificationEvent
    {
        public object Message { get; set; }

        public ReverseNumberNotificationEvent(long message)
        {
            Message = message;
        }

        public long GetMessageAsNumber()
        {
            return (long)Message;
        }
    }
}
