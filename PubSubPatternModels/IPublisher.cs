namespace PubSubPatternModels
{
    public delegate void Alert(IPublisher publisher, INotificationEvent notificationEvent);

    public interface IPublisher
    {
        event Alert? OnPublish;

        string GetName();

        void Publish();
    }
}
