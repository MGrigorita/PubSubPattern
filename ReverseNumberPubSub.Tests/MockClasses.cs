using PubSubPatternModels;

namespace ReverseNumberPubSub.Tests
{
    public class MockPublisher : IPublisher
    {
        public MockPublisher() { }

        public event Alert? OnPublish;

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public void Publish()
        {
            throw new NotImplementedException();
        }
    }
}
