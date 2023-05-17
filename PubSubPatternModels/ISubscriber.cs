namespace PubSubPatternModels
{
    public interface ISubscriber
    {
        string GetName();

        void Subscribe(IPublisher publisher);

        void Unsubscribe(IPublisher publisher);
    }
}
