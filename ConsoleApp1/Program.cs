// See https://aka.ms/new-console-template for more information

using ReverseNumberPubSub;

var reversePublisher = new ReverseNumberPublisher("1");

var subscriber1 = new ReverseNumberSubscriber("1");
var subscriber2 = new ReverseNumberSubscriber("2");
var subscriber3 = new ReverseNumberSubscriber("3");

subscriber1.Subscribe(reversePublisher);
subscriber2.Subscribe(reversePublisher);
subscriber3.Subscribe(reversePublisher);


var enqueuingThread = new Thread(() =>
{
    while (true)
    {
        Console.Write("Enter a positive valid number:");
        var input = Console.ReadLine();

        if (reversePublisher.EnqueueMessage(input))
        {
            reversePublisher.Publish();
        }
    }
});

enqueuingThread.Start();

Thread.Sleep(30*1000); // wait 30s to unsubscribe subscriber2
subscriber2.Unsubscribe(reversePublisher);


Thread.Sleep(30 * 1000); // wait 30s to unsubscribe subscriber3
subscriber3.Unsubscribe(reversePublisher);
