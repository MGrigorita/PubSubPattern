using Moq;
using PubSubPatternModels;

namespace ReverseNumberPubSub.Tests
{
    [TestClass]
    public class ReverseNumberPublisherTests
    {
        #region EnqueueMessage Tests

        [TestMethod]
        public void EnqueueMessage_WhenPositiveNumberInputEnqueued_ReturnsTrue()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "123";

            // Act
            bool result = publisher.EnqueueMessage(input);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EnqueueMessage_WhenNegativeNumberInputEnqueued_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "-123";

            // Act
            bool result = publisher.EnqueueMessage(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EnqueueMessage_WhenNullInputEnqueued_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = null;

            // Act
            bool result = publisher.EnqueueMessage(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EnqueueMessage_WhenEmptyInputEnqueued_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = string.Empty;

            // Act
            bool result = publisher.EnqueueMessage(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EnqueueMessage_WhenNotNumberInputEnqueued_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "a1290";

            // Act
            bool result = publisher.EnqueueMessage(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EnqueueMessage_WhenZeroLeadingNumberInputEnqueued_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "05628912";

            // Act
            bool result = publisher.EnqueueMessage(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EnqueueMessage_WhenNumberTooBigEnqueued_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string largeNumber = "12345678901234567890";

            // Act and Assert
            bool result = publisher.EnqueueMessage(largeNumber);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region ValidateInput Tests

        public void ValidateInput_WhenPositiveNumberInput_ReturnsTrue()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "123";

            // Act
            bool result = publisher.ValidateInput(input);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateInput_WhenNegativeNumberInput_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "-123";

            // Act
            bool result = publisher.ValidateInput(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateInput_WhenNullInput_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = null;

            // Act
            bool result = publisher.ValidateInput(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateInput_WhenEmptyInput_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = string.Empty;

            // Act
            bool result = publisher.ValidateInput(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateInput_WhenNotNumberInput_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "a1290";

            // Act
            bool result = publisher.ValidateInput(input);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateInput_WhenZeroLeadingNumberInput_ReturnsFalse()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "05628912";

            // Act
            bool result = publisher.ValidateInput(input);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region ReverseNumber Tests

        public void ReverseNumber_WhenValidInput_ReturnsReversedNumber()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "5628912";

            // Act
            long result = publisher.ReverseNumber(input);

            // Assert
            Assert.AreEqual(result, 2198265);
        }

        public void ReverseNumber_WhenNumberTooBig_ThrowsOverflowException()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            string input = "12345678901234567890";

            // Act & Assert
            Assert.ThrowsException<OverflowException>(() => publisher.ReverseNumber(input));
        }

        #endregion

        #region Publish Tests

        [TestMethod]
        public void Publish_WhenNoSubscribers_DoNotInvokeOnPublishEvent()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();

            bool eventInvoked = false;
            publisher.OnPublish += (s, e) => eventInvoked = true;

            // Act
            publisher.Publish();

            // Assert
            Assert.IsFalse(eventInvoked);
        }

        [TestMethod]
        public void Publish_WhenSubscribersAndNoMessagesAvailable_DoNotInvokeOnPublishEvent()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            var subscriberMock = new Mock<ReverseNumberSubscriber>("1");
            bool eventInvoked = false;

            subscriberMock.Setup(s => s.Subscribe(It.IsAny<ReverseNumberPublisher>()))
                .Callback((IPublisher p) => { p.OnPublish += (s, e) => eventInvoked = true; });

            subscriberMock.Object.Subscribe(publisher);

            // Act
            publisher.Publish();

            // Assert
            Assert.IsFalse(eventInvoked);
        }

        [TestMethod]
        public void Publish_WhenSubscribersAndMessagesAvailable_InvokeOnPublishEvent()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            var subscriberMock = new Mock<ReverseNumberSubscriber>("1");
            bool eventInvoked = false;

            subscriberMock.Setup(s => s.Subscribe(It.IsAny<ReverseNumberPublisher>()))
                .Callback((IPublisher p) => { p.OnPublish += (s, e) => eventInvoked = true; });


            subscriberMock.Object.Subscribe(publisher);

            publisher.EnqueueMessage("123");

            // Act
            publisher.Publish();

            // Assert
            Assert.IsTrue(eventInvoked);
        }

        [TestMethod]
        public void Publish_WhenSubscribersAndMessagesAvailable_InvokeSubscriberOnAlertReceived()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();
            var subscriberMock = new Mock<ReverseNumberSubscriber>("1");
            var notificationEvent = new ReverseNumberNotificationEvent(123);

            subscriberMock.Setup(s => s.Subscribe(It.IsAny<ReverseNumberPublisher>()))
                .Callback((IPublisher p) => { p.OnPublish += subscriberMock.Object.OnAlertReceived; });
            subscriberMock.Setup(s => s.OnAlertReceived(It.IsAny<ReverseNumberPublisher>(), It.IsAny<ReverseNumberNotificationEvent>()))
                .Verifiable();

            subscriberMock.Object.Subscribe(publisher);
            publisher.EnqueueMessage("123");

            // Act
            publisher.Publish();

            // Assert
            subscriberMock.Verify(s => s.OnAlertReceived(It.IsAny<ReverseNumberPublisher>(), It.IsAny<ReverseNumberNotificationEvent>()), Times.Once);
        }

        #endregion

        #region GetName Tests

        [TestMethod]
        public void GetName_WhenDefaultConstructorCalled_ReturnsEmptyName()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher();

            // Act & Assert
            Assert.AreEqual(publisher.GetName(), "Publisher-");
        }

        [TestMethod]
        public void GetName_WhenParameterizedConstructorCalled_ReturnsNameWithId()
        {
            // Arrange
            var publisher = new ReverseNumberPublisher("1");

            // Act & Assert
            Assert.AreEqual(publisher.GetName(), "Publisher-1");
        }

        #endregion
    }
}
