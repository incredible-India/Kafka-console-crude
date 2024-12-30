using Confluent.Kafka;

var topic = "read-Student";
var config = new ConsumerConfig
{
    GroupId = "test-consumer-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
consumer.Subscribe(topic);
CancellationTokenSource cts = new CancellationTokenSource();
try
{
    while (true)
    {
        var response = consumer.Consume(cts.Token);

        Console.WriteLine($"Student List Received: {response.Message.Value}\n");
    }
}
catch (ConsumeException e)
{
    Console.WriteLine($"Error occured: {e.Error.Reason}");
}
catch (OperationCanceledException exc)
{
    Console.WriteLine(exc.Message);
    throw;
}
