using PubSubPatternModels;

namespace ReverseNumberPubSub
{
    public class ReverseNumberSubscriber : ISubscriber
    {
        public string Id { get; }

        public ReverseNumberSubscriber(string id)
        {
            Id = id;
        }

        public virtual string GetName()
        {
            return $"Subscriber-{Id}";
        }

        public virtual void Subscribe(IPublisher revPublisher)
        {
            // subscribe only to ReverseNumberPublisher implementations
            if (revPublisher is ReverseNumberPublisher)
            {
                revPublisher.OnPublish += OnAlertReceived;
            }
        }

        public virtual void Unsubscribe(IPublisher revPublisher)
        {
            revPublisher.OnPublish -= OnAlertReceived;
        }

        public virtual void OnAlertReceived(IPublisher revPublisher, INotificationEvent revEvent)
        {
            Console.WriteLine($"Subscriber {GetName()} received from Publisher {revPublisher.GetName()}: {revEvent.Message}");
        }
    }
}
