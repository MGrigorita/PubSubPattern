using PubSubPatternModels;

namespace ReverseNumberPubSub
{
    public class ReverseNumberPublisher : IPublisher
    {
        public string Id { get; }

        public event Alert? OnPublish;

        protected Queue<long> MessageQueue;

        public ReverseNumberPublisher()
        {
            Id = string.Empty;
            MessageQueue = new Queue<long>();
        }

        public ReverseNumberPublisher(string id) : this()
        {
            Id = id;
        }

        public virtual string GetName()
        {
            return $"Publisher-{Id}";
        }

        public virtual bool EnqueueMessage(string input)
        {
            var enqueued = true;
            try
            {
                if (ValidateInput(input))
                {
                    var number = ReverseNumber(input);
                    MessageQueue.Enqueue(number);
                }
                else
                {
                    enqueued = false;
                    Console.WriteLine("Invalid number");
                }
            }
            catch (OverflowException)
            {
                enqueued = false;
                Console.WriteLine("Inputted number too big");
            }

            return enqueued;
        }

        public virtual void Publish()
        {
            // If any subscribers to alert and any messages to publish
            if (OnPublish != null && MessageQueue.Any())
            {
                // Alert all subscribers with the reversed inputted number
                var number = MessageQueue.Dequeue();
                var notificationEv = new ReverseNumberNotificationEvent(number);
                OnPublish(this, notificationEv);
            }
        }

        public virtual bool ValidateInput(string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit) && !input.StartsWith("0");
        }

        public virtual long ReverseNumber(string number)
        {
            return long.Parse(new string(number.Reverse().ToArray()));
        }
    }
}
