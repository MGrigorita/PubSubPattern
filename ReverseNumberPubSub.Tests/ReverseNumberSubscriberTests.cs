using Moq;
using PubSubPatternModels;
using System.Reflection;
using System.Security.Cryptography;

namespace ReverseNumberPubSub.Tests
{
    [TestClass]
    public class ReverseNumberSubscriberTests
    {
        #region Helper Methods
        
        private List<Delegate> FindPublisherSubscribedDelegates(IPublisher publisher, string evName)
        {
            BindingFlags filter = BindingFlags.Instance | BindingFlags.NonPublic;

            FieldInfo eventField = publisher.GetType().GetField(evName, filter);

            Delegate eh = (Alert)eventField.GetValue(publisher);

            if (eh == null)
            {
                return new List<Delegate>();
            }

            return eh.GetInvocationList().ToList();
        }

        #endregion

        #region Subscribe Tests

        [TestMethod]
        public void Subscribe_WhenGivenReverseNumberPublisher_SubscribesToOnPublishEvent()
        {
            // Arrange
            var subscriber = new ReverseNumberSubscriber("1");
            var publisher = new ReverseNumberPublisher();

            // Act
            subscriber.Subscribe(publisher);

            // Assert
            var subscribedDelegates = FindPublisherSubscribedDelegates(publisher, "OnPublish");
            Assert.IsTrue(subscribedDelegates.Any(d => Object.ReferenceEquals(subscriber, d.Target)));
        }

        [TestMethod]
        public void Subscribe_WhenNotGivenReverseNumberPublisher_DoesNotSubscribeToOnPublishEvent()
        {
            // Arrange
            var subscriber = new ReverseNumberSubscriber("1");
            var otherPublisher = new MockPublisher();

            // Act
            subscriber.Subscribe(otherPublisher);

            // Assert
            var subscribedDelegates = FindPublisherSubscribedDelegates(otherPublisher, "OnPublish");
            Assert.IsFalse(subscribedDelegates.Any(d => Object.ReferenceEquals(subscriber, d.Target)));
        }

        #endregion

        #region Unsubscribe Tests

        [TestMethod]
        public void Unsubscribe_WhenGivenReverseNumberPublisher_UnsubscribesFromOnPublishEvent()
        {
            // Arrange
            var subscriber = new ReverseNumberSubscriber("1");
            var publisher = new ReverseNumberPublisher();
            subscriber.Subscribe(publisher);

            // Act
            subscriber.Unsubscribe(publisher);

            // Assert
            var subscribedDelegates = FindPublisherSubscribedDelegates(publisher, "OnPublish");
            Assert.IsFalse(subscribedDelegates.Any(d => Object.ReferenceEquals(subscriber, d.Target)));
        }

        #endregion

        #region OnAlertReceived Tests

        [TestMethod]
        public void OnAlertReceived_WhenCalled_WritesReceivedMessageToConsole()
        {
            // Arrange
            var subscriber = new ReverseNumberSubscriber("1");
            var publisher = new ReverseNumberPublisher();
            var notificationEvent = new ReverseNumberNotificationEvent(5);

            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            subscriber.OnAlertReceived(publisher, notificationEvent);

            // Assert
            Assert.AreEqual($"Subscriber {subscriber.GetName()} received from Publisher {publisher.GetName()}: {notificationEvent.Message}\r\n", consoleOutput.ToString());
        }

        #endregion

        #region GetName Tests

        [TestMethod]
        public void GetName_WhenConstructorCalled_ReturnsCorrectName()
        {
            // Arrange
            var subscriber = new ReverseNumberSubscriber("1");
            
            // Act & Assert
            Assert.AreEqual(subscriber.GetName(), "Subscriber-1");
        }

        #endregion
    }
}
